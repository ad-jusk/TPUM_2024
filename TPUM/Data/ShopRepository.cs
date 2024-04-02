using System.Collections.ObjectModel;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Data.DataModels;
using ClientData.Interfaces;
using ClientData;
using Common;
using System.ComponentModel;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        public event EventHandler<ChangePriceEventArgs>? PriceChanged;
        public event Action? ItemsUpdated;
        public event Action<bool>? TransactionFinish;

        private readonly Dictionary<Guid, IInstrument> items = new Dictionary<Guid, IInstrument>();
        private readonly object instrumentsLock = new object();

        private readonly IConnectionService connectionService;

        public ShopRepository(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
            this.connectionService.OnMessage += OnMessage;
        }

        private void OnMessage(string message)
        {

            if (message.Contains(UpdateAllResponse.SHeader))
            {
                UpdateAllResponse response = Serializer.Deserialize<UpdateAllResponse>(message);
                UpdateAllProducts(response);
            }
            else if (message.Contains(PriceChangedResponse.SHeader))
            {
                PriceChangedResponse response = Serializer.Deserialize<PriceChangedResponse>(message);
                UpdateAllPrices(response);
            }
            else if (message.Contains(TransactionResponse.SHeader))
            {
                TransactionResponse response = Serializer.Deserialize<TransactionResponse>(message);
                if (response.Succeeded)
                {
                    RequestInstruments();
                    TransactionFinish?.Invoke(true);
                }
                else
                {
                    TransactionFinish?.Invoke(false);
                }
            }
        }

        private void UpdateAllProducts(UpdateAllResponse response)
        {
            if (response.Items == null)
                return;

            lock (instrumentsLock)
            {
                items.Clear();
                foreach (InstrumentDTO item in response.Items)
                {
                    items.Add(item.Id, item.ToInstrument());
                }
            }

            ItemsUpdated?.Invoke();
        }

        private void UpdateAllPrices(PriceChangedResponse response)
        {
            if (response.NewPrices == null)
                return;

            lock (instrumentsLock)
            {
                foreach (var newPrice in response.NewPrices)
                {
                    if (items.ContainsKey(newPrice.ItemID))
                    {
                        items[newPrice.ItemID].Price = newPrice.NewPrice;
                    }
                }
            }
            PriceChanged?.Invoke(this, new ChangePriceEventArgs(0f));
        }

        public async Task RequestInstruments()
        {
            await connectionService.SendAsync(Serializer.Serialize(new GetItemsCommand()));
        }

        public async void RequestServerForUpdate()
        {
            if (connectionService.IsConnected())
            {
                await RequestInstruments();
            }
        }

        public async Task SellInstrument(Guid itemId)
        {
            if (connectionService.IsConnected())
            {
                SellItemCommand sellItemCommand = new SellItemCommand{ ItemId = itemId };
                await connectionService.SendAsync(Serializer.Serialize(sellItemCommand));
            }
        }

        public List<IInstrument> GetInstruments()
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (instrumentsLock)
            {
                result.AddRange(items.Values.Select(i => new Instrument(i)));
            }
            return result;
        }

        public IInstrument GetInstrumentById(Guid guid)
        {
            IInstrument result;
            lock (instrumentsLock)
            {
                if (items.ContainsKey(guid))
                {
                    result = items[guid];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return result;
        }

        public List<IInstrument> GetInstrumentsByType(InstrumentType type)
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (instrumentsLock)
            {
                result.AddRange(items.Values
                    .Where(i => i.Type == type)
                    .Select(i => new Instrument(i)));
            }

            return result;
        }
    }
}

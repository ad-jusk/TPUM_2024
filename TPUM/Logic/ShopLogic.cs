using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    internal class ShopLogic : IShopLogic, IObserver<InflationChangedEventArgs>, IObserver<float>
    {
        private readonly IShopData shop;

        public event EventHandler<LogicInflationChangedEventArgs>? InflationChanged;
        public event EventHandler<float>? CustomerFundsChanged;
        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        private IDisposable shopDataSubscriptionHandle;
        private IDisposable customerFundsSubscriptionHandle;

        public ShopLogic(IShopData shopData)
        {
            this.shop = shopData;

            shopDataSubscriptionHandle = shopData.Subscribe((IObserver<InflationChangedEventArgs>) this);
            customerFundsSubscriptionHandle = shopData.Subscribe((IObserver<float>) this);

            shopData.InstrumentsUpdated += () => InstrumentsUpdated?.Invoke();
            shopData.TransactionFinish += (bool succeeded) => TransactionFinish?.Invoke(succeeded);
        }

        public async Task SellInstrument(Guid instrumentId)
        {
            List<IInstrumentLogic> instruments = shop.GetInstruments()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
            IInstrumentLogic? foundItem = instruments.Find((item) => item.Id == instrumentId);
            if (foundItem != null)
            {
                await shop.SellInstrument(foundItem.Id);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public List<IInstrumentLogic> GetInstruments()
        {
            return shop.GetInstruments()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
        }

        public List<IInstrumentLogic> GetInstrumentsByType(LogicInstrumentType type)
        {
            return shop.GetInstrumentsByType((InstrumentType)type)
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
        }

        public void OnCompleted()
        {
            shopDataSubscriptionHandle.Dispose();
            customerFundsSubscriptionHandle.Dispose();
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(InflationChangedEventArgs value)
        {
            InflationChanged?.Invoke(this, new LogicInflationChangedEventArgs(value));
        }

        public void OnNext(float value)
        {
            CustomerFundsChanged?.Invoke(this, value);
        }
    }
}
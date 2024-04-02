using ClientLogic.Enums;
using ClientLogic.Interfaces;
using Tpum.Data;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class ShopLogic : IShopLogic
    {
        public event EventHandler<ChangePriceEventArgsLogic> PriceChanged;
        public event Action? ItemsUpdated;
        public event Action<bool>? TransactionFinish;

        private readonly IShopRepository shop;

        public ShopLogic(IShopRepository shop)
        {
            this.shop = shop;
            shop.PriceChanged += HandleOnPriceChanged;
            shop.TransactionFinish += (bool succeeded) => TransactionFinish?.Invoke(succeeded);
            shop.ItemsUpdated += () => ItemsUpdated?.Invoke();
        }

        public List<IInstrumentLogic> GetInstruments()
        {
            return shop.GetInstruments()
                .Select(i => new InstrumentLogic(i))
                .Cast<IInstrumentLogic>()
                .ToList();
        }

        public List<IInstrumentLogic> GetInstrumentsByType(InstrumentTypeLogic instrumentType)
        {
            return shop
                .GetInstrumentsByType((InstrumentType)instrumentType)
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
        }

        public void RequestServerForUpdate()
        {
            shop.RequestServerForUpdate();
        }

        public async Task SellInstrument(Guid itemId)
        {
            List<IInstrumentLogic> items = 
                shop.GetInstruments()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
            IInstrumentLogic? foundItem = items.Find((item) => item.Id == itemId);
            if (foundItem != null && foundItem.Quantity > 0)
            {
                await shop.SellInstrument(foundItem.Id);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        private void HandleOnPriceChanged(object sender, ChangePriceEventArgs args)
        {
            PriceChanged?.Invoke(this, new ChangePriceEventArgsLogic(args));
        }
    }
}

using ServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    internal class ShopLogic : IShopLogic
    {
        private readonly IShopData shop;

        public event EventHandler<InflationChangedEventArgsLogic>? InflationChanged;

        public ShopLogic(IShopData shop)
        {
            this.shop = shop;
            shop.InflationChanged += HandleOnInflationChanged;
        }

        private void HandleOnInflationChanged(object sender, PriceInflationEventArgs args)
        {
            InflationChanged?.Invoke(this, new InflationChangedEventArgsLogic(args));
        }

        public void SellInstrument(Guid itemId)
        {
            List<IInstrumentLogic> items = shop
                .GetInstruments()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
            IInstrumentLogic? foundItem = items.Find(item => item.Id == itemId);
            if (foundItem != null && foundItem.Quantity > 0)
            {
                shop.SellInstrument(foundItem.Id);
            }
        }

        public List<IInstrumentLogic> GetInstruments()
        {
            return shop
                .GetInstruments()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
        }
    }
}
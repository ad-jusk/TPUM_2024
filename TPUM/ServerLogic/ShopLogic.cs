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

        public event EventHandler<PriceChangedEventArgsLogic>? PriceChanged;

        public ShopLogic(IShopData shop)
        {
            this.shop = shop;
            shop.PriceChanges += HandleOnInflationChanged;
        }

        private void HandleOnInflationChanged(object sender, PriceChangedEventArgs args)
        {
            PriceChanged?.Invoke(this, new PriceChangedEventArgsLogic(args));
        }

        public void SellItem(Guid itemId)
        {
            List<IInstrumentLogic> items = shop
                .GetItems()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
            IInstrumentLogic? foundItem = items.Find(item => item.Id == itemId);
            if (foundItem != null && foundItem.Quantity > 0)
            {
                shop.SellItem(foundItem.Id);
            }
        }

        public List<IInstrumentLogic> GetItems()
        {
            return shop
                .GetItems()
                .Select(item => new InstrumentLogic(item))
                .Cast<IInstrumentLogic>()
                .ToList();
        }
    }
}

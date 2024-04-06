using Data;

namespace Logic
{
    internal class Logic : LogicAbstractApi
    {
        private readonly IShopLogic shop;

        public Logic(DataAbstractApi dataApi) : base(dataApi)
        {
            this.shop = new ShopLogic(dataApi.GetShop());
        }

        public override IShopLogic GetShop()
        {
            return shop;
        }
    }
}
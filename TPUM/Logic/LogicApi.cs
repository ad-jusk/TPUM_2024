using Data;

namespace Logic
{
    internal class Logic : LogicAbstractApi
    {
        private readonly IShopLogic shop;
        private readonly IConnectionServiceLogic connectionService;

        public Logic(DataAbstractApi dataApi) : base(dataApi)
        {
            this.shop = new ShopLogic(dataApi.GetShop());
            this.connectionService = new ConnectionServiceLogic(dataApi.GetConnectionService());
        }

        public override IConnectionServiceLogic GetConnectionService()
        {
            return connectionService;
        }

        public override IShopLogic GetShop()
        {
            return shop;
        }
    }
}
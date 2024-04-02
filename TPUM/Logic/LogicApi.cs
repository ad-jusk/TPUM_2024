using ClientLogic;
using ClientLogic.Interfaces;
using Logic;
using Tpum.Data;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class LogicApi : LogicAbstractApi
    {
        private readonly IShopLogic shop;
        private readonly IConnectionServiceLogic connectionService;

        public LogicApi(DataAbstractApi dataApi) : base(dataApi)
        {
            this.shop = new ShopLogic(dataApi.GetShopRepository());
            this.connectionService = new ConnectionServiceLogic(dataApi.GetConnectionService());
        }

        public override IShopLogic GetStore()
        {
            return shop;
        }

        public override IConnectionServiceLogic GetConnectionService()
        {
            return connectionService;
        }
    }
}

using Tpum.ServerData;
using Tpum.ServerLogic.Interfaces;

namespace Tpum.ServerLogic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly IStore store;

        public LogicApi(DataAbstractApi dataApi) : base(dataApi)
        {
            this.store = new Store(dataApi.GetShopRepository());
        }

        public override IStore GetStore()
        {
            return store;
        }
    }
}

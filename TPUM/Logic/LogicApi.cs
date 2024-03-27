using Logic;
using Tpum.Data;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class LogicApi : LogicAbstractApi
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

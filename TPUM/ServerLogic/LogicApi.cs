using ServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly IShopLogic shop;

        public LogicApi(DataAbstractApi dataApi) : base(dataApi)
        {
            shop = new ShopLogic(dataApi.GetShop());
        }

        public override IShopLogic GetShop()
        {
            return shop;
        }
    }
}

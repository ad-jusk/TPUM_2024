using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataApi : DataAbstractApi
    {
        private readonly IShopData shop;
        private readonly IConnectionService connectionService;

        public DataApi(IConnectionService? connectionService)
        {
            this.connectionService = connectionService ?? new ConnectionService();
            this.shop = new ShopData(this.connectionService);
        }

        public override IConnectionService GetConnectionService()
        {
            return connectionService;
        }

        public override IShopData GetShop()
        {
            return shop;
        }
    }
}

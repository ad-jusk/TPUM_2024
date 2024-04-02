using ClientData;
using ClientData.Interfaces;
using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    public class DataApi : DataAbstractApi
    {
        private readonly IShopRepository shopRepository;
        private readonly IConnectionService connectionService;
        
        public DataApi(IConnectionService? connectionService)
        {
            this.connectionService = connectionService ?? new ConnectionService();
            this.shopRepository = new ShopRepository(this.connectionService);
        }

        public override IConnectionService GetConnectionService()
        {
            return connectionService;
        }

        public override IShopRepository GetShopRepository()
        {
            return shopRepository;
        }
    }
}

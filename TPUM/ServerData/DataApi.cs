using Tpum.ServerData.Interfaces;

namespace Tpum.ServerData
{
    public class DataApi : DataAbstractApi
    {
        private readonly IShopRepository shopRepository;
        
        public DataApi()
        {
            this.shopRepository = new ShopRepository();
        }

        public override IShopRepository GetShopRepository()
        {
            return shopRepository;
        }
    }
}

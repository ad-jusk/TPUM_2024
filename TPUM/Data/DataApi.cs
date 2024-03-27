using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    internal class DataApi : DataAbstractApi
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

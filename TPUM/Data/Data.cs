using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    internal class Data : IData
    {
        public IShopRepository ShopRepository { get ; set; }
        
        internal Data(IShopRepository shopRepository = default)
        {
            ShopRepository = shopRepository /*?? new ShopRepository()*/;
        }
        public static Data Create()
        {
            return new Data();
        }
    }
}

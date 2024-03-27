using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    public abstract class DataAbstractApi
    {
        public static DataApi Create()
        {
            return new DataApi();
        }

        public abstract IShopRepository GetShopRepository();
    }
}

using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    public abstract class DataAbstractApi
    {
        public static DataAbstractApi Create()
        {
            return new DataApi();
        }

        public abstract IShopRepository GetShopRepository();
    }
}

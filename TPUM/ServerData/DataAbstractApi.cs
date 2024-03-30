using Tpum.ServerData.Interfaces;

namespace Tpum.ServerData
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

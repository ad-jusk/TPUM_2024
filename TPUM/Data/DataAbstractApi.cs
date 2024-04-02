using ClientData.Interfaces;
using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    public abstract class DataAbstractApi
    {
        public static DataApi Create(IConnectionService? connectionService = null)
        {
            return new DataApi(connectionService);
        }

        public abstract IShopRepository GetShopRepository();

        public abstract IConnectionService GetConnectionService();
    }
}

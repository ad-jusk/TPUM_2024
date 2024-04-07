using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerData
{
    internal class DataApi : DataAbstractApi
    {
        private readonly IShopData shop;

        public DataApi()
        {
            shop = new ShopData();
        }

        public override IShopData GetShop()
        {
            return shop;
        }
    }
}
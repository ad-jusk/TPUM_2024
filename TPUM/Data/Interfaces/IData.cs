using Tpum.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tpum.Data.Interfaces
{
    public interface IData
    {
        IShopRepository ShopRepository { get; set; }

        public static IData Create(IShopRepository shopRepository = default)
        {
            return new Data(shopRepository);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic
{
    public class ChangeProductPriceEventArgs : EventArgs
    {
        public ChangeProductPriceEventArgs(Guid id, decimal price)
        {
            Id = id;
            Price = price;
        }

        public Guid Id { get; }
        public decimal Price { get; }
    }
}

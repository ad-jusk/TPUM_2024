using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic
{
    public class ChangeProductQuantityEventArgs : EventArgs
    {
        public ChangeProductQuantityEventArgs(Guid id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public int Quantity { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic;

namespace  Tpum.Logic.Interfaces
{
    public interface IBasket
    {
        public bool Addproduct(InsturmentDTO instrument);
        public bool RemoveProduct(InsturmentDTO instrument);
    }
}

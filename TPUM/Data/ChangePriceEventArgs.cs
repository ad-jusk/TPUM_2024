using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data
{
    public class ChangePriceEventArgs : EventArgs
    {
        public float NewPrice { get; }

        public ChangePriceEventArgs(float newPrice)
        {
            this.NewPrice = newPrice;
        }
    }
}

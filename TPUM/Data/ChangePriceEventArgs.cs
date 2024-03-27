using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data
{
    public class ChangePriceEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceEventArgs(decimal newFunds)
        {
            this.NewFunds = newFunds;
        }
    }
}

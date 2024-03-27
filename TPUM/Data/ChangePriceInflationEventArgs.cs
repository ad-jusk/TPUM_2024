using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data
{
    public class ChangePriceInflationEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceInflationEventArgs(decimal newFunds)
        {
            this.NewFunds = newFunds;
        }
    }
}

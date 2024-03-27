using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic
{
    public class ChangePriceInflationEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceInflationEventArgs(decimal newfunds)
        {
            this.NewFunds = newfunds;
        }

        internal ChangePriceInflationEventArgs(Tpum.Data.ChangePriceInflationEventArgs args)
        {
            this.NewFunds = args.NewFunds;
        }
    }
}

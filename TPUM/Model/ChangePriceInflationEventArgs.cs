using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Presentation.Model
{
    public class ChangePriceInflationEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceInflationEventArgs(decimal newFunds)
        {
            this.NewFunds = newFunds;
        }

        internal ChangePriceInflationEventArgs(Tpum.Logic.ChangePriceInflationEventArgs args)
        {
            this.NewFunds = args.NewFunds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Presentation.Model
{
    public class ChangePriceEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceEventArgs(decimal newFunds)
        {
            this.NewFunds = newFunds;
        }

        internal ChangePriceEventArgs(Tpum.Logic.ChangePriceEventArgs args)
        {
            this.NewFunds = args.NewFunds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic
{
    public class ChangePriceEventArgs : EventArgs
    {
        public decimal NewFunds { get; }

        public ChangePriceEventArgs(decimal newfunds)
        {
            this.NewFunds = newfunds;
        }

        internal ChangePriceEventArgs(Tpum.Data.ChangePriceEventArgs args)
        {
            this.NewFunds = args.NewFunds;
        }
    }
}

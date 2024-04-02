using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data;

namespace Tpum.Logic
{
    public class ChangePriceEventArgsLogic : EventArgs
    {
        public float NewPrice { get; }

        public ChangePriceEventArgsLogic(float newPrice)
        {
            this.NewPrice = newPrice;
        }

        internal ChangePriceEventArgsLogic(ChangePriceEventArgs args)
        {
            this.NewPrice = args.NewPrice;
        }
    }
}

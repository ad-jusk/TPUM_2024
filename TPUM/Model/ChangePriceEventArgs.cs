﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic;

namespace Tpum.Presentation.Model
{
    public class ChangePriceEventArgs : EventArgs
    {
        public float NewPrice { get; }

        public ChangePriceEventArgs(float newPrice)
        {
            this.NewPrice = newPrice;
        }

        internal ChangePriceEventArgs(ChangePriceEventArgsLogic args)
        {
            this.NewPrice = args.NewPrice;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.ServerLogic
{
    public class ChangeConsumerFundsEventArgs : EventArgs
    {
        public ChangeConsumerFundsEventArgs(decimal funds)
        {
            Funds = funds;
        }

        public decimal Funds { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;

namespace Tpum.Logic.Interfaces
{
    public interface ILogic
    {
        public IStore Store { get; }

        public static ILogic Create(IData data = default)
        {
            return new Logic(data ?? IData.Create());
        }
    }
}

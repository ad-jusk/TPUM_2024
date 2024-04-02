using ClientLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Interfaces
{
    public interface IInstrumentLogic
    {
        public Guid Id { get; }
        public string Name { get; }
        public InstrumentTypeLogic Type { get; }
        public float Price { get; }
        public int Year { get; }
        public int Quantity { get; }
    }
}

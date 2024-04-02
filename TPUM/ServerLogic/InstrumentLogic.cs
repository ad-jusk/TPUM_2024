using ServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    internal class InstrumentLogic : IInstrumentLogic
    {

        public InstrumentLogic(IInstrument instrument)
        {
            this.Id = instrument.Id;
            this.Name = instrument.Name;
            this.Type = (InstrumentTypeLogic) instrument.Type;
            this.Price = instrument.Price;
            this.Year = instrument.Year;
            this.Quantity = instrument.Quantity;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public InstrumentTypeLogic Type { get; private set; }
        public float Price { get; private set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

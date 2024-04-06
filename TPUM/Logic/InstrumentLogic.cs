using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class InstrumentLogic : IInstrumentLogic
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public LogicInstrumentType Type { get; private set; }
        public float Price { get; private set; }
        public int Year { get; set; }
        public int Quantity { get; set; }

        public InstrumentLogic(IInstrument instrument)
        {
            Id = instrument.Id;
            Name = instrument.Name;
            Type = (LogicInstrumentType)instrument.Type;
            Price = instrument.Price;
            Year = instrument.Year;
            Quantity = instrument.Quantity;
        }
    }
}

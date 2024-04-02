using ClientLogic.Enums;
using ClientLogic.Interfaces;
using System.ComponentModel;
using Tpum.Data.Interfaces;

namespace Tpum.Logic
{
    public class InstrumentLogic : IInstrumentLogic
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public InstrumentTypeLogic Type { get; private set; }
        public float Price { get; private set; }
        public int Year { get; private set; }
        public int Quantity { get; private set; }

        public InstrumentLogic(IInstrument i)
        {
            Id = i.Id;
            Name = i.Name;
            Type = (InstrumentTypeLogic)i.Type;
            Price = i.Price;
            Year = i.Year;
            Quantity = i.Quantity;
        }
    }
}

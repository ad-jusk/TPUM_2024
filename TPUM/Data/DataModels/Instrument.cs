using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace Tpum.Data.DataModels
{
    public class Instrument : IInstrument
    {
        public Instrument(string instrumentName, InstrumentType instrumentCategory, float instrumentPrice, int year, int quantity)
        {
            Id = Guid.NewGuid();
            Name = instrumentName;
            Type = instrumentCategory;
            Price = instrumentPrice;
            Year = year;
            Quantity = quantity;
        }

        public Instrument(Guid id, string instrumentName, InstrumentType instrumentCategory, float instrumentPrice, int year, int quantity)
        {
            Id = id;
            Name = instrumentName;
            Type = instrumentCategory;
            Price = instrumentPrice;
            Year = year;
            Quantity = quantity;
        }

        public Instrument(IInstrument i)
        {
            Id = i.Id;
            Name = i.Name;
            Type = i.Type;
            Price = i.Price;
            Year = i.Year;
            Quantity = i.Quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public InstrumentType Type { get; set; }
        public float Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

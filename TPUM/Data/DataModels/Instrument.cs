using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace Tpum.Data.DataModels
{
    public class Instrument : IInstrument
    {
        public Instrument(string instrumentName, InstrumentCategory instrumentCategory, decimal instrumentPrice, int year, int quantity)
        {
            Name = instrumentName;
            Category = instrumentCategory;
            Price = instrumentPrice;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

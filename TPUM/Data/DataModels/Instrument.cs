using System.Text.Json.Serialization;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace Tpum.Data.DataModels
{
    public class Instrument : IInstrument
    {

        [JsonConstructor]
        public Instrument(string name, Guid id, InstrumentCategory category, decimal price, int year, int quantity)
        {
            Name = name;
            Id = id;
            Category = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Instrument(string name, InstrumentCategory category, decimal price, int year, int quantity)
        {
            Name = name;
            Category = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

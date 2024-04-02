using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace LogicTest
{
    public class InstrumentMock : IInstrument
    {

        public InstrumentMock(string name, InstrumentType category, float price, int year, int quantity) { 
            Id = Guid.NewGuid();
            Name = name;
            Type = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public InstrumentType Type { get; set; }
        public float Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        float IInstrument.Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

using Tpum.ServerData.Enums;

namespace Tpum.ServerData.Interfaces
{
    public interface IInstrument
    {
        public Guid Id { get; }
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Tpum.ServerLogic
{
    // DTO -> data transfer object
    public class InstrumentDTO
    {

        [JsonConstructor]
        public InstrumentDTO(Guid id, string name, string category, decimal price, int year, int quantity)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public int Year { get; private set; }
        public int Quantity { get; private set; }
    }
}

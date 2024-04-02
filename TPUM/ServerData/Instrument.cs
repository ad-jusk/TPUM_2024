using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerData
{
    internal class Instrument : IInstrument
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public InstrumentType Type { get; }
        public float Price { get; set; }
        public int Year { get; }
        public int Quantity { get; set; }

        public Instrument(string name, InstrumentType type, float price, int year, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            Price = price;
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

        public override bool Equals(object? obj)
        {
            if(obj == null || obj is not Instrument)
            {
                return false;
            }
            else if(obj == this)
            {
                return true;
            }
            return this.Id.Equals(((Instrument)obj).Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}

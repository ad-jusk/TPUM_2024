using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class Instrument : IInstrument
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public InstrumentType Type { get; }
        public float Price { get; set; }
        public int Year { get; }
        public int Quantity { get; set; }

        public Instrument(Guid id, string name, InstrumentType type, float price, int year, int quantity)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public object Clone()
        {
            Instrument clone = (Instrument)MemberwiseClone();
            clone.Name = string.Copy(Name);
            return clone;
        }

        private bool Equals(IInstrument other)
        {
            return Id.Equals(other.Id) && Name == other.Name && Type == other.Type && Price.Equals(other.Price) && Year.Equals(other.Year) && Quantity.Equals(other.Quantity);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Instrument)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = Id.GetHashCode();
            hashCode = (hashCode * 17) ^ Name.GetHashCode();
            hashCode = (hashCode * 17) ^ (int)Type;
            hashCode = (hashCode * 17) ^ Price.GetHashCode();
            hashCode = (hashCode * 17) ^ Year.GetHashCode();
            hashCode = (hashCode * 17) ^ Quantity.GetHashCode();
            return hashCode;
        }
    }
}

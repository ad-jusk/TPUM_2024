using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tpum.Data.DataModels
{
    public class Instrument : IInstrument
    {
        public Instrument(string instrumentName, InstrumentCategory instrumentCategory, decimal instrumentPrice, decimal instrumentAge)
        {
            Name = instrumentName;
            Category = instrumentCategory;
            Price = instrumentPrice;
            Age = instrumentAge;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public decimal Age { get; set; }
    }
}

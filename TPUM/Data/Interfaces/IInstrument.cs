using Tpum.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data.Interfaces
{
    public interface IInstrument
    {
        public Guid Id { get; }
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public decimal Age { get; set; }
    }
}

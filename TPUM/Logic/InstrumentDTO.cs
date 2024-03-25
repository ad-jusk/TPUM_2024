using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Enums;

namespace Tpum.Logic
{
    // DTO -> data transfer object
    public class InstrumentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Age { get; set; }
    }
}

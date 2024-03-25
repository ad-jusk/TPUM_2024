using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data.DataModels
{
    internal class Customer(string customerName, string customerLastname)
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; } = customerName;
        public string Lastname { get; } = customerLastname;
        public string Email { get; set; }
    }
}

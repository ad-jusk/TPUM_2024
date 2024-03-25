using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data
{
    public class ChangeProductAgeEventArgs : EventArgs
    {
        public ChangeProductAgeEventArgs(Guid id, decimal age)
        {
            Id = id;
            Age = age;
        }

        public Guid Id { get; }
        public decimal Age { get; }
    }
}

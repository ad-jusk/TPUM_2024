using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tpum.Data.Enums;

namespace Tpum.Presentation.Model
{
    public class InstrumentP : INotifyPropertyChanged
    {
        public InstrumentP(Guid instrumentId, string instrumentName, string instrumentCategory, decimal instrumentPrice, decimal instrumentAge)
        {
            Id = instrumentId;
            Name = instrumentName;
            Category = instrumentCategory;
            Price = instrumentPrice;
            Age = instrumentAge;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Age { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

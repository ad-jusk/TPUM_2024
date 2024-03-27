using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tpum.Data.Interfaces;

namespace Tpum.Presentation.Model
{
    public class InstrumentPresentation : INotifyPropertyChanged
    {
        public InstrumentPresentation(Guid instrumentId, string instrumentName, string instrumentCategory, decimal instrumentPrice, int year, int quantity)
        {
            Id = instrumentId;
            Name = instrumentName;
            Category = instrumentCategory;
            Price = instrumentPrice;
            Year = year;
            Quantity = quantity;
        }
        public InstrumentPresentation(IInstrument instrument)
        {
            Id = instrument.Id;
            Name = instrument.Name;
            Category = instrument.Category.ToString();
            Price = instrument.Price;
            Year = instrument.Year;
            Quantity = instrument.Quantity;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

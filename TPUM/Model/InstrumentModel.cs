using ClientLogic.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tpum.Presentation.Model
{
    public class InstrumentModel : INotifyPropertyChanged
    {
        public InstrumentModel(Guid instrumentId, string instrumentName, string instrumentCategory, float instrumentPrice, int year, int quantity)
        {
            Id = instrumentId;
            Name = instrumentName;
            Category = instrumentCategory;
            Price = instrumentPrice;
            Year = year;
            Quantity = quantity;
        }

        public InstrumentModel(IInstrumentLogic instrument)
        {
            Id = instrument.Id;
            Name = instrument.Name;
            Category = instrument.Type.ToString();
            Price = instrument.Price;
            Year = instrument.Year;
            Quantity = instrument.Quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; set; }
        public float Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

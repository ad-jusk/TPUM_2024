using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic;
using Presentation.Model;

namespace Model
{
    public class InstrumentPresentation : INotifyPropertyChanged
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public PresentationInstrumentType Type { get; private set; }
        public float Price { get; private set; }
        public int Year { get; private set; }
        public int Quantity { get; private set; }

        public InstrumentPresentation(IInstrumentLogic instrument)
        {
            Id = instrument.Id;
            Name = instrument.Name;
            Type = (PresentationInstrumentType)instrument.Type;
            Price = instrument.Price;
            Year = instrument.Year;
            Quantity = instrument.Quantity;
        }

        public InstrumentPresentation(Guid id, string name, PresentationInstrumentType type, float price, int year, int quantity)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
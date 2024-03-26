using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tpum.Presentation.Model;

namespace Tpum.Presentation.ViewModel
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<InstrumentPresentation> instruments;
        private string mainViewVisibility;
        private readonly Timer timer;
        private readonly Model.Model model;
        private decimal basketSum;

        public ViewModel()
        {
            this.model = new Model.Model();
            this.instruments = new ObservableCollection<InstrumentPresentation>(this.model.GetInstruments());
            this.mainViewVisibility = "a"; // model.MainViewVisibility;
            InstrumentButtonClick = new RelayCommand<Guid>((id) => InstrumentButtonClickHandler(id));
            model.Store.ProductQuantityChange += OnQuantityChanged;

            Assembly assembly = Assembly.GetExecutingAssembly();
        }

        public ICommand PianoButton { get; private set; }
        public ICommand GrandPianoButton { get; private set; }
        public ICommand GuitarrButton { get; private set; }
        public ICommand ClarinetButton { get; private set; }
        public ICommand BasketButton { get; private set; }
        public ICommand InstrumentButtonClick { get; set; }

        public string MainViewVisibility
        {
            get { return mainViewVisibility; }
            set
            {
                if (value.Equals(mainViewVisibility))
                    return;
                mainViewVisibility = value;
                OnPropertyChanged("MainViewVisibility");
            }
        }
        public ObservableCollection<InstrumentPresentation> Instruments
        {
            get
            {
                return instruments;
            }
            set
            {
                if (value.Equals(instruments))
                    return;
                instruments = value;
                OnPropertyChanged("Instruments");
            }
        }

        private void InstrumentButtonClickHandler(Guid id)
        {
            foreach (InstrumentPresentation instrument in model.GetInstruments())
            {
                if (instrument.Id.Equals(id))
                {

                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnQuantityChanged(object sender, Tpum.Presentation.Model.ChangeProductQuantityEventArgs e)
        {
            ObservableCollection<InstrumentPresentation> newInstruments = Instruments;
            InstrumentPresentation instrument = newInstruments.FirstOrDefault(x => x.Id == e.Id);
            int fruitIndex = newInstruments.IndexOf(instrument);
            newInstruments[fruitIndex].Quantity = e.Quantity;
            Instruments = new ObservableCollection<InstrumentPresentation>(newInstruments);
        }
    }
}

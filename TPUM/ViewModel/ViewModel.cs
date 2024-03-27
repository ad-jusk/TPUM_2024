using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tpum.Data.Enums;
using Tpum.Presentation.Model;

namespace Tpum.Presentation.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<InstrumentPresentation> instruments;
        private readonly Model.Model model;
        private float customerFunds;

        public ViewModel()
        {
            this.model = new Model.Model();
            this.instruments = new ObservableCollection<InstrumentPresentation>(this.model.GetInstruments());
            InstrumentButtonClick = new RelayCommand<Guid>((id) => InstrumentButtonClickHandler(id));
            StringButton = new RelayCommand<Guid>((id) => StringButtonClickHandler());
            WindButton = new RelayCommand<Guid>((id) => WindButtonClickHandler());
            PercussionButton = new RelayCommand<Guid>((id) => PercussionButtonClickHandler());
            model.Store.ProductQuantityChange += OnQuantityChanged;
            model.Store.ConsumerFundsChange += OnConsumerFundsChanged;
            model.Store.PriceChange += HandlePriceInflationChanged;
            customerFunds = 120700.0f;
        }

        public ICommand StringButton { get; private set; }
        public ICommand WindButton { get; private set; }
        public ICommand PercussionButton { get; private set; }
        public ICommand InstrumentButtonClick { get; set; }
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

        public float CustomerFunds
        {
            get { return customerFunds; }
            set
            {
                if (value != customerFunds)
                {
                    customerFunds = value;
                    OnPropertyChanged();
                }

            }
        }

        private void HandlePriceInflationChanged(object sender, ChangePriceEventArgs args)
        {
            List<InstrumentPresentation> displayed = new List<InstrumentPresentation>(Instruments);
            List<InstrumentPresentation> all = model.Store.GetInstruments();

            Instruments.Clear();

            all
                .Where(i =>  displayed.Contains(i))
                .ToList()
                .ForEach(i => Instruments.Add(i));
        }

        private void StringButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory(InstrumentCategory.String)
                .ForEach(i => Instruments.Add(i));
        }

        private void WindButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory(InstrumentCategory.Wind)
                .ForEach(i => Instruments.Add(i));
        }

        private void PercussionButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory(InstrumentCategory.Percussion)
                .ForEach(i => Instruments.Add(i));
        }

        private void InstrumentButtonClickHandler(Guid id)
        {
            int i = 0;

            foreach (InstrumentPresentation instrument in model.GetInstruments())
            {
                if (instrument.Id.Equals(id))
                {
                    //instrument.Quantity -= 1;
                    model.Store.DecrementInstrumentQuantity(id);
                    //if(CustomerFunds -(float)instrument.Price > 0) CustomerFunds -= (float)instrument.Price;
                    model.Store.ChangeConsumerFunds(id, (decimal)customerFunds);
                    break;
                }
                i++;
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
            int instrumentIndex = newInstruments.IndexOf(instrument);
            newInstruments[instrumentIndex].Quantity = e.Quantity;
            Instruments = new ObservableCollection<InstrumentPresentation>(newInstruments);
        }

        private void OnConsumerFundsChanged(object sender, Tpum.Presentation.Model.ChangeConsumerFundsEventArgs e)
        {
            CustomerFunds = (float)e.Funds;
        }
    }
}

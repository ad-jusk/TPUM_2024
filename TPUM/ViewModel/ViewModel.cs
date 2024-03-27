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
        private decimal consumerFunds;

        public ViewModel()
        {
            this.model = new Model.Model();
            this.instruments = new ObservableCollection<InstrumentPresentation>(this.model.GetInstruments());
            InstrumentButtonClick = new RelayCommand<Guid>(id => InstrumentButtonClickHandler(id));
            AllButton = new RelayCommand(AllButtonClickHandler);
            StringButton = new RelayCommand(StringButtonClickHandler);
            WindButton = new RelayCommand(WindButtonClickHandler);
            PercussionButton = new RelayCommand(PercussionButtonClickHandler);
            model.Store.ProductQuantityChange += OnQuantityChanged;
            model.Store.ConsumerFundsChange += OnConsumerFundsChanged;
            model.Store.PriceChange += HandlePriceInflationChanged;
            consumerFunds = this.model.Store.GetConsumerFunds();
        }

        public ICommand AllButton { get; private set; }
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

        public decimal CustomerFunds
        {
            get { return consumerFunds; }
            set
            {
                if (value != consumerFunds)
                {
                    consumerFunds = value;
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

        private void AllButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstruments()
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
            InstrumentPresentation? instrument = model.Store.GetInstrumentById(id);
            if (instrument == null || instrument.Price > consumerFunds)
            {
                return;
            }
            model.Store.DecrementInstrumentQuantity(id);
            model.Store.ChangeConsumerFunds(id);
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
            CustomerFunds = e.Funds;
        }
    }
}

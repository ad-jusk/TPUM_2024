using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tpum.Presentation.Model;

namespace Tpum.Presentation.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<InstrumentPresentation> instruments;
        private readonly Model.Model model;
        private decimal consumerFunds;
        private string transactionStatusText;

        public ViewModel()
        {
            this.model = new Model.Model();

            InstrumentButtonClick = new RelayCommand<Guid>(id => InstrumentButtonClickHandler(id));
            AllButton = new RelayCommand(AllButtonClickHandler);
            StringButton = new RelayCommand(StringButtonClickHandler);
            WindButton = new RelayCommand(WindButtonClickHandler);
            PercussionButton = new RelayCommand(PercussionButtonClickHandler);

            model.Store.ConsumerFundsChangeCS += OnConsumerFundsChanged;
            model.Store.TransactionSucceeded += OnTransactionSucceeded;
            model.Store.InstrumentChange += OnInstrumentChanged;

            instruments = new ObservableCollection<InstrumentPresentation>();
           
            foreach (InstrumentPresentation instrument in model.Store.GetInstruments())
            {
                Instruments.Add(instrument);
            }
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
        public string TransactionStatusText
        {
            get
            {
                return transactionStatusText;
            }
            set
            {
                if (value.Equals(transactionStatusText))
                    return;
                transactionStatusText = value;
                OnPropertyChanged("TransactionStatusText");
            }
        }

        private void AllButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstruments()
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("RequestInstruments");
        }

        private void StringButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("String")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("RequestString");
        }

        private void WindButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("Wind")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("RequestWind");
        }

        private void PercussionButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("Percussion")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("RequestPercussion");
        }

        private void InstrumentButtonClickHandler(Guid id)
        {
            InstrumentPresentation? instrument = model.Store.GetInstrumentById(id);
            if (instrument == null || instrument.Price > consumerFunds)
            {
                return;
            }

            Task.Run(async () => await model.Store.SellInstrument(instrument));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnConsumerFundsChanged(object? sender, decimal funds)
        {
            CustomerFunds = funds;
        }

        private void OnInstrumentChanged(object? sender, InstrumentPresentation e)
        {
            ObservableCollection<InstrumentPresentation> newInstruments = new ObservableCollection<InstrumentPresentation>(Instruments);
            InstrumentPresentation Instrument = newInstruments.FirstOrDefault(x => x.Id == e.Id);

            if (Instrument != null)
            {
                int InstrumentIndex = newInstruments.IndexOf(Instrument);

                if (e.Category.ToLower() == "deleted")
                {
                    newInstruments.RemoveAt(InstrumentIndex);
                }
                else
                {
                    newInstruments[InstrumentIndex].Name = e.Name;
                    newInstruments[InstrumentIndex].Price = e.Price;
                    newInstruments[InstrumentIndex].Category = e.Category;
                    newInstruments[InstrumentIndex].Year = e.Year;
                    newInstruments[InstrumentIndex].Quantity = e.Quantity;
                }
            }
            else
            {
                newInstruments.Add(e);
            }

            Instruments = new ObservableCollection<InstrumentPresentation>(newInstruments);
        }

        private void OnTransactionSucceeded(object? sender, InstrumentPresentation e)
        {
            TransactionStatusText = "Succesfully bought product" + e.Name.ToString();
        }
    }
}

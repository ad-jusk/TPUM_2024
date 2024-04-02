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
        private string connectButtonText;
        private string transactionStatusText;

        public ViewModel()
        {
            this.model = new Model.Model();
            this.instruments = new ObservableCollection<InstrumentPresentation>(this.model.GetInstruments());
            InstrumentButtonClick = new RelayCommand<Guid>(id => InstrumentButtonClickHandler(id));
            AllButton = new RelayCommand(AllButtonClickHandler);
            StringButton = new RelayCommand(StringButtonClickHandler);
            WindButton = new RelayCommand(WindButtonClickHandler);
            PercussionButton = new RelayCommand(PercussionButtonClickHandler);
            //ConnectButton = new RelayCommand(async () => await ConnectButtonClickHandler());
            ConnectButton = new RelayCommand(() => ConnectButtonClickHandler());
            model.Store.ProductQuantityChange += OnQuantityChanged;
            model.Store.ConsumerFundsChange += OnConsumerFundsChanged;
            model.Store.PriceChange += HandlePriceInflationChanged;
            model.Store.TransactionSucceeded += OnTransactionSucceeded;
            consumerFunds = this.model.Store.GetConsumerFunds();
            instruments = new ObservableCollection<InstrumentPresentation>();
            foreach (InstrumentPresentation instrument in model.Store.GetInstruments())
            {
                Instruments.Add(instrument);
            }
            model.Store.InstrumentChange += OnInstrumentChanged;

        }

        public ICommand AllButton { get; private set; }
        public ICommand StringButton { get; private set; }
        public ICommand WindButton { get; private set; }
        public ICommand PercussionButton { get; private set; }
        public ICommand InstrumentButtonClick { get; set; }
        public ICommand ConnectButton{ get; set; }

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
        public string ConnectButtonText
        {
            get
            {
                return connectButtonText;
            }
            set
            {
                if (value.Equals(connectButtonText))
                    return;
                connectButtonText = value;
                OnPropertyChanged("ConnectButtonText");
            }
        }

        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged("Log");
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
        private string _log = "Waiting for connection logs...";
        private async Task ConnectButtonClickHandler()
        {
            model.Store.SendMessageAsync("ConnectButtonClick");
            if (!model.Store.IsConnected())
            {
                ConnectButtonText = "łączenie";
                bool result = await model.Store.Connect(new Uri("ws://localhost:8081"));
                if (result)
                {
                    ConnectButtonText = "połączono";
                    Instruments.Clear();
                    foreach (InstrumentPresentation instrument in model.Store.GetInstruments())
                    {
                        Instruments.Add(instrument);
                    }
                }
            }
            else
            {
                await model.Store.Disconnect();
                ConnectButtonText = "rozłączono";
                Instruments.Clear();
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
            model.Store.SendMessageAsync("AllButtonClick");
        }

        private void StringButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("String")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("StringButtonClick");
        }

        private void WindButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("Wind")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("WindButtonClick");
        }

        private void PercussionButtonClickHandler()
        {
            Instruments.Clear();
            model.Store.GetInstrumentsByCategory("Percussion")
                .ForEach(i => Instruments.Add(i));
            model.Store.SendMessageAsync("PercussionButtonClick");
        }

        private void InstrumentButtonClickHandler(Guid id)
        {
            InstrumentPresentation? instrument = model.Store.GetInstrumentById(id);
            if (instrument == null || instrument.Price > consumerFunds)
            {
                return;
            }
            //model.Store.DecrementInstrumentQuantity(id);
            //model.Store.ChangeConsumerFunds(id);
            Task.Run(async () => await model.Store.SellInstrument(instrument));
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

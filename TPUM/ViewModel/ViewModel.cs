using GalaSoft.MvvmLight.Command;
using Model;
using Presentation.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Presentation.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private enum CurrentTab
        {
            String = 0,
            Wind = 1,
            Percussion = 2,
            All = 3
        }

        private CurrentTab currentTab;

        private ObservableCollection<InstrumentPresentation> instruments;
        public ObservableCollection<InstrumentPresentation> Instruments
        {
            get { return instruments; }
            private set
            {
                if(instruments != value) 
                {
                    instruments = value;
                    OnPropertyChanged(nameof(Instruments));
                }
            }
        }

        private float customerFunds;
        public float CustomerFunds
        {
            get { return customerFunds; }
            private set
            {
                customerFunds = value;
                OnPropertyChanged(nameof(CustomerFunds));
            }
        }

        private readonly Model.Model model;

        public ViewModel()
        {
            this.model = new Model.Model(null);

            model.Shop.OnInstrumentsUpdated += HandleOnInstrumentsUpdated;
            model.Shop.InflationChanged += HandleInflationChanged;
            model.Shop.CustomerFundsChanged += HandleCustomerFundsChanged;
            model.Shop.TransactionFinish += HandleTransactionFinish;

            Instruments = new AsyncObservableCollection<InstrumentPresentation>();
            currentTab = CurrentTab.All;

            AllButton = new RelayCommand(() => HandleOnAllButton());
            StringButton = new RelayCommand(() => HandleOnStringButton());
            PercussionButton = new RelayCommand(() => HandleOnPercussionButton());
            WindButton = new RelayCommand(() => HandleOnWindButton());
            InstrumentButtonClick = new RelayCommand<Guid>(id => HandleOnInstrumentButtonClick(id));
        }

        public async Task CloseConnection()
        {
            await model.Disconnect();
        }

        private void HandleCustomerFundsChanged(float funds)
        {
            CustomerFunds = funds;
        }

        private void HandleTransactionFinish(bool success)
        {
            
        }

        private void HandleOnInstrumentsUpdated()
        {
            RefreshInstruments();
        }

        private void HandleInflationChanged(object sender, ModelInflationChangedEventArgs args)
        {
            RefreshInstruments();
        }

        private void RefreshInstruments()
        {
            instruments.Clear();

            if(currentTab == CurrentTab.All)
            {
                List<InstrumentPresentation> allInstruments = model.Shop.GetInstruments();
                foreach(var i in allInstruments)
                {
                    instruments.Add(i);
                }
            }
            else
            {
                List<InstrumentPresentation> typeInstruments = model.Shop.GetInstrumentsByType((PresentationInstrumentType)currentTab);
                foreach (var i in typeInstruments)
                {
                    instruments.Add(i);
                }
            }
        }

        public ICommand AllButton { get; private set; }
        private void HandleOnAllButton()
        {
            currentTab = CurrentTab.All;
            RefreshInstruments();
        }

        public ICommand StringButton { get; private set; }
        private void HandleOnStringButton()
        {
            currentTab = CurrentTab.String;
            RefreshInstruments();
        }

        public ICommand WindButton { get; private set; }
        private void HandleOnWindButton()
        {
            currentTab = CurrentTab.Wind;
            RefreshInstruments();
        }

        public ICommand PercussionButton { get; private set; }
        private void HandleOnPercussionButton()
        {
            currentTab = CurrentTab.Percussion;
            RefreshInstruments();
        }

        public ICommand InstrumentButtonClick { get; set; }
        private void HandleOnInstrumentButtonClick(Guid instrumentId)
        {
            Task.Run(async () => await model.SellInstrument(instrumentId));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

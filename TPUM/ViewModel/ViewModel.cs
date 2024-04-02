using GalaSoft.MvvmLight.Command;
using Model;
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
        private enum CurrentTab
        {
            String = 0,
            Wind = 1,
            Percussion = 2,
            All = 3
        }

        private CurrentTab currentTab = CurrentTab.All;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<InstrumentModel> instruments;
        private readonly Model.Model model;

        public ViewModel()
        {
            this.model = new Model.Model();

            model.Store.PriceChanged += HandlePriceInflationChanged;
            model.Store.OnItemsUpdated += () => RefreshInstruments();
            model.Store.TransactionFinish += (success) => RefreshInstruments();
            model.ConnectionService.OnConnectionStateChanged += OnConnectionStateChanged;
            model.ConnectionService.OnError += OnConnectionStateChanged;

            OnConnectionStateChanged();

            this.instruments = new ObservableCollection<InstrumentModel>();

            InstrumentButtonClick = new RelayCommand<Guid>(id => InstrumentButtonClickHandler(id));
            AllButton = new RelayCommand(AllButtonClickHandler);
            StringButton = new RelayCommand(StringButtonClickHandler);
            WindButton = new RelayCommand(WindButtonClickHandler);
            PercussionButton = new RelayCommand(PercussionButtonClickHandler);
        }

        public ICommand AllButton { get; private set; }
        public ICommand StringButton { get; private set; }
        public ICommand WindButton { get; private set; }
        public ICommand PercussionButton { get; private set; }
        public ICommand InstrumentButtonClick { get; set; }
        public ObservableCollection<InstrumentModel> Instruments
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

        private void RefreshInstruments()
        {
            instruments.Clear();

            if (currentTab == CurrentTab.All)
            {
                List<InstrumentModel> list = model.Store.GetInstruments();
                list.ForEach(x => instruments.Add(x));
            }
            else
            {
                List<InstrumentModel> list = model.Store.GetInstrumentsByCategory((InstrumentTypeModel)currentTab);
                list.ForEach(x =>  instruments.Add(x));
            }
        }

        private void OnConnectionStateChanged()
        {
            bool actualState = model.ConnectionService.IsConnected();

            if (!actualState)
            {
                model.ConnectionService.Connect(new Uri(@"ws://localhost:8080"));
            }
            else
            {
                model.Store.RequestServerForUpdate();
            }
        }

        private void HandlePriceInflationChanged(object sender, ChangePriceEventArgs args)
        {
            RefreshInstruments();
        }

        private void AllButtonClickHandler()
        {
            currentTab = CurrentTab.All;
            RefreshInstruments();
        }

        private void StringButtonClickHandler()
        {
            currentTab = CurrentTab.String;
            RefreshInstruments();
        }

        private void WindButtonClickHandler()
        {
            currentTab = CurrentTab.Wind;
            RefreshInstruments();
        }

        private void PercussionButtonClickHandler()
        {
            currentTab = CurrentTab.Percussion;
            RefreshInstruments();
        }

        private void InstrumentButtonClickHandler(Guid id)
        {
            model.SellInstrument(id);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnQuantityChanged(object sender, Tpum.Presentation.Model.ChangeProductQuantityEventArgs e)
        {
            ObservableCollection<InstrumentModel> newInstruments = Instruments;
            InstrumentModel instrument = newInstruments.FirstOrDefault(x => x.Id == e.Id);
            int instrumentIndex = newInstruments.IndexOf(instrument);
            newInstruments[instrumentIndex].Quantity = e.Quantity;
            Instruments = new ObservableCollection<InstrumentModel>(newInstruments);
        }
    }
}

using GalaSoft.MvvmLight.Command;
using Logic;
using Model.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TP.GraphicalData.ViewModel.MVVMLight;
using Tpum.Presentation.Model;
using Tpum.Presentation.Model.Interfaces;

namespace Tpum.Presentation.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel() : this(IModel.CreateApi())
        {
        }
        public ViewModel(IModel model)
        {
            _model = model;
            _instruments = new ObservableCollection<InstrumentP>();
            MainViewVisibility = _model.MainViewVisibility;
            InstrumentButtonClick = new RelayCommand<Guid>((id) => InstrumentButtonClickHandler(id));
            
            var fruits = _model.StorePresentation.GetInstruments();
            foreach (var instrument in _instruments)
            {
                Instruments.Add(instrument);
            }
        }

        public ICommand PianoButton
        {
            get; private set;
        }
        public ICommand GrandPianoButton
        {
            get; private set;
        }
        public ICommand GuitarrButton
        {
            get; private set;
        }
        public ICommand ClarinetButton
        {
            get; private set;
        }
        public ICommand BasketButton
        {
            get; private set;
        }
        public ICommand InstrumentButtonClick
        {
            get; set;
        }

        public string MainViewVisibility
        {
            get { return _mainViewVisibility; }
            set
            {
                if (value.Equals(_mainViewVisibility))
                    return;
                _mainViewVisibility = value;
                RaisePropertyChanged("MainViewVisibility");
            }
        }
        public ObservableCollection<InstrumentP> Instruments
        {
            get
            {
                return _instruments;
            }
            set
            {
                if (value.Equals(_instruments))
                    return;
                _instruments = value;
                RaisePropertyChanged("Instruments");
            }
        }

        private ObservableCollection<InstrumentP> _instruments;
        private string _mainViewVisibility;
        private Timer _timer;
        private IModel _model;
        private IBasketP _basketP;
        private decimal _basketSum;

        private void InstrumentButtonClickHandler(Guid id)
        {
            foreach (InstrumentP instrument in _model.StorePresentation.GetInstruments())
            {
                if (instrument.Id.Equals(id))
                {
                    _basketP.AddProduct(instrument);
                    _basketSum = _basketP.SumProducts();
                }
            }
        }
    }
}

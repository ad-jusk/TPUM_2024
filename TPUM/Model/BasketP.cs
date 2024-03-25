using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic.Interfaces;
using Tpum.Presentation.Model.Interfaces;

namespace Tpum.Presentation.Model
{
    public class BasketP : IBasketP
    {
        public ObservableCollection<InstrumentP> Instruments { get; set; }
        public BasketP(ObservableCollection<InstrumentP> instruments, IStore store)
        {
            Instruments = instruments;
            _store = store;
        }

        public void AddProduct(InstrumentP instrument)
        {
            Instruments.Add(instrument);
        }
        public decimal SumProducts()
        {
            return Instruments.Sum(instrument => instrument.Price);
        }

        private IStore _store { get; set; }

    }
}

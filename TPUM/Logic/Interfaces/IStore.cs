using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic.Interfaces
{
    public interface IStore
    {
        public bool Sell(List<Instrument> instruments);
        public List<InstrumentDTO> GetAvailableInstruments();
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductAgeChange;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic.Interfaces;
using Tpum.Logic;

namespace Tpum.Presentation.Model
{
    public class StoreP
    {

        public event EventHandler<Tpum.Presentation.Model.ChangeProductAgeEventArgs> ProductAgeChange;
        public StoreP(IStore store)
        {
            Store = store;
            Store.ProductAgeChange += OnAgeChanged;
        }
        public List<InstrumentP> GetInstruments()
        {
            List<InstrumentP> instruments = new List<InstrumentP>();
            foreach (InstrumentDTO insturment in Store.GetAvailableInstruments())
            {
                instruments.Add(new InstrumentP(insturment.Id, insturment.Name, insturment.Category, insturment.Price, insturment.Age));
            }
            return instruments;
        }

        private IStore Store { get; }

        private void OnAgeChanged(object sender, Tpum.Logic.ChangeProductAgeEventArgs e)
        {
            ProductAgeChange.Invoke(this, new Tpum.Presentation.Model.ChangeProductAgeEventArgs(e.Id, e.Age));
        }
    }
}

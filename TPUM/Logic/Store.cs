using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class Store : IStore
    {
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductAgeChange;
        private IShopRepository _shopRepository;
        
        public Store(IShopRepository shopRepository)
        {
            this._shopRepository = shopRepository;
            shopRepository.ProductPriceChange += OnPriceChanged;
        }
        public List<InstrumentDTO> GetAvailableInstruments()
        {
            List<InstrumentDTO> result = new List<InstrumentDTO>();

            foreach (IInstrument instrument in _shopRepository.ProductStock)
            {
                result.Add(new InstrumentDTO { Id = instrument.Id,  Name = instrument.Name, Category = instrument.Category.ToString(), Price = instrument.Price, Age = instrument.Age });
            }
            return result;
        }

        public bool Sell(List<Instrument> instruments)
        {
            throw new NotImplementedException();
        }

        private void OnPriceChanged(object sender, Tpum.Data.ChangeProductPriceEventArgs e)
        {
            ProductPriceChange?.Invoke(this, new Tpum.Logic.ChangeProductPriceEventArgs(e.Id, e.Price));
        }
        private void OnAgeChanged(object sender, Tpum.Data.ChangeProductAgeEventArgs e)
        {
            ProductAgeChange?.Invoke(this, new Tpum.Logic.ChangeProductAgeEventArgs(e.Id, e.Age));
        }
    }
}

using Tpum.Data.Interfaces;
using Tpum.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository
    {
        public List<IInstrument> ProductStock { get; }
        public void AddInstruments(List<IInstrument> instrumentsToAdd);
        public void RemoveInstruments(List<IInstrument> instrumentsToRemove);
        public void GetProductsWithId(List<Guid> productIds);
        public void ChangeInstruentCategory(Guid instrumentId, InstrumentCategory newCategory); 
        public void ChangeInstrumentPrice(Guid instrumentId, decimal instrumentPrice); 

        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductAgeChange;

    }
}

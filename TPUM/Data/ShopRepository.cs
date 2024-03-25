using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Data.DataModels;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        public ShopRepository() 
        {
            ProductStock = new List<IInstrument>();
            ProductStock.Add(new Instrument("pianino", InstrumentCategory.strunowe, 40000, 3));

        }

        public List<IInstrument> ProductStock { get; }

        public void AddInstruments(List<IInstrument> instrumentsToAdd)
        {
            throw new NotImplementedException();
        }
        public void RemoveInstruments(List<IInstrument> instrumentsToRemove)
        {
            throw new NotImplementedException();
        }
        public void ChangeInstruentCategory(Guid instrumentId, InstrumentCategory newInstrumentCategory)
        {
            throw new NotImplementedException();
        }

        public void ChangeInstrumentPrice(Guid instrumentId, decimal instrumentPrice)
        {
            throw new NotImplementedException();
        }
        public void ChangeInstrumentAge(Guid instrumentId, decimal instrumentAge)
        {
            throw new NotImplementedException();
        }
        public void GetProductsWithId(List<Guid> productIds)
        {
            throw new NotImplementedException();
        }
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductAgeChange;


    }
}

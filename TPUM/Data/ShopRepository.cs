using System.Collections.ObjectModel;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Data.DataModels;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        private readonly List<IInstrument> productStock;
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;

        public ShopRepository() 
        {
            productStock = [
                new Instrument("pianino", InstrumentCategory.strunowe, 4000, 2014, 10),
                new Instrument("pianino elektryczne", InstrumentCategory.strunowe, 2000, 2020, 20),
                new Instrument("trabka", InstrumentCategory.dęte, 1000, 2018, 5),
            ];
        }

        public void AddInstruments(List<IInstrument> instrumentsToAdd)
        {
            productStock.AddRange(instrumentsToAdd);
        }

        public void AddInstrument(IInstrument instrument)
        {
            productStock.Add(instrument);
        }

        public void ChangeInstrumentPrice(Guid instrumentId, decimal instrumentPrice)
        {
            IInstrument? instrument = productStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null)
            {
                instrument.Price = instrumentPrice;
            }
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            IInstrument? instrument = productStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Quantity > 0)
            {
                instrument.Quantity -= 1;
            }
        }

        public void RemoveInstrument(IInstrument instrument)
        {
            productStock.Remove(instrument);
        }

        public IList<IInstrument> GetAllInstruments()
        {
            return new ReadOnlyCollection<IInstrument>(productStock);
        }

        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return new ReadOnlyCollection<IInstrument>(productStock.Where(i => i.Category == category).ToList());
        }

        public IInstrument? GetInstrumentById(Guid productId)
        {
            return productStock.Find(i => i.Id.Equals(productId));
        }
    }
}

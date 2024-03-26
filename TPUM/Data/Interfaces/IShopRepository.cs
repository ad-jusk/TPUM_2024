using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository
    {
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductAgeChange;

        public void AddInstruments(List<IInstrument> instrumentsToAdd);
        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(IInstrument instrument);
        public IList<IInstrument> GetAllInstruments();
        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category);
        public IInstrument? GetInstrumentById(Guid productId);
        public void ChangeInstrumentPrice(Guid instrumentId, decimal instrumentPrice); 
        public void DecrementInstrumentQuantity(Guid instrumentId); 
    }
}

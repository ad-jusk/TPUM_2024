using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceInflationEventArgs> PriceInflationChange;

        public void AddInstruments(List<IInstrument> instrumentsToAdd);
        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(IInstrument instrument);
        public IList<IInstrument> GetAllInstruments();
        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category);
        public IInstrument? GetInstrumentById(Guid productId);
        public void ChangeConsumerFunds(Guid instrumentId, decimal instrumentPrice); 
        public void DecrementInstrumentQuantity(Guid instrumentId); 
    }
}

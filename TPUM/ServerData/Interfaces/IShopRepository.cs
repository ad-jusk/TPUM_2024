using Tpum.ServerData.Enums;

namespace Tpum.ServerData.Interfaces
{
    public interface IShopRepository
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;

        public void AddInstruments(List<IInstrument> instrumentsToAdd);
        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(IInstrument instrument);
        public IList<IInstrument> GetAllInstruments();
        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category);
        public IInstrument? GetInstrumentById(Guid productId);
        public decimal GetConsumerFunds();
        public void ChangeConsumerFunds(Guid instrumentId); 
        public void DecrementInstrumentQuantity(Guid instrumentId); 
    }
}

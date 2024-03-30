using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository : IObservable<IInstrument>
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
        public Task Connect(Uri uri);
        public Task SendMessage(string message);

    }
}

using Data.WebSocket;
using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository : IObservable<IInstrument>
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        public event EventHandler<IInstrument> TransactionSucceeded;

        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(IInstrument instrument);
        public IList<IInstrument> GetAllInstruments();
        public IList<IInstrument> GetInstrumentsByCategory(string category);
        public IInstrument? GetInstrumentById(Guid productId);
        public decimal GetConsumerFunds();
        public void ChangeConsumerFunds(Guid instrumentId); 
        public void DecrementInstrumentQuantity(Guid instrumentId);
        public IConnectionService GetConnectionService();
        public Task Connect(Uri uri);
        public Task SendMessageAsync(string message);
        Task TryBuy(IInstrument instrument);

    }
}

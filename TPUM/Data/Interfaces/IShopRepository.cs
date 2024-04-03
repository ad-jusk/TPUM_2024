using Tpum.Data.Interfaces;
using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository : IObservable<IInstrument>, IObservable<decimal>
    {
/*        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;*/
        public event EventHandler<IInstrument> TransactionSucceeded;

        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(IInstrument instrument);
        public IList<IInstrument> GetAllInstruments();
        public IList<IInstrument> GetInstrumentsByCategory(string category);
        public IInstrument? GetInstrumentById(Guid productId);
        public decimal GetConsumerFunds();
        public void DecrementInstrumentQuantity(Guid instrumentId);
        public Task Connect(Uri uri);
        public Task SendMessageAsync(string message);
        Task TryBuyingInstrument(IInstrument instrument);

    }
}

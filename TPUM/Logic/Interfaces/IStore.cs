using Data.WebSocket;
using Tpum.Data.Enums;

namespace Tpum.Logic.Interfaces
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(string category);

        public InstrumentDTO GetInstrumentById(Guid id);

        public void DecrementInstrumentQuantity(Guid instrumentId);
        public decimal GetConsumerFunds();

        public Task SendMessageAsync(string message);
        public Task SellInstrument(InstrumentDTO instrumentDTO);

        public event EventHandler<decimal> ConsumerFundsChangeCS;
        public event EventHandler<InstrumentDTO> InstrumentChange;
        public event EventHandler<InstrumentDTO> TransactionSucceeded;

    }
}

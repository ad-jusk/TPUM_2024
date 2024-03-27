using Tpum.Data.Enums;

namespace Tpum.Logic.Interfaces
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category);

        public InstrumentDTO GetInstrumentById(Guid id);

        public void DecrementInstrumentQuantity(Guid instrumentId);

        public decimal GetConsumerFunds();

        public void ChangeConsumerFunds(Guid instrumentId);

        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
    }
}

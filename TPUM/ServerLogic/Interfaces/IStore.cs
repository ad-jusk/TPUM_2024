using Tpum.ServerData.Enums;

namespace Tpum.ServerLogic.Interfaces
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category);

        public bool SellInstrument(InstrumentDTO instrument);
        public InstrumentDTO GetInstrumentById(Guid id);

        public void DecrementInstrumentQuantity(Guid instrumentId);

        public decimal GetConsumerFunds();

        public void ChangeConsumerFunds(Guid instrumentId);

        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
    }
}

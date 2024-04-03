using Tpum.ServerData.Enums;

namespace Tpum.ServerLogic.Interfaces
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(string category);

        public InstrumentDTO GetInstrumentById(Guid id);

        public void DecrementInstrumentQuantity(Guid instrumentId);

        public void ChangeConsumerFunds(Guid instrumentId);
        public decimal GetConsumerFunds();
        public bool SellInstrument(InstrumentDTO instrument);

        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
    }
}

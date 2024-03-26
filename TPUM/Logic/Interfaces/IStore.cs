using Tpum.Data.Enums;

namespace Tpum.Logic.Interfaces
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category);

        public void DecrementInstrumentQuantity(Guid instrumentId);

        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
    }
}

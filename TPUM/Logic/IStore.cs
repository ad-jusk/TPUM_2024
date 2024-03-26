using Tpum.Data.Enums;
using Tpum.Logic;

namespace Logic
{
    public interface IStore
    {
        public List<InstrumentDTO> GetAvailableInstruments();

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category);

        public void DecrementInstrumentQuantity(Guid instrumentId);

        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
    }
}

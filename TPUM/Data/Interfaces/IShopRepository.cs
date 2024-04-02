using Tpum.Data.Enums;

namespace Tpum.Data.Interfaces
{
    public interface IShopRepository
    {
        public event EventHandler<ChangePriceEventArgs> PriceChanged;

        public event Action? ItemsUpdated;
        public event Action<bool>? TransactionFinish;

        public void RequestServerForUpdate();
        public List<IInstrument> GetInstruments();
        public IInstrument GetInstrumentById(Guid instrumentId);
        public List<IInstrument> GetInstrumentsByType(InstrumentType type);
        public Task SellInstrument(Guid instrumentId);
    }
}

using ClientLogic.Enums;
using ClientLogic.Interfaces;

namespace Tpum.Logic.Interfaces
{
    public interface IShopLogic
    {
        public event EventHandler<ChangePriceEventArgsLogic> PriceChanged;
        public event Action? ItemsUpdated;
        public event Action<bool>? TransactionFinish;

        public void RequestServerForUpdate();

        public Task SellInstrument(Guid itemId);

        public List<IInstrumentLogic> GetInstruments();
        public List<IInstrumentLogic> GetInstrumentsByType(InstrumentTypeLogic instrumentType);
    }
}

using ClientLogic.Enums;
using Model;
using Tpum.Data.Enums;
using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    public class StoreModel
    {
        public event EventHandler<ChangePriceEventArgs>? PriceChanged;
        public Action? OnItemsUpdated;
        public event Action<bool>? TransactionFinish;

        private readonly IShopLogic store;

        public StoreModel(IShopLogic store)
        {
            this.store = store;
            this.store.PriceChanged += (obj, args) => PriceChanged?.Invoke(this, new ChangePriceEventArgs(args));
            this.store.ItemsUpdated += () => OnItemsUpdated?.Invoke();
            this.store.TransactionFinish += succeeded => TransactionFinish?.Invoke(succeeded);
        }

        public void RequestServerForUpdate()
        {
            store.RequestServerForUpdate();
        }

        public List<InstrumentModel> GetInstruments()
        {
            return store.GetInstruments()
                .Select(i => new InstrumentModel(i))
                .ToList();
        }

        public List<InstrumentModel> GetInstrumentsByCategory(InstrumentTypeModel type)
        {
            return store.GetInstrumentsByType((InstrumentTypeLogic)type)
                .Select(i => new InstrumentModel(i))
                .ToList();
        }
    }
}

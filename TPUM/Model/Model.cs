using Logic;
using Tpum.Data.Enums;

namespace Tpum.Presentation.Model
{
    public class Model
    {
        private readonly LogicAbstractApi logicApi;
        private readonly StorePresentation store;

        public Model(LogicAbstractApi logicApi = null)
        {
            this.logicApi = logicApi ?? LogicAbstractApi.Create();
            this.store = new StorePresentation(this.logicApi.GetStore());
        }

        public List<InstrumentPresentation> GetInstruments()
        {
            return store.GetInstruments();
        }

        public List<InstrumentPresentation> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return store.GetInstrumentsByCategory(category);
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            store.DecrementInstrumentQuantity(instrumentId);
        }

        public string MainViewVisibility => throw new NotImplementedException();

        public string BasketViewVisibility => throw new NotImplementedException();
    }
}

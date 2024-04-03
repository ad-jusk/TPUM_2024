using Logic;
using Tpum.Data.Enums;
using Tpum.Logic;

namespace Tpum.Presentation.Model
{
    public class Model
    {
        private readonly LogicAbstractApi logicApi;
        public StorePresentation Store { get; private set; } // Expose StorePresentation as a public property

        public Model(LogicAbstractApi logicApi = null)
        {
            this.logicApi = logicApi ?? LogicAbstractApi.Create();
            this.Store = new StorePresentation(this.logicApi.GetStore());
        }

        public List<InstrumentPresentation> GetInstruments()
        {
            return Store.GetInstruments();
        }

        public List<InstrumentPresentation> GetInstrumentsByCategory(string category)
        {
            return Store.GetInstrumentsByCategory(category);
        }

        public InstrumentPresentation GetInstrumentsById(Guid Id)
        {
            return Store.GetInstrumentById(Id);
        }
        public async Task SellInstrument(InstrumentPresentation instrument)
        {
            await Store.SellInstrument(instrument);
        }
        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            Store.DecrementInstrumentQuantity(instrumentId);
        }
    }
}

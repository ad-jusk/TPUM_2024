using Logic;
using Model;
using Tpum.Data.Enums;

namespace Tpum.Presentation.Model
{
    public class Model
    {
        private readonly LogicAbstractApi logicApi;
        public StoreModel Store { get; private set; }
        public ConnectionServiceModel ConnectionService { get; private set; }

        public event Action? InstrumentsUpdated;

        public Model(LogicAbstractApi logicApi = null)
        {
            this.logicApi = logicApi ?? LogicAbstractApi.Create();
            this.Store = new StoreModel(this.logicApi.GetStore());
            this.ConnectionService = new ConnectionServiceModel(this.logicApi.GetConnectionService());
        }

        public async Task SellInstrument(Guid instrumentId)
        {
            await logicApi.GetStore().SellInstrument(instrumentId);
        }
    }
}

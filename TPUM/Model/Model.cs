using Logic;
using Tpum.Data.Enums;
using Tpum.Logic;

namespace Tpum.Presentation.Model
{
    public class Model
    {
        private readonly LogicAbstractApi logicApi;
        public StorePresentation Store { get; private set; }

        public Model(LogicAbstractApi logicApi = null)
        {
            this.logicApi = logicApi ?? LogicAbstractApi.Create();
            this.Store = new StorePresentation(this.logicApi.GetStore());
        }
    }
}

using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    public abstract class ModelAbstractApi
    {
        public static ModelAbstractApi CreateApi(ILogic logic = default)
        {
            return new ModelApi(logic ?? ILogic.Create());
        }
        public abstract string MainViewVisibility { get; }
        public abstract string BasketViewVisibility { get; }

    }
}

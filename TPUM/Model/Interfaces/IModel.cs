using Tpum.Logic;
using Tpum.Logic.Interfaces;
using Tpum.Presentation.Model;

namespace Model.Interfaces
{
    public interface IModel
    {
        ILogic Logic { get; set; }
        public static IModel CreateApi(ILogic logic = default)
        {
            return new Tpum.Presentation.Model.Model(logic ?? ILogic.Create());
        }
        public abstract StoreP StorePresentation { get; }
        public abstract string MainViewVisibility { get; }
        public abstract string BasketViewVisibility { get; }

    }

}

using TP.GraphicalData.ViewModel.MVVMLight;
using Tpum.Presentation.Model;

namespace Tpum.Presentation.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel() : this(ModelAbstractApi.CreateApi())
        {
        }
        public ViewModel(ModelAbstractApi modelAbstractApi)
        {
        }
    }
}

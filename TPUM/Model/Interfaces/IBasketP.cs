using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Presentation.Model.Interfaces
{
    public interface IBasketP
    {
        ObservableCollection<InstrumentP> Instruments { get; }
        decimal SumProducts();
        void AddProduct(InstrumentP instrument);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    internal class ModelApi : ModelAbstractApi
    {
        public ModelApi(ILogic logic)
        {
            _logic = logic;
        }
        public override string MainViewVisibility => throw new NotImplementedException();
        public override string BasketViewVisibility => throw new NotImplementedException();
        private ILogic _logic;
    }
}

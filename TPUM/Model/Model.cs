using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Interfaces;
using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    internal class Model : IModel
    {
        public Model(ILogic logic)
        {
            Logic = logic;
        }


        public ILogic Logic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StoreP StorePresentation => throw new NotImplementedException();
        public string MainViewVisibility => throw new NotImplementedException();

        public string BasketViewVisibility => throw new NotImplementedException();

    }
}

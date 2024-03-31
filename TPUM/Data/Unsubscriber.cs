using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<IInstrument>> _observers;
        private IObserver<IInstrument> _observer;

        public Unsubscriber(List<IObserver<IInstrument>> observers, IObserver<IInstrument> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}

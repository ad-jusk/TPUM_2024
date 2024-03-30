using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    private class Unsubscriber : IDisposable
    {
        private List<IObserver<IFruit>> _observers;
        private IObserver<IFruit> _observer;

        public Unsubscriber(List<IObserver<IFruit>> observers, IObserver<IFruit> observer)
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

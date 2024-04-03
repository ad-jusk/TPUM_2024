using Tpum.Data.Interfaces;

namespace Tpum.Data
{
    internal class Unsubscriber : IDisposable
    {
        private List<IObserver<IInstrument>> _observers;
        private List<IObserver<decimal>> _fundsObservers;
        private IObserver<IInstrument> _observer;
        private IObserver<decimal> _fundsObserver;

        public Unsubscriber(List<IObserver<IInstrument>> observers, IObserver<IInstrument> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }
        public Unsubscriber(List<IObserver<decimal>> fundsObservers, IObserver<decimal> fundsObserver)
        {
            this._fundsObservers = fundsObservers;
            this._fundsObserver = fundsObserver;
        }
        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
            if (_fundsObservers != null && _fundsObservers.Contains(_fundsObserver))
                _fundsObservers.Remove(_fundsObserver);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Commons;
using Data;

namespace Data
{
    internal class ShopData : IShopData
    {
        private readonly Dictionary<Guid, IInstrument> instruments = new Dictionary<Guid, IInstrument>();
        private readonly object instrumentLock = new object();

        private HashSet<IObserver<InflationChangedEventArgs>> observers;

        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        private readonly IConnectionService connectionService;

        private readonly JsonSerializer serializer;

        public ShopData(IConnectionService connectionService)
        {
            this.observers = new HashSet<IObserver<InflationChangedEventArgs>>();

            this.connectionService = connectionService;
            this.connectionService.OnMessage += OnMessage;
            this.connectionService.OnConnectionStateChanged += OnConnect;

            this.serializer = new JsonSerializer();

            this.connectionService.Connect(new Uri(@"ws://localhost:8080"));
        }

        ~ShopData()
        {
            List<IObserver<InflationChangedEventArgs>> cachedObservers = observers.ToList();
            foreach (IObserver<InflationChangedEventArgs>? observer in cachedObservers)
            {
                observer?.OnCompleted();
            }
        }

        private void OnConnect()
        {
            if (connectionService.IsConnected())
            {
                Task.Run(async () => await RequestInstruments());
            }
        }

        private void OnMessage(string message)
        {
            if (serializer.GetHeader(message) == UpdateAllResponse.StaticHeader)
            {
                UpdateAllResponse response = serializer.Deserialize<UpdateAllResponse>(message);
                UpdateAllProducts(response);
            }
            else if (serializer.GetHeader(message) == InflationChangedResponse.StaticHeader)
            {
                InflationChangedResponse response = serializer.Deserialize<InflationChangedResponse>(message);
                UpdateAllPrices(response);
            }
            else if (serializer.GetHeader(message) == TransactionResponse.StaticHeader)
            {
                TransactionResponse response = serializer.Deserialize<TransactionResponse>(message);
                if (response.Succeeded)
                {
                    Task.Run(() => RequestInstruments());
                    TransactionFinish?.Invoke(true);
                }
                else
                {
                    TransactionFinish?.Invoke(false);
                }
            }
        }

        private void UpdateAllProducts(UpdateAllResponse response)
        {
            if (response.Instruments == null)
                return;

            lock (instrumentLock)
            {
                instruments.Clear();
                foreach (InstrumentDTO item in response.Instruments)
                {
                    instruments.Add(item.Id, item.ToInstrument());
                }
            }

            InstrumentsUpdated?.Invoke();
        }

        private void UpdateAllPrices(InflationChangedResponse response)
        {
            if (response.NewPrices == null)
                return;

            lock (instrumentLock)
            {
                foreach (var newPrice in response.NewPrices)
                {
                    if (instruments.ContainsKey(newPrice.InstrumentID))
                    {
                        instruments[newPrice.InstrumentID].Price = newPrice.NewPrice;
                    }
                }
            }

            foreach (IObserver<InflationChangedEventArgs>? observer in observers)
            {
                observer.OnNext(new InflationChangedEventArgs(response.NewInflation));
            }
        }

        private async Task RequestInstruments()
        {
            await connectionService.SendAsync(serializer.Serialize(new GetInstrumentsCommand()));
        }

        public void RequestUpdate()
        {
            if (connectionService.IsConnected())
            {
                Task.Run(async () => await RequestInstruments());
            }
        }

        public async Task SellInstrument(Guid instrumentId)
        {
            if (connectionService.IsConnected())
            {
                SellInstrumentCommand command = new SellInstrumentCommand(instrumentId);
                await connectionService.SendAsync(serializer.Serialize(command));
            }
        }

        public List<IInstrument> GetInstruments()
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (instrumentLock)
            {
                result.AddRange(instruments.Values.Select(item => (IInstrument)item.Clone()));
            }
            return result;
        }

        public IInstrument GetInstrumentByID(Guid instrumentId)
        {
            IInstrument result;
            lock (instrumentLock)
            {
                if (instruments.ContainsKey(instrumentId))
                {
                    result = instruments[instrumentId];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return result;
        }

        public List<IInstrument> GetInstrumentsByType(InstrumentType type)
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (instrumentLock)
            {
                result.AddRange(instruments.Values
                .Where(item => item.Type == type)
                    .Select(item => (IInstrument)item.Clone()));
            }

            return result;
        }

        public IDisposable Subscribe(IObserver<InflationChangedEventArgs> observer)
        {
            observers.Add(observer);
            return new ShopDataDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<InflationChangedEventArgs> observer)
        {
            observers.Remove(observer);
        }

        private class ShopDataDisposable : IDisposable
        {
            private readonly ShopData warehouse;
            private readonly IObserver<InflationChangedEventArgs> observer;

            public ShopDataDisposable(ShopData warehouse, IObserver<InflationChangedEventArgs> observer)
            {
                this.warehouse = warehouse;
                this.observer = observer;
            }

            public void Dispose()
            {
                warehouse.UnSubscribe(observer);
            }
        }
    }

}
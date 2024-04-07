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
        private readonly object customerFundsLock = new object();

        private float customerFunds = 0f;

        private HashSet<IObserver<InflationChangedEventArgs>> inflationObservers;
        private HashSet<IObserver<float>> customerFundsObservers;

        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        private readonly IConnectionService connectionService;

        private readonly JsonSerializer serializer;

        public ShopData(IConnectionService connectionService)
        {
            this.inflationObservers = new HashSet<IObserver<InflationChangedEventArgs>>();
            this.customerFundsObservers = new HashSet<IObserver<float>>();

            this.connectionService = connectionService;
            this.connectionService.OnMessage += OnMessage;
            this.connectionService.OnConnectionStateChanged += OnConnect;

            this.serializer = new JsonSerializer();

            this.connectionService.Connect(new Uri(@"ws://localhost:8080"));
        }

        ~ShopData()
        {
            List<IObserver<InflationChangedEventArgs>> cachedObservers = inflationObservers.ToList();
            foreach (IObserver<InflationChangedEventArgs>? observer in cachedObservers)
            {
                observer?.OnCompleted();
            }
        }

        private void OnConnect()
        {
            Task.Run(async () => await RequestInstrumentsAndFunds());
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
                    UpdateCustomerFunds(response.CustomerFunds);
                    Task.Run(() => RequestInstruments());
                    TransactionFinish?.Invoke(true);
                }
                else
                {
                    TransactionFinish?.Invoke(false);
                }
            }
            else if (serializer.GetHeader(message) == GetInstrumentsAndFundsResponse.StaticHeader)
            {
                GetInstrumentsAndFundsResponse response = serializer.Deserialize<GetInstrumentsAndFundsResponse>(message);
                
                UpdateAllProducts(new UpdateAllResponse { Instruments = response.Instruments });
                UpdateCustomerFunds(response.Funds);
            }
        }

        private void UpdateCustomerFunds(float newFunds)
        {
            lock(customerFundsLock)
            {
                customerFunds = newFunds;
                foreach (IObserver<float>? observer in customerFundsObservers)
                {
                    observer.OnNext(customerFunds);
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

            foreach (IObserver<InflationChangedEventArgs>? observer in inflationObservers)
            {
                observer.OnNext(new InflationChangedEventArgs(response.NewInflation));
            }
        }

        private async Task RequestInstrumentsAndFunds()
        {
            await connectionService.SendAsync(serializer.Serialize(new GetInstrumentsAndFundsCommand()));
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
            inflationObservers.Add(observer);
            return new ShopDataDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<InflationChangedEventArgs> observer)
        {
            inflationObservers.Remove(observer);
        }

        public IDisposable Subscribe(IObserver<float> observer)
        {
            customerFundsObservers.Add(observer);
            return new ShopDataDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<float> observer)
        {
            customerFundsObservers.Remove(observer);
        }

        private class ShopDataDisposable : IDisposable
        {
            private readonly ShopData shop;
            private readonly IObserver<InflationChangedEventArgs> observer;
            private readonly IObserver<float> observerFunds;

            public ShopDataDisposable(ShopData shop, IObserver<InflationChangedEventArgs> observer)
            {
                this.shop = shop;
                this.observer = observer;
            }

            public ShopDataDisposable(ShopData shop, IObserver<float> observer)
            {
                this.shop = shop;
                this.observerFunds = observer;
            }

            public void Dispose()
            {
                shop.UnSubscribe(observer);
                shop.UnSubscribe(observerFunds);
            }
        }
    }

}
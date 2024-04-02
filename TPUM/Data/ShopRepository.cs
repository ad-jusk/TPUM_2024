using System.Collections.ObjectModel;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Data.DataModels;
using Tpum.Data.WebSocket;
using System;
using Data.WebSocket;
using Data;
using System.Text.Json;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        public event EventHandler<IInstrument> TransactionSucceeded;
        private readonly ConnectionService connectionService;
        private readonly List<IInstrument> productStock;
        private readonly List<IObserver<IInstrument>> observers;
        private decimal consumerFunds;
        private bool transactionSuccess;

        public ShopRepository() 
        {
            productStock = new List<IInstrument>();
            observers = new List<IObserver<IInstrument>>();
            consumerFunds = 120000M;
            connectionService = new ConnectionService();
            Connect(new Uri("ws://localhost:8080"));
        }

        public IDisposable Subscribe(IObserver<IInstrument> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }
        public IConnectionService GetConnectionService() { return connectionService; }
        public async Task Connect(Uri uri)
        {
            try
            {
                bool connected = await connectionService.Connect(uri);
                if (connected)
                {
                    connectionService.Connection.onMessage = ParseMessage;
                    await SendMessageAsync("RequestInstruments");
                }
                else
                {
                    Console.WriteLine($"Failed to connect with {uri}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (connectionService.Connected)
            {
                Console.WriteLine($"Server: Sending message {message}");
                await connectionService.Connection.SendAsync(message);
            }
        }
        public async Task TryBuy(IInstrument instrument)
        {
            string categoryString = Enum.GetName(typeof(InstrumentCategory), instrument.Category);

            // Create a new anonymous object with the enum value converted to string
            var instrumentData = new
            {
                instrument.Id,
                instrument.Name,
                Category = categoryString, // Use the string representation here
                instrument.Price,
                instrument.Year,
                instrument.Quantity
            };


            //string json = Serializer.InstrumentToJSON(instrument);
            string json = JsonSerializer.Serialize(instrumentData);

            await connectionService.Connection.SendAsync("RequestTransaction" + json);
        }
        public void AddInstrument(IInstrument instrument)
        {
            productStock.Add(instrument);
            
            // Notify observers about changes
            foreach (var observer in observers)
            {
                observer.OnNext(instrument);
            }
        }

        public void RemoveInstrument(IInstrument instrument)
        {
            IInstrument? instrumentToRemove = productStock.Find(x => x.Id == instrument.Id);
            if (instrument == null)
                return; 
            productStock.Remove(instrument);
            
            foreach (var observer in observers)
            {
                observer.OnNext(instrument);
            }
        }

        public decimal GetConsumerFunds()
        {
            return consumerFunds;
        }

        public void ChangeConsumerFunds(Guid instrumentId)
        {
            IInstrument? instrument = productStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Price > 0 && instrument.Quantity > 0)
            {
                consumerFunds -= instrument.Price;
                OnConsumerFundsChanged(consumerFunds);
            }
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            IInstrument? instrument = productStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Quantity > 0)
            {
                instrument.Quantity -= 1;
                OnQuantityChanged(instrument.Id, instrument.Quantity);
            }
        }

        public IList<IInstrument> GetAllInstruments()
        {
            return new ReadOnlyCollection<IInstrument>(productStock);
        }

        public IList<IInstrument> GetInstrumentsByCategory(string category)
        {
            if (!Enum.TryParse(category, true, out InstrumentCategory instrumentCategory))
            {
                throw new ArgumentException("Invalid instrument category.", nameof(category));
            }
            return new ReadOnlyCollection<IInstrument>(productStock.Where(i => i.Category == instrumentCategory).ToList());
        }

        public IInstrument? GetInstrumentById(Guid productId)
        {
            return productStock.Find(i => i.Id.Equals(productId));
        }

        private void ParseMessage(string message)
        {
            if (message.StartsWith("UpdateAll"))
            {
                string json = message.Substring("UpdateAll".Length);
                List<IInstrument> instruments = Serializer.JSONToInstruments(json);
                foreach (var instrument in instruments)
                {
                    AddInstrument(instrument);
                }
            }
            else if (message.Contains("PriceChanged"))
            {
                string priceChangedStr = message.Substring("PriceChanged".Length);
                SendMessageAsync("RequestInstruments");

                /*                productStock.Clear();
                                string json = message.Substring("UpdateAll".Length);
                                List<IInstrument> instruments = Serializer.JSONToInstruments(json);
                                foreach (var instrument in instruments)
                                {
                                    instrument.Price *= decimal.Parse(priceChangedStr);
                                    AddInstrument(instrument);
                                }
                                PriceChange?.Invoke(this, new ChangePriceEventArgs(decimal.Parse(priceChangedStr)));*/
                //ChangePrice(decimal.Parse(priceChangedStr));
            }
            else if (message.Contains("TransactionResult"))
            {
                string resString = message.Substring("TransactionResult".Length);
                transactionSuccess = resString[0] == '1';

                if (!transactionSuccess)
                {
                    //EventHandler handler = TransactionFailed;
                    //handler?.Invoke(this, EventArgs.Empty);
                    SendMessageAsync("RequestInstruments");
                }
                else
                {
                    EventHandler<IInstrument> handler = TransactionSucceeded;
                    handler?.Invoke(this, Serializer.JSONToInstrument(resString.Substring(1)));
                    SendMessageAsync("RequestInstruments");
                }
            }
        }
        public void ChangePrice(decimal inflationRate)
        {
/*            productStock.Clear();
            string json = message.Substring("UpdateAll".Length);
            List<IInstrument> instruments = Serializer.JSONToInstruments(json);
            foreach (var instrument in productStock)
            {
                instrument.Price *= inflationRate;
            }
            PriceChange?.Invoke(this, new ChangePriceEventArgs(inflationRate));*/
        }
        private void OnConsumerFundsChanged(decimal funds)
        {
            EventHandler<ChangeConsumerFundsEventArgs> handler = ConsumerFundsChange;
            handler?.Invoke(this, new ChangeConsumerFundsEventArgs(funds));
        }

        private void OnQuantityChanged(Guid id, int quantity)
        {
            EventHandler<ChangeProductQuantityEventArgs> handler = ProductQuantityChange;
            handler?.Invoke(this, new ChangeProductQuantityEventArgs(id, quantity));
        }

        private async Task SimulatePriceChangeAsync()
        {
            var random = new Random();
            while (true)
            {
                // Wait for a random duration between 7 to 10 seconds
                int waitMilliseconds = random.Next(7000, 10000);
                await Task.Delay(waitMilliseconds);

                // Calculate a random inflation rate between 0.8 and 1.2
                decimal inflationRate = (decimal)random.NextDouble()*(1.2m - 0.8m) + 0.8m;

                // Apply inflation to all items
                foreach (var instrument in productStock)
                {
                    instrument.Price *= inflationRate;
                }

                // Notify listeners of the inflation change
                PriceChange?.Invoke(this, new ChangePriceEventArgs(inflationRate));

            }
        }

    }
}

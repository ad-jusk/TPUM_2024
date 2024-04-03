using System.Collections.ObjectModel;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Data.WebSocket;
using Data;
using System.Text.Json;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        public event EventHandler<IInstrument> TransactionSucceeded;
        private readonly ConnectionService connectionService;
        private readonly List<IInstrument> productStock;
        private readonly List<IObserver<IInstrument>> observers;
        private readonly List<IObserver<decimal>> fundsObservers;
        private decimal consumerFunds;
        private bool transactionSuccess;

        public ShopRepository() 
        {
            productStock = new List<IInstrument>();
            observers = new List<IObserver<IInstrument>>();
            fundsObservers = new List<IObserver<decimal>>();
            connectionService = new ConnectionService();
            Connect(new Uri("ws://localhost:8080"));
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

        public decimal GetConsumerFunds()
        {
            return consumerFunds;
        }

        public void ChangeConsumerFundsCS(decimal consumerFunds)
        {
            foreach (var fundObserver in fundsObservers)
            {
                fundObserver.OnNext(consumerFunds);
            }
        }
        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            IInstrument? instrument = productStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Quantity > 0)
            {
                instrument.Quantity -= 1;
            }
        }

        public async Task Connect(Uri uri)
        {
            try
            {
                bool connected = await connectionService.Connect(uri);
                if (connected)
                {
                    connectionService.Connection.onMessage = ParseMessage;
                    await SendMessageAsync("RequestInstruments");
                    await SendMessageAsync("RequestFunds");
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
        public async Task TryBuyingInstrument(IInstrument instrument)
        {
            string categoryString = Enum.GetName(typeof(InstrumentCategory), instrument.Category);

            var instrumentData = new
            {
                instrument.Id,
                instrument.Name,
                Category = categoryString, 
                instrument.Price,
                instrument.Year,
                instrument.Quantity
            };
            string json = JsonSerializer.Serialize(instrumentData);

            await connectionService.Connection.SendAsync("RequestTransaction" + json);
        }

        private void ParseMessage(string message)
        {
            if (message.StartsWith("UpdateAll"))
            {
                string json = message.Substring("UpdateAll".Length);
                productStock.Clear();
                List<IInstrument> instruments = Serializer.JSONToInstruments(json);
                foreach (var instrument in instruments)
                {
                    AddInstrument(instrument);
                }
            }
            else if (message.StartsWith("UpdateCategory"))
            {
                string json = message.Substring("UpdateCategory".Length);
                productStock.Clear();
                List<IInstrument> instruments = Serializer.JSONToInstruments(json);
                foreach (var instrument in instruments)
                {
                    AddInstrument(instrument);
                }
            }
            else if(message.StartsWith("UpdateSome"))
            {
                string json = message.Substring("UpdateSome".Length);
                productStock.Clear();
                List<IInstrument> instruments = Serializer.JSONToInstruments(json);
                foreach (var instrument in instruments)
                {
                    AddInstrument(instrument);
                }
            }
            else if (message.Contains("PriceChanged"))
            {
                string priceChangedStr = message.Substring("PriceChanged".Length);

                string json = UpdateSomeInstruments();
                SendMessageAsync("RequestInstrumentsById" + json);
            }
            else if (message.Contains("ConsumerFundsChanged"))
            {
                string priceChangedStr = message.Substring("ConsumerFundsChanged".Length);
                ChangeConsumerFundsCS(decimal.Parse(priceChangedStr));
            }
            else if (message.Contains("TransactionResult"))
            {
                string resString = message.Substring("TransactionResult".Length);
                transactionSuccess = resString[0] == '1';

                if (!transactionSuccess)
                {
                    SendMessageAsync("OUT OF STOCK");
                    SendMessageAsync("RequestInstruments");
                }
                else
                {
                    EventHandler<IInstrument> handler = TransactionSucceeded;
                    handler?.Invoke(this, Serializer.JSONToInstrument(resString.Substring(1)));

                    string json = UpdateSomeInstruments();
                    SendMessageAsync("RequestInstrumentsById" +json);
                }
            }
        }

        private string UpdateSomeInstruments()
        {
            List<IInstrument> displayedProducts = new List<IInstrument>(productStock);
            productStock.Clear();
            IList<IInstrument> all = GetAllInstruments();
            List<object> instrumentDataList = new List<object>();

            all
                .Where(i => displayedProducts.Contains(i))
                .ToList()
                .ForEach(i => {
                    AddInstrument(i);
                });
            foreach (var instrument in displayedProducts)
            {
                string categoryString = Enum.GetName(typeof(InstrumentCategory), instrument.Category);

                var instrumentData = new
                {
                    instrument.Id,
                    instrument.Name,
                    Category = categoryString,
                    instrument.Price,
                    instrument.Year,
                    instrument.Quantity
                };
                instrumentDataList.Add(instrumentData);
            }
            string json = JsonSerializer.Serialize(instrumentDataList);
            return json;
        }

        public IDisposable Subscribe(IObserver<IInstrument> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public IDisposable Subscribe(IObserver<decimal> observer)
        {
            if (!fundsObservers.Contains(observer))
                fundsObservers.Add(observer);
            return new Unsubscriber(fundsObservers, observer);
        }
    }
}

using System.Collections.ObjectModel;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Data.DataModels;
using Tpum.Data.WebSocket;
using System;

namespace Tpum.Data
{
    internal class ShopRepository : IShopRepository
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        private readonly List<IInstrument> productStock;
        private List<IObserver<IInstrument>> observers;
        private decimal consumerFunds;
        private WebSocketConnection connection = null;

        public ShopRepository() 
        {
            // THIS IS COMMENTED BECAUSE DATA WILL BE RETRIEVED FROM SERVER
/*            productStock = [
                new Instrument("Pianino", InstrumentCategory.String, 5000, 2014, 10),
                new Instrument("Fortepian", InstrumentCategory.String, 6000, 2014, 10),
                new Instrument("Gitara", InstrumentCategory.String, 2200, 2020, 20),
                new Instrument("Trąbka", InstrumentCategory.Wind, 1500, 2018, 5),
                new Instrument("Flet", InstrumentCategory.Wind, 1100, 2018, 5),
                new Instrument("Harmonijka", InstrumentCategory.Wind, 900, 2018, 5),
                new Instrument("Tamburyn", InstrumentCategory.Percussion, 200, 2018, 5),
                new Instrument("Bęben", InstrumentCategory.Percussion, 800, 2018, 5),
            ];*/
            productStock = new List<IInstrument>();
            observers = new List<IObserver<IInstrument>>();
            consumerFunds = 120000M;
            SimulatePriceChangeAsync();
        }

        public IDisposable Subscribe(IObserver<IInstrument> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public async Task Connect(Uri uri)
        {
            try
            {
                connection = await WebSocketClient.Connect(uri, log => { });
                if (connection != null)
                {
                    connection.onMessage = ParseMessage;
                    await SendMessage("RequestInstruments");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        public async Task SendMessage(string message)
        {
            if (connection != null)
            {
                Console.WriteLine($"Server: Sending message {message}");
                await connection.SendAsync(message);
            }
        }

        public void AddInstruments(List<IInstrument> instrumentsToAdd)
        {
            productStock.AddRange(instrumentsToAdd);
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

        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return new ReadOnlyCollection<IInstrument>(productStock.Where(i => i.Category == category).ToList());
        }

        public IInstrument? GetInstrumentById(Guid productId)
        {
            return productStock.Find(i => i.Id.Equals(productId));
        }

        private void ParseMessage(string message)
        {
            // TODO
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

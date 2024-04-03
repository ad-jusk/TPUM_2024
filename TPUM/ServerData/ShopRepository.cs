using System.Collections.ObjectModel;
using Tpum.ServerData.Enums;
using Tpum.ServerData.Interfaces;
using Tpum.ServerData.DataModels;

namespace Tpum.ServerData
{
    internal class ShopRepository : IShopRepository
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        private readonly List<IInstrument> productStock;
        private decimal consumerFunds;
        private object instrumentsLock = new object();
        private object consumerFundsLock = new object();

        public ShopRepository() 
        {
            productStock = [
                new Instrument("Pianino", InstrumentCategory.String, 5000, 2014, 10),
                new Instrument("Fortepian", InstrumentCategory.String, 6000, 2014, 10),
                new Instrument("Gitara", InstrumentCategory.String, 2200, 2020, 20),
                new Instrument("Trąbka", InstrumentCategory.Wind, 1500, 2018, 5),
                new Instrument("Flet", InstrumentCategory.Wind, 1100, 2018, 5),
                new Instrument("Harmonijka", InstrumentCategory.Wind, 900, 2018, 5),
                new Instrument("Tamburyn", InstrumentCategory.Percussion, 200, 2018, 5),
                new Instrument("Bęben", InstrumentCategory.Percussion, 800, 2018, 5),
            ];
            consumerFunds = 120000M;
            SimulatePriceChangeAsync();
        }

        public void AddInstrument(IInstrument instrument)
        {
            lock(instrumentsLock)
            {
                productStock.Add(instrument);
            }
        }

        public IList<IInstrument> GetAllInstruments()
        {
            List<IInstrument> instruments = new List<IInstrument>();
            lock (instrumentsLock) 
            {
                instruments.AddRange(productStock);
            }
            return new ReadOnlyCollection<IInstrument>(instruments);
        }

        public IList<IInstrument> GetInstrumentsByCategory(string category)
        {
            if (!Enum.TryParse(category, true, out InstrumentCategory instrumentCategory))
            {
                throw new ArgumentException("Invalid instrument category.", nameof(category));
            }
            List<IInstrument> instruments = new List<IInstrument>();
            lock (instrumentsLock)
            {
                instruments.AddRange(productStock.Where(i => i.Category == instrumentCategory).ToList());
            }
            return new ReadOnlyCollection<IInstrument>(instruments);
        }

        public IInstrument? GetInstrumentById(Guid productId)
        {
            IInstrument? result;
            lock (instrumentsLock)
            {
                result = productStock.Find(i => i.Id.Equals(productId));
            }
            return result;
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
                lock(consumerFundsLock)
                {
                    consumerFunds -= instrument.Price;
                }
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
                lock (instrumentsLock)
                {
                    foreach (var instrument in productStock)
                    {
                        instrument.Price *= inflationRate;
                    }
                }
                // Notify listeners of the inflation change
                PriceChange?.Invoke(this, new ChangePriceEventArgs(inflationRate));
            }
        }
    }
}

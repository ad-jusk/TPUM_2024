using ServerData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ServerData
{
    internal class ShopData : IShopData
    {
        private readonly Dictionary<Guid, IInstrument> items = new Dictionary<Guid, IInstrument>();
        private float customerFunds = 1000000f;

        private bool priceChangeEnabled;

        private object itemsLock = new object();
        private object inflationLock = new object();

        public event EventHandler<PriceInflationEventArgs>? InflationChanged;

        public ShopData()
        {
            AddInstrument(new Instrument("Pianino", InstrumentType.String, 5000, 2014, 10));
            AddInstrument(new Instrument("Gitara", InstrumentType.String, 2200, 2020, 20));
            AddInstrument(new Instrument("Trąbka", InstrumentType.Wind, 1500, 2018, 5));
            AddInstrument(new Instrument("Flet", InstrumentType.Wind, 1100, 2018, 5));
            AddInstrument(new Instrument("Harmonijka", InstrumentType.Wind, 900, 2018, 5));
            AddInstrument(new Instrument("Tamburyn", InstrumentType.Percussion, 200, 2018, 5));
            AddInstrument(new Instrument("Bęben", InstrumentType.Percussion, 800, 2018, 5));
            SimulatePriceChange();
            priceChangeEnabled = true;
        }

        ~ShopData()
        {
            priceChangeEnabled = false;
            lock (inflationLock) { }
        }

        private async void SimulatePriceChange()
        {
            while (true)
            {
                Random random = new Random();
                float waitSeconds = (float)random.NextDouble() * 5f + 5f;
                await Task.Delay((int)Math.Truncate(waitSeconds * 1000f));

                float inflation = (float)random.NextDouble() + 0.5f;

                lock (itemsLock)
                {
                    foreach (IInstrument item in items.Values)
                    {
                        item.Price *= inflation;
                    }
                }

                InflationChanged?.Invoke(this, new PriceInflationEventArgs(inflation));

                lock (inflationLock)
                {
                    if (!priceChangeEnabled)
                    {
                        break;
                    }
                }
            }
        }

        public void SellInstrument(Guid instrumentId)
        {
            lock (itemsLock)
            {
                if (items.ContainsKey(instrumentId))
                {
                    items[instrumentId].Quantity -= 1;
                    customerFunds -= items[instrumentId].Price;
                }
            }
        }

        public List<IInstrument> GetInstruments()
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (itemsLock)
            {
                result.AddRange(items.Values.Select(item => new Instrument(item)));
            }
            return result;
        }

        public void AddInstrument(IInstrument instrument)
        {
            lock (itemsLock)
            {
                items.Add(instrument.Id, instrument);
            }
        }

        public void RemoveInstrument(Guid instrumentId)
        {
            lock (itemsLock)
            {
                items.Remove(instrumentId);
            }
        }

        public IInstrument GetInstrumentByID(Guid instrumentId)
        {
            IInstrument result;
            lock (itemsLock)
            {
                if (items.ContainsKey(instrumentId))
                {
                    result = items[instrumentId];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return result;
        }

        public float GetCustomerFunds()
        {
            return customerFunds;
        }
    }
}
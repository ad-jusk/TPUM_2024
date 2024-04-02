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
        private bool priceChangeEnabled;

        private object itemsLock = new object();
        private object inflationLock = new object();

        public event EventHandler<PriceChangedEventArgs>? PriceChanges;

        public ShopData()
        {             
            AddItem(new Instrument("Pianino", InstrumentType.String, 5000, 2014, 10));
            AddItem(new Instrument("Fortepian", InstrumentType.String, 6000, 2014, 10));
            AddItem(new Instrument("Gitara", InstrumentType.String, 2200, 2020, 20));
            AddItem(new Instrument("Trąbka", InstrumentType.Wind, 1500, 2018, 5));
            AddItem(new Instrument("Flet", InstrumentType.Wind, 1100, 2018, 5));
            AddItem(new Instrument("Harmonijka", InstrumentType.Wind, 900, 2018, 5));
            AddItem(new Instrument("Tamburyn", InstrumentType.Percussion, 200, 2018, 5));
            AddItem(new Instrument("Bęben", InstrumentType.Percussion, 800, 2018, 5));
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

                PriceChanges?.Invoke(this, new PriceChangedEventArgs(inflation));

                lock (inflationLock)
                {
                    if (!priceChangeEnabled)
                    {
                        break;
                    }
                }
            }
        }

        public void SellItem(Guid itemId)
        {
            lock (itemsLock)
            {
                if (items.ContainsKey(itemId))
                {
                    items[itemId].Quantity -=1;
                }
            }
        }

        public List<IInstrument> GetItems()
        {
            List<IInstrument> result = new List<IInstrument>();
            lock (itemsLock)
            {
                result.AddRange(items.Values.Select(item => new Instrument(item)));
            }
            return result;
        }

        public void AddItem(IInstrument itemToAdd)
        {
            lock (itemsLock)
            {
                items.Add(itemToAdd.Id, itemToAdd);
            }
        }

        public void RemoveItem(Guid itemIdToRemove)
        {
            lock (itemsLock)
            {
                items.Remove(itemIdToRemove);
            }
        }

        public IInstrument GetItemByID(Guid guid)
        {
            IInstrument result;
            lock (itemsLock)
            {
                if (items.ContainsKey(guid))
                {
                    result = items[guid];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return result;
        }
    }
}
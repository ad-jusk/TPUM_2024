using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace DataTest
{
    public class DataApiMock : DataAbstractApi
    {
        private readonly IShopRepository shopRepository = new ShopRepositoryMock();

        public override IShopRepository GetShopRepository()
        {
            return shopRepository;
        }
    }

    public class InstrumentMock : IInstrument
    {
        public InstrumentMock(string name, InstrumentCategory category, decimal price, int year, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }

    public class ShopRepositoryMock : IShopRepository
    {

        private readonly List<IInstrument> instrumentStock;
        private decimal consumerFunds;
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;

        public ShopRepositoryMock()
        {
            this.instrumentStock = new List<IInstrument>()
            {
                new InstrumentMock("Pianino", InstrumentCategory.String, 5000, 2014, 10),
                new InstrumentMock("Fortepian", InstrumentCategory.String, 6000, 2014, 10),
                new InstrumentMock("Gitara", InstrumentCategory.String, 2200, 2020, 20),
                new InstrumentMock("Trąbka", InstrumentCategory.Wind, 1500, 2018, 5),
                new InstrumentMock("Flet", InstrumentCategory.Wind, 1100, 2018, 5),
                new InstrumentMock("Harmonijka", InstrumentCategory.Wind, 900, 2018, 5),
                new InstrumentMock("Tamburyn", InstrumentCategory.Percussion, 200, 2018, 5),
                new InstrumentMock("Bęben", InstrumentCategory.Percussion, 800, 2018, 5),
            };
            this.consumerFunds = 1000000M;
        }

        public void AddInstrument(IInstrument instrument)
        {
            instrumentStock.Add(instrument);
        }

        public void AddInstruments(List<IInstrument> instrumentsToAdd)
        {
            instrumentStock.AddRange(instrumentsToAdd);
        }

        public void ChangeConsumerFunds(Guid instrumentId, decimal instrumentPrice)
        {
            IInstrument? instrument = instrumentStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Price > 0 && instrument.Quantity > 0)
            {
                consumerFunds -= instrument.Price;
            }
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            IInstrument? instrument = instrumentStock.Find(i => i.Id.Equals(instrumentId));
            if (instrument != null && instrument.Quantity > 0)
            {
                instrument.Quantity -= 1;
            }
        }

        public IList<IInstrument> GetAllInstruments()
        {
            return instrumentStock;
        }

        public IInstrument? GetInstrumentById(Guid productId)
        {
            return instrumentStock.Find(i => i.Id.Equals(productId));
        }

        public IList<IInstrument> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return new ReadOnlyCollection<IInstrument>(instrumentStock.Where(i => i.Category == category).ToList());
        }

        public void RemoveInstrument(IInstrument instrument)
        {
            throw new NotImplementedException();
        }

        public decimal GetConsumerFunds()
        {
            return consumerFunds;
        }

        public void ChangeConsumerFunds(Guid instrumentId)
        {
            IInstrument? i = instrumentStock.Find(i => i.Id.Equals(instrumentId));
            if (i != null)
            {
                consumerFunds -= i.Price;
            }
        }

        public IDisposable Subscribe(IObserver<IInstrument> observer)
        {
            throw new NotImplementedException();
        }

        public Task Connect(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}

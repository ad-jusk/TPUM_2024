using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTest
{
    internal class DataApiMock : DataAbstractApi
    {
        private readonly IShopData shop = new ShopDataMock();

        public override IConnectionService GetConnectionService()
        {
            return null;
        }

        public override IShopData GetShop()
        {
            return shop;
        }
    }

    internal class ShopDataMock : IShopData
    {
        private readonly Dictionary<Guid, IInstrument> instruments = new Dictionary<Guid, IInstrument>();

        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        public ShopDataMock()
        {
            AddInstrument(new InstrumentMock("Pianino", InstrumentType.String, 5000, 2014, 10));
            AddInstrument(new InstrumentMock("Fortepian", InstrumentType.String, 6000, 2014, 10));
            AddInstrument(new InstrumentMock("Gitara", InstrumentType.String, 2200, 2020, 20));
            AddInstrument(new InstrumentMock("Trąbka", InstrumentType.Wind, 1500, 2018, 5));
            AddInstrument(new InstrumentMock("Tamburyn", InstrumentType.Percussion, 500, 2011, 2));
        }

        public void AddInstrument(IInstrument instrument)
        {
            instruments.Add(instrument.Id, instrument);
        }

        public IInstrument GetInstrumentByID(Guid instrumentId)
        {
            IInstrument? instrument = instruments[instrumentId];
            if (instrument == null)
            {
                throw new KeyNotFoundException();
            }
            return instrument;
        }

        public List<IInstrument> GetInstruments()
        {
            return new List<IInstrument>(instruments.Values);
        }

        public float GetCustomerFunds()
        {
            throw new NotImplementedException();
        }

        public Task SellInstrument(Guid instrumentId)
        {
            throw new NotImplementedException();
        }

        public List<IInstrument> GetInstrumentsByType(InstrumentType type)
        {
            return instruments.Values
                .Where(item => item.Type == type)
                .ToList();
        }

        public IDisposable Subscribe(IObserver<InflationChangedEventArgs> observer)
        {
            return null;
        }

        public IDisposable Subscribe(IObserver<float> observer)
        {
            return null;
        }
    }

    internal class InstrumentMock : IInstrument
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public InstrumentType Type { get; }
        public float Price { get; set; }
        public int Year { get; }
        public int Quantity { get; set; }

        public InstrumentMock(string name, InstrumentType type, float price, int year, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}

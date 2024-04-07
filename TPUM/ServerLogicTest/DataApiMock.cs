using ServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogicTest
{
    internal class DataApiMock : DataAbstractApi
    {
        private readonly IShopData shop = new ShopDataMock();

        public override IShopData GetShop()
        {
            return shop;
        }
    }

    internal class ShopDataMock : IShopData
    {
        // NOT USED IN MOCK
        public event EventHandler<PriceInflationEventArgs> InflationChanged;

        private readonly Dictionary<Guid, IInstrument> instruments = new Dictionary<Guid, IInstrument>();

        public ShopDataMock() 
        {
            AddInstrument(new InstrumentMock("Pianino", InstrumentType.String, 5000, 2014, 10));
            AddInstrument(new InstrumentMock("Fortepian", InstrumentType.String, 6000, 2014, 10));
            AddInstrument(new InstrumentMock("Gitara", InstrumentType.String, 2200, 2020, 20));
            AddInstrument(new InstrumentMock("Trąbka", InstrumentType.Wind, 1500, 2018, 5));
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

        public void RemoveInstrument(Guid instrumentId)
        {
            instruments.Remove(instrumentId);
        }

        public void SellInstrument(Guid instrumentId)
        {
            IInstrument instrument = GetInstrumentByID(instrumentId);
            instrument.Quantity -= 1;
        }

        public float GetCustomerFunds()
        {
            throw new NotImplementedException();
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
    }
}


using Tpum.Data;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace LogicTest
{
    public class DataApiMock : DataAbstractApi
    {
        private readonly IShopRepository shopRepository = new ShopRepositoryMock();

        public override IShopRepository GetShopRepository()
        {
            return shopRepository;
        }
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
                new InstrumentMock("instrument1", InstrumentCategory.String, 0M, 10, 10),
                new InstrumentMock("instrument2", InstrumentCategory.Percussion, 0M, 10, 10)
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            IInstrument? i = instrumentStock.Find(i => i.Equals(instrumentId));
            if (i != null)
            {
                consumerFunds -= i.Price;
            }
        }

        public IDisposable Subscribe(IObserver<IInstrument> observer)
        {
            throw new NotImplementedException();
        }
    }
}


using ClientData.Interfaces;
using Tpum.Data;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace LogicTest
{
    public class DataApiMock : DataAbstractApi
    {
        private readonly IShopRepository shopRepository = new ShopRepositoryMock();

        public override IConnectionService GetConnectionService()
        {
            throw new NotImplementedException();
        }

        public override IShopRepository GetShopRepository()
        {
            return shopRepository;
        }
    }

    public class ShopRepositoryMock : IShopRepository
    {

        private readonly List<IInstrument> instrumentStock;
        private float consumerFunds;
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        public event EventHandler<ChangePriceEventArgs> PriceChanged;
        public event Action? ItemsUpdated;
        public event Action<bool>? TransactionFinish;

        public ShopRepositoryMock()
        {
            this.instrumentStock = new List<IInstrument>()
            {
                new InstrumentMock("instrument1", InstrumentType.String, 0F, 10, 10),
                new InstrumentMock("instrument2", InstrumentType.Percussion, 0F, 10, 10)
            };
            this.consumerFunds = 1000000F;
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

        public IList<IInstrument> GetInstrumentsByCategory(InstrumentType category)
        {
            throw new NotImplementedException();
        }

        public void RemoveInstrument(IInstrument instrument)
        {
            throw new NotImplementedException();
        }

        public float GetConsumerFunds()
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

        public void RequestServerForUpdate()
        {
            throw new NotImplementedException();
        }

        public List<IInstrument> GetInstruments()
        {
            throw new NotImplementedException();
        }

        public List<IInstrument> GetInstrumentsByType(InstrumentType type)
        {
            throw new NotImplementedException();
        }

        public Task SellInstrument(Guid instrumentId)
        {
            throw new NotImplementedException();
        }
    }
}

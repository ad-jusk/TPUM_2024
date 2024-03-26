
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
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;

        public ShopRepositoryMock()
        {
            this.instrumentStock = new List<IInstrument>()
            {
                new InstrumentMock("instrument1", InstrumentCategory.String, 0M, 10, 10),
                new InstrumentMock("instrument2", InstrumentCategory.Percussion, 0M, 10, 10)
            };
        }

        public void AddInstrument(IInstrument instrument)
        {
            instrumentStock.Add(instrument);
        }

        public void AddInstruments(List<IInstrument> instrumentsToAdd)
        {
            instrumentStock.AddRange(instrumentsToAdd);
        }

        public void ChangeInstrumentPrice(Guid instrumentId, decimal instrumentPrice)
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
    }
}

using Tpum.ServerData;
using Tpum.ServerData.DataModels;
using Tpum.ServerData.Enums;
using Tpum.ServerData.Interfaces;

namespace ServerDataTest
{
    [TestClass]
    public class ServerDataTest
    {
        private static DataAbstractApi PrepareDataLayer()
        {
            return new DataApiMock();
        }

        [TestMethod]
        public void ShouldAddInstrument()
        {
            DataAbstractApi api = PrepareDataLayer();
            IInstrument instrument = new Instrument("a", InstrumentCategory.Wind, 0, 0, 0);
            api.GetShopRepository().AddInstrument(instrument);
            Assert.IsTrue(api.GetShopRepository().GetAllInstruments().Contains(instrument));
        }

        [TestMethod]
        public void ShouldGetAllInstruments()
        {
            DataAbstractApi api = PrepareDataLayer();
            Assert.AreNotEqual(api.GetShopRepository().GetAllInstruments().Count, 0);
        }

        [TestMethod]
        public void ShouldGetAllInstrumentsByCategory()
        {
            DataAbstractApi api = PrepareDataLayer();
            InstrumentCategory category = InstrumentCategory.Wind;
            IList<IInstrument> instruments = api.GetShopRepository().GetInstrumentsByCategory(category.ToString());
            foreach (IInstrument i in instruments)
            {
                Assert.AreEqual(i.Category, category);
            }
        }

        [TestMethod]
        public void ShouldGetInstrumentById()
        {
            DataAbstractApi api = PrepareDataLayer();
            IInstrument instrument1 = api.GetShopRepository().GetAllInstruments()[0];
            IInstrument? instrument2 = api.GetShopRepository().GetInstrumentById(instrument1.Id);
            Assert.IsNotNull(instrument2);
            Assert.AreEqual(instrument1.Id, instrument2.Id);
        }

        [TestMethod]
        public void ShouldDecrementInstrumentQuantity()
        {
            DataAbstractApi api = PrepareDataLayer();
            IInstrument i = api.GetShopRepository().GetAllInstruments()[0];
            int oldQuantity = i.Quantity;
            api.GetShopRepository().DecrementInstrumentQuantity(i.Id);
            Assert.AreEqual(oldQuantity - 1, i.Quantity);
        }
        [TestMethod]
        public void GetConsumerFunds_ReturnsCorrectFunds()
        {
            DataAbstractApi api = PrepareDataLayer();
            var funds = api.GetShopRepository().GetConsumerFunds();

            Assert.AreEqual(1000000M, funds); 
        }

        [TestMethod]
        public void ShouldChangeConsumerFunds_Correctly()
        {
            DataAbstractApi api = PrepareDataLayer();
            var initialFunds = api.GetShopRepository().GetConsumerFunds();
            var instrumentId = Guid.NewGuid();
            var instrument = new InstrumentMock("Pianino", InstrumentCategory.String, 5000, 2014, 1);
            api.GetShopRepository().AddInstrument(instrument);
            var expectedFunds = initialFunds - instrument.Price;

            api.GetShopRepository().ChangeConsumerFunds(instrumentId);
            var resultFunds = api.GetShopRepository().GetConsumerFunds();

            Assert.AreNotEqual(expectedFunds, resultFunds);
        }
    }
}
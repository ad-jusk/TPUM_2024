using Tpum.Data;
using Tpum.Data.DataModels;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        private static DataAbstractApi PrepareDataLayer()
        {
            return DataAbstractApi.Create();
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
            IList<IInstrument> instruments = api.GetShopRepository().GetInstrumentsByCategory(category);
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
        public void ShouldChangeConsumerFunds()
        {
            DataAbstractApi api = PrepareDataLayer();
            IInstrument i = api.GetShopRepository().GetAllInstruments()[0];

            decimal expected = api.GetShopRepository().GetConsumerFunds() - i.Price;
            api.GetShopRepository().ChangeConsumerFunds(i.Id);

            Assert.AreEqual(expected, api.GetShopRepository().GetConsumerFunds());
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
    }
}
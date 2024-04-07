using Data;
using LogicTest;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        private static DataAbstractApi createApi()
        {
            return new DataApiMock();
        }

        [TestMethod]
        public void Should_Get_Instruments()
        {
            DataAbstractApi data = createApi();
            List<IInstrument> instruments = data.GetShop().GetInstruments();
            Assert.IsTrue(instruments.Count != 0);
        }

        [TestMethod]
        public void Should_Get_Instrument_ById()
        {
            DataAbstractApi data = createApi();
            IInstrument toSell = data.GetShop().GetInstruments().First();
            IInstrument byId = data.GetShop().GetInstrumentByID(toSell.Id);

            Assert.AreEqual(toSell.Id, byId.Id);  
        }

        [TestMethod]
        public void Should_Get_Instrument_ByType()
        {
            DataAbstractApi data = createApi();
            List<IInstrument> byType = data.GetShop().GetInstrumentsByType(InstrumentType.String);

            foreach(var i in byType)
            {
                Assert.AreEqual(InstrumentType.String, i.Type);
            }
        }
    }
}
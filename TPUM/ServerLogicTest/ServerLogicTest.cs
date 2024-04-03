using Tpum.ServerData.DataModels;
using Tpum.ServerData.Enums;
using Tpum.ServerLogic;

namespace ServerLogicTest
{
    [TestClass]
    public class ServerLogicTest
    {
        private readonly LogicAbstractApi logicApi = LogicAbstractApi.Create(new DataApiMock());

        [TestMethod]
        public void ShouldGetAllInstruments()
        {
            //Assert.AreEqual(logicApi.GetStore().GetAllInstruments().Count, 2);
        }

        [TestMethod]
        public void ShouldGetInstrumentById()
        {
            InstrumentDTO instrument = logicApi.GetStore().GetAvailableInstruments()[0];
            Assert.AreEqual(instrument.Id, logicApi.GetStore().GetInstrumentById(instrument.Id).Id);
        }

        [TestMethod]
        public void ShouldDecrementInstrumentQuantity()
        {
            InstrumentDTO i = logicApi.GetStore().GetAvailableInstruments()[0];

            logicApi.GetStore().DecrementInstrumentQuantity(i.Id);
            InstrumentDTO i2 = logicApi.GetStore().GetInstrumentById(i.Id);

            Assert.AreEqual(i.Quantity - 1, i2.Quantity);
        }

        [TestMethod]
        public void ShouldSellInstrument_Returns_True_If_Successful()
        {
            var store = logicApi.GetStore();
            var instrumentId = Guid.NewGuid();
            var instrument = new InstrumentDTO(instrumentId, "Pianino", "String", 5000, 2014, 1);

            var result = store.SellInstrument(instrument);

            Assert.IsTrue(result);
        }
               [TestMethod]
        public void ShouldSellInstrument_Returns_False_If_Instrument_Quantity_Zero()
        {
            var store = logicApi.GetStore();
            var instrumentId = Guid.NewGuid();
            var instrument = new InstrumentDTO(instrumentId, "Pianino", "String", 5000, 2014, 0);

            var result = store.SellInstrument(instrument);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldSellInstrument_Returns_False_If_Instrument_Null()
        {
            var store = logicApi.GetStore();

            var result = store.SellInstrument(null);

            Assert.IsFalse(result);
        }
    }
}
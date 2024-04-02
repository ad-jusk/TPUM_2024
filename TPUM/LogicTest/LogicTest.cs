using Logic;
using Tpum.Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        private readonly LogicAbstractApi logicApi = LogicAbstractApi.Create(new DataApiMock());

        [TestMethod]
        public void ShouldGetAllInstruments()
        {
/*            Assert.AreEqual(logicApi.GetStore().GetAvailableInstruments().Count, 2);*/
        }

        [TestMethod]
        public void ShouldGetInstrumentById()
        {
/*            InstrumentLogic instrument = logicApi.GetStore().GetAvailableInstruments()[0];
            Assert.AreEqual(instrument.Id, logicApi.GetStore().GetInstrumentById(instrument.Id).Id);*/
        }

        [TestMethod]
        public void ShouldDecrementInstrumentQuantity()
        {
/*            InstrumentLogic i = logicApi.GetStore().GetAvailableInstruments()[0];

            logicApi.GetStore().DecrementInstrumentQuantity(i.Id);
            InstrumentLogic i2 = logicApi.GetStore().GetInstrumentById(i.Id);

            Assert.AreEqual(i.Quantity - 1, i2.Quantity);*/
        }
    }
}
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
            //Assert.AreEqual(logicApi.GetStore().GetAllInstruments().Count, 2);
        }

        [TestMethod]
        public void ShouldGetInstrumentById()
        {
            InstrumentDTO instrument = logicApi.GetStore().GetAllInstruments()[0];
            Assert.AreEqual(instrument.Id, logicApi.GetStore().GetInstrumentById(instrument.Id).Id);
        }

        [TestMethod]
        public void ShouldDecrementInstrumentQuantity()
        {
            InstrumentDTO i = logicApi.GetStore().GetAllInstruments()[0];

            logicApi.GetStore().DecrementInstrumentQuantity(i.Id);
            InstrumentDTO i2 = logicApi.GetStore().GetInstrumentById(i.Id);

            Assert.AreEqual(i.Quantity - 1, i2.Quantity);
        }
    }
}
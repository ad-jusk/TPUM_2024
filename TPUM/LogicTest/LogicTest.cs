using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        private static LogicAbstractApi createApi()
        {
            return LogicAbstractApi.Create(new DataApiMock());
        }

        [TestMethod]
        public void Should_Get_Instruments()
        {
            LogicAbstractApi logic = createApi();
            List<IInstrumentLogic> instruments = logic.GetShop().GetInstruments();
            Assert.IsTrue(instruments.Count != 0);
        }

        [TestMethod]
        public void Should_Get_Instruments_ByType()
        {
            LogicAbstractApi logic = createApi();
            List<IInstrumentLogic> instruments = logic.GetShop().GetInstrumentsByType(LogicInstrumentType.String);
            
            foreach (var i in instruments)
            {
                Assert.AreEqual(LogicInstrumentType.String, i.Type);
            }
        }
    }
}

using ServerData;
using ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogicTest
{
    [TestClass]
    public class LogicTest
    {
        private static readonly LogicAbstractApi logic = LogicAbstractApi.Create(new DataApiMock());

        [TestMethod]
        public void Should_Get_Instruments()
        {
            List<IInstrumentLogic> instruments = logic.GetShop().GetInstruments();
            Assert.IsTrue(instruments.Count != 0);
        }

        [TestMethod]
        public void Should_Sell_Instrument()
        {
            IInstrumentLogic toSell = logic.GetShop().GetInstruments().First();
            int quantityBeforeSell = toSell.Quantity;

            logic.GetShop().SellInstrument(toSell.Id);

            IInstrumentLogic afterSell = logic.GetShop().GetInstruments().Find(x => x.Id == toSell.Id)!;

            Assert.AreEqual(quantityBeforeSell - 1, afterSell.Quantity);
        }
    }
}

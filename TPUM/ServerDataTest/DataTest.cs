using ServerData;

namespace ServerDataTest
{
    [TestClass]
    public class DataTest
    {
        private static DataAbstractApi createApi()
        {
            return DataAbstractApi.Create();
        }

        [TestMethod]
        public void Should_Get_Instruments()
        {
            DataAbstractApi data = createApi();
            List<IInstrument> instruments = data.GetShop().GetInstruments();
            Assert.IsTrue(instruments.Count != 0);
        }

        [TestMethod]
        public void Should_Sell_Instrument()
        {
            DataAbstractApi data = createApi();
            IInstrument toSell = data.GetShop().GetInstruments().First();
            int quantityBeforeSell = toSell.Quantity;

            data.GetShop().SellInstrument(toSell.Id);

            IInstrument afterSell = data.GetShop().GetInstruments().Find(x => x.Id == toSell.Id)!;

            Assert.AreEqual(quantityBeforeSell - 1, afterSell.Quantity);
        }

        [TestMethod]
        public void Should_Remove_Instrument()
        {
            DataAbstractApi data = createApi();
            Guid toRemove = data.GetShop().GetInstruments().First().Id;

            data.GetShop().RemoveInstrument(toRemove);

            List<Guid> ids = data.GetShop().GetInstruments().Select(i => i.Id).ToList();

            Assert.IsFalse(ids.Contains(toRemove));
        }
    }
}
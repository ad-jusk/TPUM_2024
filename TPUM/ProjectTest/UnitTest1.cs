namespace ProjectTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(5 + 5 == 10);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.IsFalse(5 - 4 == 0);
        }
    }
}
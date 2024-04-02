using ServerData;

namespace ServerLogic
{
    public abstract class LogicAbstractApi
    {
        public DataAbstractApi DataApi { get; private set; }

        public LogicAbstractApi(DataAbstractApi dataApi)
        {
            DataApi = dataApi;
        }

        public static LogicAbstractApi Create(DataAbstractApi? dataApi = null)
        {
            DataAbstractApi dataAbstractApi = dataApi ?? DataAbstractApi.Create();
            return new LogicApi(dataAbstractApi);
        }

        public abstract IShopLogic GetShop();
    }

    public class PriceChangedEventArgsLogic : EventArgs
    {
        public float NewPrice { get; }

        public PriceChangedEventArgsLogic(float newPrice)
        {
            this.NewPrice = newPrice;
        }

        internal PriceChangedEventArgsLogic(PriceChangedEventArgs args)
        {
            this.NewPrice = args.NewPrice;
        }
    }

    public enum InstrumentTypeLogic
    {
        String = 0,
        Wind = 1,
        Percussion = 2,
    }

    public interface IInstrumentLogic
    {
        Guid Id { get; }
        string Name { get; }
        InstrumentTypeLogic Type { get; }
        float Price { get; }
        int Year { get; }
        int Quantity { get; }
    }

    public interface IShopLogic
    {
        public event EventHandler<PriceChangedEventArgsLogic> PriceChanged;

        public void SellItem(Guid itemId);

        public List<IInstrumentLogic> GetItems();
    }
}
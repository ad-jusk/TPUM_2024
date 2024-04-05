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

    public class InflationChangedEventArgsLogic : EventArgs
    {
        public float NewInflation { get; }

        public InflationChangedEventArgsLogic(float newInflation)
        {
            this.NewInflation = newInflation;
        }

        internal InflationChangedEventArgsLogic(PriceInflationEventArgs args)
        {
            this.NewInflation = args.NewInflation;
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
        public event EventHandler<InflationChangedEventArgsLogic> InflationChanged;

        public void SellInstrument(Guid instrumentId);

        public List<IInstrumentLogic> GetInstruments();
    }
}
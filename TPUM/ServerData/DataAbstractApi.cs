using System.ComponentModel;

namespace ServerData
{
    public abstract class DataAbstractApi
    {
        public static DataAbstractApi Create()
        {
            return new DataApi();
        }

        public abstract IShopData GetShop();
    }

    public enum InstrumentType
    {
        String = 0,
        Wind = 1,
        Percussion = 2
    }

    public interface IInstrument
    {
        Guid Id { get; }
        string Name { get; }
        InstrumentType Type { get; }
        float Price { get; set; }
        int Year { get; }
        int Quantity { get; set; }
    }

    public class PriceInflationEventArgs : EventArgs
    {
        public float NewInflation { get; }

        public PriceInflationEventArgs(float newInflation)
        {
            this.NewInflation = newInflation;
        }
    }

    public interface IShopData
    {
        public event EventHandler<PriceInflationEventArgs> InflationChanged;

        public void SellInstrument(Guid instrumentId);

        public IInstrument GetInstrumentByID(Guid instrumentId);
        public List<IInstrument> GetInstruments();

        public void AddInstrument(IInstrument instrument);
        public void RemoveInstrument(Guid instrumentId);

        public float GetCustomerFunds();
    }
}
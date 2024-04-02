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
        int Year { get;}
        int Quantity { get; set; }
    }

    public class PriceChangedEventArgs : EventArgs
    {
        public float NewPrice { get; }

        public PriceChangedEventArgs(float newPrice)
        {
            this.NewPrice = newPrice;
        }
    }

    public interface IShopData
    {
        public event EventHandler<PriceChangedEventArgs> PriceChanges;

        public void SellItem(Guid itemId);

        public IInstrument GetItemByID(Guid guid);
        public List<IInstrument> GetItems();

        public void AddItem(IInstrument itemToAdd);
        public void RemoveItem(Guid itemIdToRemove);
    }
}
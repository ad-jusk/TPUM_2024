using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public static DataAbstractApi Create(IConnectionService? connectionService)
        {
            return new DataApi(connectionService);
        }

        public abstract IShopData GetShop();
        public abstract IConnectionService GetConnectionService();
    }

    public enum InstrumentType
    {
        String = 0,
        Wind = 1,
        Percussion = 2
    }

    public interface IInstrument : ICloneable
    {
        Guid Id { get; }
        string Name { get; }
        InstrumentType Type { get; }
        float Price { get; set; }
        int Year { get; }
        int Quantity { get; set; }
    }

    public interface IShopData : IObservable<InflationChangedEventArgs>, IObservable<float>
    {
        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        public void RequestUpdate();

        public Task SellInstrument(Guid instrumentId);

        public List<IInstrument> GetInstruments();

        public IInstrument GetInstrumentByID(Guid instrumentId);
        public List<IInstrument> GetInstrumentsByType(InstrumentType type);
    }

    public interface IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;


        public Task Connect(Uri peerUri);
        public Task Disconnect();

        public bool IsConnected();

        public Task SendAsync(string message);
    }

    public class InflationChangedEventArgs : EventArgs
    {
        public float NewInflation { get; }

        public InflationChangedEventArgs(float newInflation)
        {
            this.NewInflation = newInflation;
        }
    }
}

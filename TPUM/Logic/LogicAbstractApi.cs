using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public DataAbstractApi DataApi { get; private set; }

        public LogicAbstractApi(DataAbstractApi dataApi)
        {
            DataApi = dataApi;
        }

        public static LogicAbstractApi Create(DataAbstractApi? dataAbstractApi = null)
        {
            DataAbstractApi dataApi = dataAbstractApi ?? DataAbstractApi.Create(null);
            return new Logic(dataApi);
        }

        public abstract IShopLogic GetShop();
        public abstract IConnectionServiceLogic GetConnectionService();
    }

    public enum LogicInstrumentType
    {
        String = 0,
        Wind = 1,
        Percussion = 2
    }

    public interface IInstrumentLogic
    {
        Guid Id { get; }
        string Name { get; }
        LogicInstrumentType Type { get; }
        float Price { get; }
        int Year { get; }
        int Quantity { get; }
    }

    public class LogicInflationChangedEventArgs : EventArgs
    {
        public float NewInflation { get; }

        public LogicInflationChangedEventArgs(float newInflation)
        {
            this.NewInflation = newInflation;
        }

        internal LogicInflationChangedEventArgs(InflationChangedEventArgs args)
        {
            this.NewInflation = args.NewInflation;
        }
    }

    public interface IShopLogic
    {
        public event EventHandler<LogicInflationChangedEventArgs> InflationChanged;
        public event EventHandler<float> CustomerFundsChanged;

        public event Action? InstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        public Task SellInstrument(Guid instrumentId);

        public List<IInstrumentLogic> GetInstruments();
        public List<IInstrumentLogic> GetInstrumentsByType(LogicInstrumentType type);
    }

    public interface IConnectionServiceLogic
    {
        public Task Disconnect(); 
    }
}

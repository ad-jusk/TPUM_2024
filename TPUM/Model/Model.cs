using System;
using System.Threading.Tasks;
using Logic;

namespace Presentation.Model
{
    public enum PresentationInstrumentType
    {
        String = 0,
        Wind = 1,
        Percussion = 2
    }

    public class ModelInflationChangedEventArgs : EventArgs
    {
        public float NewInflation { get; }

        public ModelInflationChangedEventArgs(float newInflation)
        {
            this.NewInflation = newInflation;
        }

        internal ModelInflationChangedEventArgs(LogicInflationChangedEventArgs args)
        {
            this.NewInflation = args.NewInflation;
        }
    }

    public class Model
    {
        private LogicAbstractApi logicAbstractApi;
        public ShopPresentation Shop { get; private set; }

        public event Action? InstrumentsUpdated;

        public Model(LogicAbstractApi? logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi ?? LogicAbstractApi.Create();

            Shop = new ShopPresentation(this.logicAbstractApi.GetShop());
        }

        public async Task SellInstrument(Guid instrumentId)
        {
            await logicAbstractApi.GetShop().SellInstrument(instrumentId);
        }
    }
}
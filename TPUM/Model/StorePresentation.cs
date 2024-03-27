using Tpum.Data.Enums;
using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    public class StorePresentation
    {
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangePriceEventArgs>? PriceChange;
        private readonly IStore store;

        public StorePresentation(IStore store)
        {
            this.store = store;
            this.store.ProductQuantityChange += OnQuantityChanged;
            this.store.ConsumerFundsChange += OnConsumerFundsChanged;
            this.store.PriceChange += OnPriceChanged;
        }

        public List<InstrumentPresentation> GetInstruments()
        {
            return store.GetAvailableInstruments()
                .Select(i => new InstrumentPresentation(i.Id, i.Name, i.Category, i.Price, i.Year, i.Quantity))
                .ToList();
        }

        public List<InstrumentPresentation> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return store.GetAvailableInstruments()
                .Where(i => i.Category == category.ToString())
                .Select(i => new InstrumentPresentation(i.Id, i.Name, i.Category, i.Price, i.Year, i.Quantity))
                .ToList();
        }
        public InstrumentPresentation GetInstrumentById(Guid id)
        {
            InstrumentDTO instrument = store.GetInstrumentById(id);
            if (instrument != null)
            {
                return new InstrumentPresentation(instrument.Id, instrument.Name, instrument.Category, instrument.Price, instrument.Year, instrument.Quantity);
            }
            return null;
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            store.DecrementInstrumentQuantity(instrumentId);
        }

        public decimal GetConsumerFunds()
        {
            return store.GetConsumerFunds();
        }

        public void ChangeConsumerFunds(Guid instrumentId)
        {
            store.ChangeConsumerFunds(instrumentId);
        }

        private void OnQuantityChanged(object sender, Tpum.Logic.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange?.Invoke(this, new Tpum.Presentation.Model.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }

        private void OnConsumerFundsChanged(object sender, Tpum.Logic.ChangeConsumerFundsEventArgs e)
        {
            ConsumerFundsChange?.Invoke(this, new Tpum.Presentation.Model.ChangeConsumerFundsEventArgs(e.Funds));
        }

        private void OnPriceChanged(object sender, Tpum.Logic.ChangePriceEventArgs e)
        {
            PriceChange?.Invoke(this, new Tpum.Presentation.Model.ChangePriceEventArgs(e));
        }
    }
}

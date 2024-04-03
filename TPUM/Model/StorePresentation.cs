using Tpum.Logic;
using Tpum.Logic.Interfaces;
using Tpum.Presentation.Model.Interfaces;

namespace Tpum.Presentation.Model
{
    public class StorePresentation : IStorePresentation
    {
        public event EventHandler<decimal> ConsumerFundsChangeCS;
        public event EventHandler<InstrumentPresentation> InstrumentChange;
        public event EventHandler<InstrumentPresentation> TransactionSucceeded;
        private readonly IStore store;

        public StorePresentation(IStore store)
        {
            this.store = store;
            this.store.ConsumerFundsChangeCS += OnConsumerFundsChanged;
            this.store.InstrumentChange += OnInstrumentChanged;
            this.store.TransactionSucceeded += OnTransactionSucceeded;
        }

        public async Task SendMessageAsync(string message)
        {
            await this.store.SendMessageAsync(message);
        }

        public async Task SellInstrument(InstrumentPresentation instrument)
        {
            InstrumentDTO instrumentDTO = store.GetInstrumentById(instrument.Id);
            await store.SellInstrument(instrumentDTO);
        }

        public List<InstrumentPresentation> GetInstruments()
        {
            return store.GetAllInstruments()
                .Select(i => new InstrumentPresentation(i.Id, i.Name, i.Category, i.Price, i.Year, i.Quantity))
                .ToList();
        }

        public List<InstrumentPresentation> GetInstrumentsByCategory(string category)
        {
            return store.GetAllInstruments()
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

        private void OnConsumerFundsChanged(object sender, decimal funds)
        {
            ConsumerFundsChangeCS?.Invoke(this, funds);
        }

        private void OnInstrumentChanged(object? sender, InstrumentDTO e)
        {
            EventHandler<InstrumentPresentation> handler = InstrumentChange;
            InstrumentPresentation Instrument = new InstrumentPresentation(e.Id, e.Name, e.Category, e.Price, e.Year, e.Quantity);
            handler?.Invoke(this, Instrument);
        }

        private void OnTransactionSucceeded(object? sender,InstrumentDTO e)
        {
            EventHandler<InstrumentPresentation> handler = TransactionSucceeded;
            InstrumentPresentation instrumentPresentation = new InstrumentPresentation(e.Id, e.Name, e.Category,
                    e.Price, e.Year, e.Quantity);
            handler?.Invoke(this, instrumentPresentation);
        }
    }
}

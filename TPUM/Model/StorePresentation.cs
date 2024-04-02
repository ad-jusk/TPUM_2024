using Tpum.Data.Enums;
using Tpum.Logic;
using Tpum.Logic.Interfaces;
using Tpum.Logic.WebSocket;
using Tpum.Presentation.Model.Interfaces;

namespace Tpum.Presentation.Model
{
    public class StorePresentation : IStorePresentation
    {
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangePriceEventArgs>? PriceChange;
        public event EventHandler<InstrumentPresentation> InstrumentChange;
        public event EventHandler<InstrumentPresentation> TransactionSucceeded;
        private readonly IStore store;
        private IConnectionService connectionService;
        public StorePresentation(IStore store)
        {
            this.store = store;
            this.store.ProductQuantityChange += OnQuantityChanged;
            this.store.ConsumerFundsChange += OnConsumerFundsChanged;
            this.store.PriceChange += OnPriceChanged;
            this.store.InstrumentChange += OnInstrumentChanged;
            this.store.TransactionSucceeded += OnTransactionSucceeded;
            connectionService = ConnectionFactory.CreateConnectionService;


        }
        public async Task SendMessageAsync(string message)
        {
            await this.store.SendMessageAsync(message);
        }
        public Task<bool> Connect(Uri uri)
        {
            return connectionService.Connect(uri);
        }
        public async Task Disconnect()
        {
            await connectionService.Disconnect();
        }

        public bool IsConnected()
        {
            return connectionService.Connected;
        }
        public List<InstrumentPresentation> GetInstruments()
        {
            return store.GetAvailableInstruments()
                .Select(i => new InstrumentPresentation(i.Id, i.Name, i.Category, i.Price, i.Year, i.Quantity))
                .ToList();
        }

        public List<InstrumentPresentation> GetInstrumentsByCategory(string category)
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

        public async Task SellInstrument(InstrumentPresentation instrument)
        {
            InstrumentDTO instrumentDTO = store.GetInstrumentById(instrument.Id);
            store.SellInstrument(instrumentDTO);
        }
        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            store.DecrementInstrumentQuantity(instrumentId);
        }

        public void ChangeConsumerFunds(Guid instrumentId)
        {
            store.ChangeConsumerFunds(instrumentId);
        }
        public decimal GetConsumerFunds()
        {
            return store.GetConsumerFunds();
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
        private void OnInstrumentChanged(object? sender, InstrumentDTO e)
        {
            EventHandler<InstrumentPresentation> handler = InstrumentChange;
            InstrumentPresentation Instrument = new InstrumentPresentation(e.Id, e.Name, e.Category, e.Price, e.Year, e.Quantity);
            handler?.Invoke(this, Instrument);
        }

        private void OnTransactionSucceeded(object? sender,InstrumentDTO e)
        {
            EventHandler<InstrumentPresentation> handler = TransactionSucceeded;
            InstrumentPresentation fruitPresentation = new InstrumentPresentation(e.Id, e.Name, e.Category,
                    e.Price, e.Year, e.Quantity);
            handler?.Invoke(this, fruitPresentation);
        }
    }
}

using Tpum.ServerData.Enums;
using Tpum.ServerData.Interfaces;
using Tpum.ServerLogic.Interfaces;

namespace Tpum.ServerLogic
{
    public class Store : IStore
    {
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangePriceEventArgs> PriceChange;
        private readonly IShopRepository shopRepository;
        
        public Store(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
            this.shopRepository.ConsumerFundsChange += OnConsumerFundsChanged;
            this.shopRepository.ProductQuantityChange += OnQuantityChanged;
            this.shopRepository.PriceChange += OnFundsChanged;
        }

        public List<InstrumentDTO> GetAvailableInstruments()
        {
            return shopRepository.GetAllInstruments()
                .Select(i => new InstrumentDTO(i.Id, i.Name, i.Category.ToString(), i.Price, i.Year, i.Quantity))
                .ToList();
        }

        public InstrumentDTO GetInstrumentById(Guid id)
        {
            IInstrument? i = shopRepository.GetInstrumentById(id) ?? throw new ArgumentException("Instrumnent with id " + id + " not found");
            return new InstrumentDTO(i.Id, i.Name, i.Category.ToString(), i.Price, i.Year, i.Quantity);
        }

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return shopRepository.GetInstrumentsByCategory(category)
                .Select(i => new InstrumentDTO(i.Id, i.Name, i.Category.ToString(), i.Price, i.Year, i.Quantity))
                .ToList();
        }

        public bool SellInstrument(InstrumentDTO instrument)
        {
            if (instrument == null) return false;
            DecrementInstrumentQuantity(instrument.Id);
            ChangeConsumerFunds(instrument.Id);
            return true;
        }
        public decimal GetConsumerFunds()
        {
            return shopRepository.GetConsumerFunds();
        }

        public void ChangeConsumerFunds(Guid instrumentId)
        {
            shopRepository.ChangeConsumerFunds(instrumentId);
        }

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            shopRepository.DecrementInstrumentQuantity(instrumentId);
        }

        private void OnConsumerFundsChanged(object sender, Tpum.ServerData.ChangeConsumerFundsEventArgs e)
        {
            ConsumerFundsChange?.Invoke(this, new Tpum.ServerLogic.ChangeConsumerFundsEventArgs(e.Funds));
        }

        private void OnQuantityChanged(object sender, Tpum.ServerData.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange?.Invoke(this, new Tpum.ServerLogic.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }

        private void OnFundsChanged(object sender, Tpum.ServerData.ChangePriceEventArgs e)
        {
            PriceChange?.Invoke(this, new Tpum.ServerLogic.ChangePriceEventArgs(e));
        }

    }
}

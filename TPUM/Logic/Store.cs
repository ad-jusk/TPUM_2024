using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
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
                .Select(i => new InstrumentDTO { Id = i.Id, Name = i.Name, Category = i.Category.ToString(), Price = i.Price, Year = i.Year, Quantity = i.Quantity })
                .ToList();
        }

        public InstrumentDTO GetInstrumentById(Guid id)
        {
            IInstrument? i = shopRepository.GetInstrumentById(id) ?? throw new ArgumentException("Instrumnent with id " + id + " not found");
            return new InstrumentDTO { Id = i.Id, Name = i.Name, Category = i.Category.ToString(), Price = i.Price, Year = i.Year, Quantity = i.Quantity };
        }

        public List<InstrumentDTO> GetInstrumentsByCategory(InstrumentCategory category)
        {
            return shopRepository.GetInstrumentsByCategory(category)
                .Select(i => new InstrumentDTO { Id = i.Id, Name = i.Name, Category = i.Category.ToString(), Price = i.Price, Year = i.Year, Quantity = i.Quantity })
                .ToList();
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

        private void OnConsumerFundsChanged(object sender, Tpum.Data.ChangeConsumerFundsEventArgs e)
        {
            ConsumerFundsChange?.Invoke(this, new Tpum.Logic.ChangeConsumerFundsEventArgs(e.Funds));
        }

        private void OnQuantityChanged(object sender, Tpum.Data.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange?.Invoke(this, new Tpum.Logic.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }

        private void OnFundsChanged(object sender, Tpum.Data.ChangePriceEventArgs e)
        {
            PriceChange?.Invoke(this, new Tpum.Logic.ChangePriceEventArgs(e));
        }

    }
}

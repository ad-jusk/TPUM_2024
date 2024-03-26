using Tpum.Data.Enums;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class Store : IStore
    {
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        private readonly IShopRepository shopRepository;
        
        public Store(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
            this.shopRepository.ProductPriceChange += OnPriceChanged;
            this.shopRepository.ProductQuantityChange += OnQuantityChanged;
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

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            shopRepository.DecrementInstrumentQuantity(instrumentId);
        }

        private void OnPriceChanged(object sender, Tpum.Data.ChangeProductPriceEventArgs e)
        {
            ProductPriceChange?.Invoke(this, new Tpum.Logic.ChangeProductPriceEventArgs(e.Id, e.Price));
        }
        private void OnQuantityChanged(object sender, Tpum.Data.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange?.Invoke(this, new Tpum.Logic.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }
    }
}

using Logic;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

namespace Tpum.Logic
{
    public class Store : IStore
    {
        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        public event EventHandler<ChangeProductAgeEventArgs> ProductYearChange;
        private readonly IShopRepository shopRepository;
        
        public Store(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
            this.shopRepository.ProductPriceChange += OnPriceChanged;
        }

        public List<InstrumentDTO> GetAvailableInstruments()
        {
            return shopRepository.GetAllInstruments()
                .Select(i => new InstrumentDTO { Id = i.Id, Name = i.Name, Category = i.Category.ToString(), Price = i.Price, Year = i.Year, Quantity = i.Quantity })
                .ToList();
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
    }
}

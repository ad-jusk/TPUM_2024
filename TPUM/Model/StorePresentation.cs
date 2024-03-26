using Tpum.Data.Enums;
using Tpum.Logic.Interfaces;

namespace Tpum.Presentation.Model
{
    public class StorePresentation
    {
        public event EventHandler<Tpum.Presentation.Model.ChangeProductQuantityEventArgs> ProductQuantityChange;
        private readonly IStore store;

        public StorePresentation(IStore store)
        {
            this.store = store;
            this.store.ProductQuantityChange += OnQuantityChanged; 
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

        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            store.DecrementInstrumentQuantity(instrumentId);
        }

        private void OnQuantityChanged(object sender, Tpum.Logic.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange.Invoke(this, new Tpum.Presentation.Model.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }
    }
}

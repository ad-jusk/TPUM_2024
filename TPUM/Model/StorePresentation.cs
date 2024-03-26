using Logic;
using Tpum.Data.Enums;

namespace Tpum.Presentation.Model
{
    public class StorePresentation
    {
        public event EventHandler<Tpum.Presentation.Model.ChangeProductAgeEventArgs> ProductAgeChange;
        private readonly IStore store;

        public StorePresentation(IStore store)
        {
            this.store = store;
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

        private void OnAgeChanged(object sender, Tpum.Logic.ChangeProductAgeEventArgs e)
        {
            ProductAgeChange.Invoke(this, new Tpum.Presentation.Model.ChangeProductAgeEventArgs(e.Id, e.Age));
        }
    }
}

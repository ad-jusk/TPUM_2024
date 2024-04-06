using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using Model;
using Presentation.Model;

namespace Presentation.Model
{
    public class ShopPresentation
    {
        private IShopLogic Shop { get; set; }

        public event EventHandler<ModelInflationChangedEventArgs>? InflationChanged;
        public Action? OnInstrumentsUpdated;
        public event Action<bool>? TransactionFinish;

        public ShopPresentation(IShopLogic shop)
        {
            this.Shop = shop;

            shop.InstrumentsUpdated += () => OnInstrumentsUpdated?.Invoke();
            shop.InflationChanged += (obj, args) => InflationChanged?.Invoke(this, new ModelInflationChangedEventArgs(args));
            shop.TransactionFinish += succeeded => TransactionFinish?.Invoke(succeeded);
        }

        public List<InstrumentPresentation> GetInstruments()
        {
            return Shop.GetInstruments()
                .Select(i => new InstrumentPresentation(i))
                .ToList();
        }

        public List<InstrumentPresentation> GetInstrumentsByType(PresentationInstrumentType type)
        {
            return Shop.GetInstrumentsByType((LogicInstrumentType)type)
                .Select(i => new InstrumentPresentation(i))
                .ToList();
        }
    }
}
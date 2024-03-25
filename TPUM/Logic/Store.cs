using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class Store : IStore
    {
        public Store(IShopRepository shopRepository)
        {
            this._shopRepository = shopRepository;
            shopRepository.ProductPriceChange += OnPriceChanged;
        }


        private IShopRepository _shopRepository;

        public bool Sell(List<Instrument> instruments)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ChangeProductPriceEventArgs> ProductPriceChange;
        private void OnPriceChanged(object sender, Tpum.Data.ChangeProductPriceEventArgs e)
        {
            ProductPriceChange?.Invoke(this, new Tpum.Logic.ChangeProductPriceEventArgs(e.Id, e.Price));
        }

    }
}

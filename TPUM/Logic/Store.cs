using System;
using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;

namespace Tpum.Logic
{
    public class Store : IStore, IObserver<IInstrument>, IObserver<decimal>
    {
        public event EventHandler<decimal> ConsumerFundsChangeCS;
        public event EventHandler<InstrumentDTO> InstrumentChange;
        public event EventHandler<InstrumentDTO> TransactionSucceeded;
        private readonly IShopRepository shopRepository;
        private readonly IDisposable unsubcriber;
        
        public Store(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
            //this.shopRepository.ProductQuantityChange += OnQuantityChanged;
            //this.shopRepository.PriceChange += OnPriceChanged;
            this.shopRepository.TransactionSucceeded += OnTransactionSucceeded;
            //shopRepository.Subscribe(this);
            shopRepository.Subscribe((IObserver<IInstrument>)this);
            shopRepository.Subscribe((IObserver<decimal>)this);

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

        public List<InstrumentDTO> GetInstrumentsByCategory(string category)
        {
            return shopRepository.GetInstrumentsByCategory(category)
                .Select(i => new InstrumentDTO { Id = i.Id, Name = i.Name, Category = i.Category.ToString(), Price = i.Price, Year = i.Year, Quantity = i.Quantity })
                .ToList();
        }


        public void DecrementInstrumentQuantity(Guid instrumentId)
        {
            shopRepository.DecrementInstrumentQuantity(instrumentId);
        }
        public decimal GetConsumerFunds()
        {
            return shopRepository.GetConsumerFunds();
        }

        public async Task SendMessageAsync(string message)
        {
            await shopRepository.SendMessageAsync(message);
        }
        public async Task SellInstrument(InstrumentDTO instrumentDTO)
        {
            IInstrument instrument = shopRepository.GetInstrumentById(instrumentDTO.Id);

            if (instrument != null && instrument.Quantity > 0)
                instrument.Price = instrumentDTO.Price;


            await shopRepository.TryBuyingInstrument(instrument);
        }
/*        private void OnQuantityChanged(object sender, Tpum.Data.ChangeProductQuantityEventArgs e)
        {
            ProductQuantityChange?.Invoke(this, new Tpum.Logic.ChangeProductQuantityEventArgs(e.Id, e.Quantity));
        }

        private void OnPriceChanged(object sender, Tpum.Data.ChangePriceEventArgs e)
        {
            PriceChange?.Invoke(this, new Tpum.Logic.ChangePriceEventArgs(e));
        }*/
        private void OnTransactionSucceeded(object sender, IInstrument instrument)
        {
            InstrumentDTO InstrumentDTO = new InstrumentDTO();
            InstrumentDTO.Id = instrument.Id;
            InstrumentDTO.Name = instrument.Name;
            InstrumentDTO.Category = instrument.Category.ToString();
            InstrumentDTO.Price = instrument.Price;
            InstrumentDTO.Year = instrument.Year;
            InstrumentDTO.Quantity = instrument.Quantity;

            TransactionSucceeded?.Invoke(this, InstrumentDTO);
        }
        public void OnCompleted()
        {
            this.unsubcriber.Dispose();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"An error occurred during event subscription: {error.Message}");
        }

        public void OnNext(IInstrument value)
        {
            var instrumentDTO = new InstrumentDTO();
            instrumentDTO.Id = value.Id;
            instrumentDTO.Name = value.Name;
            instrumentDTO.Category = value.Category.ToString();
            instrumentDTO.Year = value.Year;
            instrumentDTO.Price = value.Price;
            instrumentDTO.Quantity = value.Quantity;

            if (value.Price < -0.01m && value.Name == "")
            {
                //InstrumentChange?.Invoke(this, dto);
            }
            else
                InstrumentChange.Invoke(this, instrumentDTO);
        }
        public void OnNext(decimal consumerFunds)
        {
            //decimal consumerFundsDTO = 0M;

            ConsumerFundsChangeCS.Invoke(this, consumerFunds);
        }
    }
}

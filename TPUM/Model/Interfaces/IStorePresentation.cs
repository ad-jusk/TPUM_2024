﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Presentation.Model.Interfaces
{
    internal interface IStorePresentation
    {
        public event EventHandler<ChangeProductQuantityEventArgs> ProductQuantityChange;
        public event EventHandler<ChangeConsumerFundsEventArgs> ConsumerFundsChange;
        public event EventHandler<ChangePriceEventArgs>? PriceChange;
        public event EventHandler<InstrumentPresentation> InstrumentChange;
        public event EventHandler<InstrumentPresentation> TransactionSucceeded;

        public Task SendMessageAsync(string message);
        public List<InstrumentPresentation> GetInstruments();
        public List<InstrumentPresentation> GetInstrumentsByCategory(string category);
        public InstrumentPresentation GetInstrumentById(Guid id);
        public void DecrementInstrumentQuantity(Guid instrumentId);
        public void ChangeConsumerFunds(Guid instrumentId);
        public decimal GetConsumerFunds();

    }
}
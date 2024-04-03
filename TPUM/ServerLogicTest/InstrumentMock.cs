﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.ServerData.Enums;
using Tpum.ServerData.Interfaces;

namespace ServerLogicTest
{
    public class InstrumentMock : IInstrument
    {

        public InstrumentMock(string name, InstrumentCategory category, decimal price, int year, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Price = price;
            Year = year;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public InstrumentCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

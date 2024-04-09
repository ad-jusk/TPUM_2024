using System;

namespace Commons
{
    [Serializable]
    public abstract class ServerCommand
    {
        public string Header;

        protected ServerCommand(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public class GetInstrumentsAndFundsCommand : ServerCommand
    {
        public static string StaticHeader = "GetInstrumentsAndFunds";

        public GetInstrumentsAndFundsCommand() : base(StaticHeader)
        {
        }
    }

    [Serializable]
    public class GetInstrumentsCommand : ServerCommand
    {
        public static string StaticHeader = "GetInstruments";

        public GetInstrumentsCommand(): base(StaticHeader)
        {
        }
    }

    [Serializable]
    public class SellInstrumentCommand : ServerCommand
    {
        public static string StaticHeader = "SellInstrument";

        public Guid TransactionID;
        public Guid InstrumentID;

        public SellInstrumentCommand(Guid id) : base(StaticHeader)
        {
            TransactionID = Guid.NewGuid();
            InstrumentID = id;
        }
    }

    [Serializable]
    public class InstrumentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }

        public InstrumentDTO()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Type = string.Empty;
            Price = 0f;
            Year = 0;
            Quantity = 0;
        }

        public InstrumentDTO(Guid id, string name, string type, float price, int year, int quantity)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Year = year;
            Quantity = quantity;
        }
    }

    [Serializable]
    public class NewPriceDTO
    { 

        public Guid InstrumentID { get; set; }
        public float NewPrice { get; set; }

        public NewPriceDTO() { }

        public NewPriceDTO(Guid instrumentId, float newPrice)
        {
            InstrumentID = instrumentId;
            NewPrice = newPrice;
        }
    }

    [Serializable]
    public abstract class ServerResponse
    {
        public string Header { get; private set; }

        protected ServerResponse(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public class GetInstrumentsAndFundsResponse : ServerResponse
    {
        public static readonly string StaticHeader = "GetAllData";

        public InstrumentDTO[]? Instruments;
        public float Funds;

        public GetInstrumentsAndFundsResponse() : base(StaticHeader)
        {
        }
    }

    [Serializable]
    public class UpdateAllResponse : ServerResponse
    {
        public static readonly string StaticHeader = "UpdateAllItems";

        public InstrumentDTO[]? Instruments;

        public UpdateAllResponse() : base(StaticHeader)
        {
        }
    }

    [Serializable]
    public class InflationChangedResponse : ServerResponse
    {
        public static readonly string StaticHeader = "InflationChanged";

        public float NewInflation;
        public NewPriceDTO[]? NewPrices;

        public InflationChangedResponse() : base(StaticHeader)
        {
        }

    }

    [Serializable]
    public class TransactionResponse : ServerResponse
    {
        public static readonly string StaticHeader = "TransactionResponse";

        public Guid TransactionId;
        public bool Succeeded;
        public float CustomerFunds;

        public TransactionResponse() : base(StaticHeader)
        {
        }
    }
}
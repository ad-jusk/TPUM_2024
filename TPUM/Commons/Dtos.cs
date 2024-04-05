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
    public struct InstrumentDTO
    {
        public Guid Id;
        public string Name;
        public string Type;
        public float Price;
        public int Year;
        public int Quantity;

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
    public struct NewPriceDTO
    {
        public Guid InstrumentID;
        public float NewPrice;

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

        public TransactionResponse() : base(StaticHeader)
        {
        }
    }
}
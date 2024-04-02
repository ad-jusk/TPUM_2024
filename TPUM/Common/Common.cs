using System.Text.Json.Serialization;

namespace Common
{
    // CLIENT COMMANDS

    [Serializable]
    public abstract class Command
    {
        public string Header { get; set; }

        public Command() { }

        public Command(string header)
        {
            this.Header = header;
        }
    }

    [Serializable]
    public class GetItemsCommand : Command
    {
        public static readonly string SHeader = "GetItems";

        public GetItemsCommand() : base(SHeader) { }
    }

    [Serializable]
    public class SellItemCommand : Command
    {
        public static readonly string SHeader = "SellItem";
        public Guid ItemId;

        public SellItemCommand() : base(SHeader) { }
    }

    // SERVER RESPONSES

    public abstract class Response
    {
        public string Header { get; private set; }

        private Response() { }

        [JsonConstructor]
        public Response(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public class UpdateAllResponse : Response
    {
        public static readonly string SHeader = "UpdateAllItems";

        public InstrumentDTO[]? Items;

        public UpdateAllResponse() : base(SHeader) { }
    }

    [Serializable]
    public class PriceChangedResponse : Response
    {
        public static readonly string SHeader = "PriceChanged";
        public NewPriceDTO[]? NewPrices;

        public PriceChangedResponse() : base(SHeader) { }
    }

    [Serializable]
    public class TransactionResponse : Response
    {
        public static readonly string SHeader = "TransactionResponse";
        public bool Succeeded;

        public TransactionResponse() : base(SHeader) { }
    }

    // DTOS

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
        public Guid ItemID;
        public float NewPrice;

        public NewPriceDTO(Guid itemId, float newPrice)
        {
            ItemID = itemId;
            NewPrice = newPrice;
        }
    }

}


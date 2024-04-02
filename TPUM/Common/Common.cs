namespace Common
{
    // CLIENT COMMANDS

    public abstract class Command
    {
        public string Header;

        protected Command(string header)
        {
            Header = header;
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

    [Serializable]
    public class UpdateAllResponse
    {
        public static readonly string Header = "UpdateAllItems";

        public InstrumentDTO[]? Items;
    }

    [Serializable]
    public class PriceChangedResponse
    {
        public static readonly string Header = "PriceChanged";
        public NewPriceDTO[]? NewPrices;
    }

    [Serializable]
    public class TransactionResponse
    {
        public static readonly string Header = "TransactionResponse";
        public bool Succeeded;
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


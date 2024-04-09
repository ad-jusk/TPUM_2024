using Commons;
using GeneratedClasses;

namespace ServerLogicTest
{
    [TestClass]
    public class SerializationTest
    {

        private static readonly JsonSerializer serializer = new JsonSerializer();

        [TestMethod]
        public void Should_Serialize_And_Deserialize_InstrumentDTO()
        {
            Guid id = Guid.NewGuid();
            string name = "Test name";
            string type = "Test type";
            float price = 12.99f;
            int year = 2001;
            int quantity = 10;

            InstrumentDTO dto = new InstrumentDTO(id, name, type, price, year, quantity);

            string json = serializer.Serialize(dto);

            InstrumentDTO deserialized = serializer.Deserialize<InstrumentDTO>(json);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(id, deserialized.Id);
            Assert.AreEqual(name, deserialized.Name);
            Assert.AreEqual(type, deserialized.Type);
            Assert.AreEqual(price, deserialized.Price);
            Assert.AreEqual(year, deserialized.Year);
            Assert.AreEqual(quantity, deserialized.Quantity);
        }

        [TestMethod]
        public void Should_Serialize_And_Deserialize_NewPriceDTO()
        {
            Guid id = Guid.NewGuid();
            float price = 124.54f;

            NewPriceDTO dto = new NewPriceDTO(id, price);

            string json = serializer.Serialize(dto);

            NewPriceDTO deserialized = serializer.Deserialize<NewPriceDTO>(json);

            Assert.IsNotNull (deserialized);
            Assert.AreEqual(id, deserialized.InstrumentID);
            Assert.AreEqual(price, deserialized.NewPrice);
        }

        [TestMethod]
        public void Should_Get_Header_From_Command()
        {
            ServerCommand command = new GetInstrumentsCommand();

            string json = serializer.Serialize(command);
            
            string? header = serializer.GetHeader(json);

            Assert.AreEqual (header, GetInstrumentsCommand.StaticHeader);
        }

        [TestMethod]
        public void Should_Get_Header_From_Response()
        {
            ServerResponse response = new UpdateAllResponse();

            string json = serializer.Serialize(response);

            string? header = serializer.GetHeader(json);

            Assert.AreEqual(header, UpdateAllResponse.StaticHeader);
        }

        [TestMethod]
        public void Should_Serialize_And_Deserialize_NewPriceDTOGenerated()
        {
            Guid id = Guid.NewGuid();
            float price = 124.54f;

            NewPriceDTOGenerated dto = new NewPriceDTOGenerated
            {
                InstrumentID = id,
                NewPrice = price
            };

            string json = serializer.Serialize(dto);

            NewPriceDTOGenerated deserialized = serializer.Deserialize<NewPriceDTOGenerated>(json);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(id, deserialized.InstrumentID);
            Assert.AreEqual(price, deserialized.NewPrice);
        }

        [TestMethod]
        public void Should_Serialize_And_Deserialize_InstrumentDTOGenerated()
        {
            Guid id = Guid.NewGuid();
            string name = "Test name";
            string type = "Test type";
            float price = 12.99f;
            int year = 2001;
            int quantity = 10;

            InstrumentDTOGenerated dto = new InstrumentDTOGenerated()
            {
                Id = id,
                Name = name,
                Type = type,
                Price = price,
                Year = year,
                Quantity = quantity
            };

            string json = serializer.Serialize(dto);

            InstrumentDTOGenerated deserialized = serializer.Deserialize<InstrumentDTOGenerated>(json);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(id, deserialized.Id);
            Assert.AreEqual(name, deserialized.Name);
            Assert.AreEqual(type, deserialized.Type);
            Assert.AreEqual(price, deserialized.Price);
            Assert.AreEqual(year, deserialized.Year);
            Assert.AreEqual(quantity, deserialized.Quantity);
        }
    }
}
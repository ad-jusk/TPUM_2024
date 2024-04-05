using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commons
{
    public class JsonSerializer
    {
        public string Serialize<T>(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public T Deserialize<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

        // RETURNS COMMAND OR RESPONSE HEADER
        public string? GetHeader(string message)
        {
            JObject jObject = JObject.Parse(message);
            if (jObject.ContainsKey("Header"))
            {
                return (string)jObject["Header"];
            }
            return null;
        }
    }
}
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace ClientData
{
    internal class Serializer
    {
        internal static string Serialize<T>(T objectToSerialize)
        {
            return JsonSerializer.Serialize(objectToSerialize, new JsonSerializerOptions { TypeInfoResolver = new DefaultJsonTypeInfoResolver()});
        }

        internal static T Deserialize<T>(string message)
        {
            return JsonSerializer.Deserialize<T>(message, new JsonSerializerOptions { TypeInfoResolver = new DefaultJsonTypeInfoResolver() });
        }
    }
}

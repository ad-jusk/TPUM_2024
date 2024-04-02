using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerPresentation
{
    internal class Serializer
    {
        internal static string Serialize<T>(T objectToSerialize)
        {
            return JsonSerializer.Serialize(objectToSerialize);
        }

        internal static T Deserialize<T>(string message)
        {
            return JsonSerializer.Deserialize<T>(message);
        }

        internal static string? GetHeaderForCommand(string message)
        {
            Command command = Deserialize<Command>(message);
            return command != null ? command.Header : null;
        }
    }
}

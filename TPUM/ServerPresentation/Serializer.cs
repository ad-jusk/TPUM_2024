using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tpum.ServerLogic;

namespace ServerPresentation
{
    internal class Serializer
    {
        internal static string InstrumentToJSON(InstrumentDTO weapon)
        {
            return JsonSerializer.Serialize(weapon);
        }

        internal static InstrumentDTO JSONToInstrument(string json)
        {
            return JsonSerializer.Deserialize<InstrumentDTO>(json)!;
        }

        internal static string InstrumentsToJSON(List<InstrumentDTO> weapons)
        {
            return JsonSerializer.Serialize(weapons);
        }

        internal static List<InstrumentDTO> JSONToInstruments(string json)
        {
            return new List<InstrumentDTO>(JsonSerializer.Deserialize<List<InstrumentDTO>>(json)!);
        }
    }
}

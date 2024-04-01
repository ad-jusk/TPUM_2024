using System.Text.Json;
using System.Text.Json.Serialization;
using Tpum.Data.DataModels;
using Tpum.Data.Interfaces;

namespace Data
{
    internal abstract class Serializer
    {
        public static string InstrumentToJSON(IInstrument instrument)
        {
            return JsonSerializer.Serialize(instrument);
        }

        public static IInstrument JSONToInstrument(string json)
        {
            return JsonSerializer.Deserialize<Instrument>(json)!;
        }

        public static string InstrumentsToJSON(List<IInstrument> weapons)
        {
            return JsonSerializer.Serialize(weapons);
        }

        public static List<IInstrument> JSONToInstruments(string json)
        {
            return new List<IInstrument>(JsonSerializer.Deserialize<List<Instrument>>(json)!);
        }
    }
}

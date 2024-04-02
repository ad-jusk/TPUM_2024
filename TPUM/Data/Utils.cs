using Common;
using System.Text;
using Tpum.Data.DataModels;
using Tpum.Data.Enums;
using Tpum.Data.Interfaces;

internal static class Utils
{
    public static IInstrument ToInstrument(this InstrumentDTO instrumentDTO)
    {
        return new Instrument(
            instrumentDTO.Id,
            instrumentDTO.Name,
            InstrumentTypeFromString(instrumentDTO.Type),
            instrumentDTO.Price,
            instrumentDTO.Year,
            instrumentDTO.Quantity
        );
    }

    public static ArraySegment<byte> GetArraySegment(this string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        return new ArraySegment<byte>(buffer);
    }

    public static InstrumentType InstrumentTypeFromString(string typeAsString)
    {
        return (InstrumentType)Enum.Parse(typeof(InstrumentType), typeAsString);
    }

    public static string ToString(this InstrumentType typeAsString)
    {
        return Enum.GetName(typeof(InstrumentType), typeAsString) ?? throw new InvalidOperationException();
    }
}
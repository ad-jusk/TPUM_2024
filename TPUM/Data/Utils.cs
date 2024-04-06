using Commons;
using Data;
using System;
using System.ComponentModel;
using System.Text;

namespace Data
{
    internal static class Utils
    {
        public static InstrumentType ItemTypeFromString(string typeAsString)
        {
            return (InstrumentType)Enum.Parse(typeof(InstrumentType), typeAsString);
        }

        public static string ToString(this InstrumentType typeAsString)
        {
            return Enum.GetName(typeof(InstrumentType), typeAsString) ?? throw new InvalidOperationException();
        }

        public static IInstrument ToInstrument(this InstrumentDTO instrumentDto)
        {
            return new Instrument(
                instrumentDto.Id,
                instrumentDto.Name,
                ItemTypeFromString(instrumentDto.Type),
                instrumentDto.Price,
                instrumentDto.Year,
                instrumentDto.Quantity
            );
        }

        public static ArraySegment<byte> GetArraySegment(this string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }
    }
}

using Commons;
using ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPresentation
{
    internal static class Utils
    {
        public static InstrumentDTO ToDTO(this IInstrumentLogic item)
        {
            return new InstrumentDTO(
                item.Id,
                item.Name,
                item.Type.ToString(),
                item.Price,
                item.Year,
                item.Quantity
            );
        }

        public static ArraySegment<byte> GetArraySegment(this string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }
    }
}

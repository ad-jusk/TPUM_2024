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
        internal static string WeaponToJSON(InstrumentDTO weapon)
        {
            return JsonSerializer.Serialize(weapon);
        }

        internal static InstrumentDTO JSONToWeapon(string json)
        {
            return JsonSerializer.Deserialize<InstrumentDTO>(json)!;
        }

        internal static string WarehouseToJSON(List<InstrumentDTO> weapons)
        {
            return JsonSerializer.Serialize(weapons);
        }

        internal static List<InstrumentDTO> JSONToWarehouse(string json)
        {
            return new List<InstrumentDTO>(JsonSerializer.Deserialize<List<InstrumentDTO>>(json)!);
        }
    }
}

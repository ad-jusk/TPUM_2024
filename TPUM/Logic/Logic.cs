using Tpum.Data.Interfaces;
using Tpum.Logic.Interfaces;
using static System.Formats.Asn1.AsnWriter;

namespace Tpum.Logic
{
    public class Logic : ILogic
    {
        public Logic(IData data)
        {
            Data = data;
            Store = new Store(Data.ShopRepository);
        }
        public IStore Store { get; }
        private IData Data { get; }
    }
}

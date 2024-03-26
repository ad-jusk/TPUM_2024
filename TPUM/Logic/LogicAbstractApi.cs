using Tpum.Data;
using Tpum.Logic;
using Tpum.Logic.Interfaces;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public DataAbstractApi DataAbstractApi { get; private set; }

        public LogicAbstractApi(DataAbstractApi dataAbstractApi)
        {
            DataAbstractApi = dataAbstractApi;
        }

        public static LogicAbstractApi Create(DataAbstractApi dataApi = null)
        {
            if(dataApi == null)
            {
                 return new LogicApi(DataAbstractApi.Create());
            }
            return new LogicApi(dataApi);
        }

        public abstract IStore GetStore();
    }
}

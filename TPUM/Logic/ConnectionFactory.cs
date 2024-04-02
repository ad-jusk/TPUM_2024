using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tpum.Logic.WebSocket;

namespace Tpum.Logic
{
    public static class ConnectionFactory
    {
        public static IConnectionService CreateConnectionService => new ConnectionService();
    }
}


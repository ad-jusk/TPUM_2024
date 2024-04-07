using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class ConnectionServiceLogic : IConnectionServiceLogic
    {
        private readonly IConnectionService connectionService;

        public ConnectionServiceLogic(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }

        public async Task Disconnect()
        {
            await connectionService.Disconnect();
        }
    }
}

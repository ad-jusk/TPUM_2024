using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Logic.WebSocket
{
    public interface IConnectionService
    {
        public bool Connected { get; }
        public Task<bool> Connect(Uri peerUri);
        public Task Disconnect();
    }
}

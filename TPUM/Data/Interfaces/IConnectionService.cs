using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Interfaces
{
    public interface IConnectionService
    {
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;

        public Task Connect(Uri uri);
        public Task Disconnect();
        public Task SendAsync(string message);

        public bool IsConnected();
    }
}

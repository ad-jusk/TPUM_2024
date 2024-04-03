using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Tpum.Data.Interfaces;
using Tpum.Data.WebSocket;

namespace Data.WebSocket
{
    public class ConnectionService : IConnectionService
    {
        public WebSocketConnection Connection { get; private set; }
        public Action OnConnected { get; set; }

        public Action<string> ConnectionLogger = msg => { Console.WriteLine(msg); };

        public bool Connected
        {
            get
            {
                if (Connection != null)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> Connect(Uri peerUri)
        {
            try
            {
                ConnectionLogger?.Invoke($"Establishing connection to {peerUri.OriginalString}");

                Connection = await WebSocketClient.Connect(peerUri, ConnectionLogger);
                OnConnected?.Invoke();

                return await Task.FromResult(true);
            }
            catch (WebSocketException e)
            {
                Debug.WriteLine($"Caught web socket exception {e.Message}");
                ConnectionLogger?.Invoke(e.Message);
                return await Task.FromResult(false);
            }

        }

        public async Task Disconnect()
        {
            await Connection.DisconnectAsync();
            Connection = null;
        }

    }
}

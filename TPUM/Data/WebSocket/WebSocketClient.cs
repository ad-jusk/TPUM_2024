using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Tpum.Data.WebSocket
{
    public static class WebSocketClient
    {
        public static async Task<WebSocketConnection> Connect(Uri peer, Action<string> log)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            // Connect to WebSocket server
            await clientWebSocket.ConnectAsync(peer, CancellationToken.None);

            switch (clientWebSocket.State)
            {
                case WebSocketState.Open:
                    log($"Opening WebSocket connection to remote server {peer}");
                    WebSocketConnection socket = new ClientWebSocketConnection(clientWebSocket, peer, log);
                    return socket;
                default:
                    log?.Invoke($"Cannot connect to remote node status {clientWebSocket.State}");
                    throw new WebSocketException($"Cannot connect to remote node status {clientWebSocket.State}");
            }
        }

        private class ClientWebSocketConnection : WebSocketConnection
        {

            private ClientWebSocket _clientWebSocket;
            private Uri _peer;
            private readonly Action<string> _log;
            public ClientWebSocketConnection(ClientWebSocket clientWebSocket, Uri peer, Action<string> log)
            { 
                _clientWebSocket = clientWebSocket;
                _peer = peer;
                _log = log;
                Task.Factory.StartNew(() => ClientMessageLoop());
            }
            public override Task DisconnectAsync()
            {
                return _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown procedure started", CancellationToken.None);
            }
            public override string ToString()
            {
                return _peer.ToString();
            }
            protected override Task SendTask(string message)
            {
                return _clientWebSocket.SendAsync(message.GetArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            private void ClientMessageLoop() 
            {
                try{                
                    // Stores websocket data in byttes form
                    byte[] buffer = new byte[1024 * 8];
                    while (true)
                    {
                        if (_clientWebSocket.State == WebSocketState.Aborted)
                        {
                            onClose?.Invoke();
                            return;
                        }
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                        WebSocketReceiveResult result = _clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            onClose?.Invoke();
                            _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "I am closing", CancellationToken.None).Wait();
                            return;
                        }
                        int count = result.Count;
                        while (!result.EndOfMessage)
                        {
                            if (count >= buffer.Length)
                            {
                                onClose?.Invoke();
                                _clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None).Wait();
                                return;
                            }
                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            result = _clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                            count -= result.Count;
                        }
                        string message = Encoding.UTF8.GetString(buffer, 0, count);
                        onMessage?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    _log($"Connection has been broken because of an exception {ex}");
                    _clientWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Connection has been broken because of an exception", CancellationToken.None).Wait();
                    onClose?.Invoke();
                }
            }
        }
    }
}

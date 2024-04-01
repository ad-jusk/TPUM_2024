using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerPresentation
{
    internal static class WebSocketServer
    {
        public static WebSocketConnection CurrentConnection { get; set; }

        public static async Task Server(int p2pPort, Action<WebSocketConnection> onWebSocketConnection)
        {
            Uri uri = new Uri($@"http://localhost:{p2pPort}/");
            await ServerLoop(uri, onWebSocketConnection);
        }

        private static async Task ServerLoop(Uri uri, Action<WebSocketConnection> onWebSocketConnection)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add(uri.ToString());
            server.Start();
            while (true)
            {
                HttpListenerContext httpContext = await server.GetContextAsync();
                if (!httpContext.Request.IsWebSocketRequest)
                {
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.Close();
                }
                HttpListenerWebSocketContext context = await httpContext.AcceptWebSocketAsync(null);
                WebSocketConnection websocket = new ServerWebSocketConncetion(context.WebSocket, httpContext.Request.RemoteEndPoint);
                onWebSocketConnection.Invoke(websocket);
            }
        }

        private class ServerWebSocketConncetion : WebSocketConnection
        {
            private WebSocket _serverWebSocket = null;
            private IPEndPoint _remoteEndPoint;

            public ServerWebSocketConncetion(WebSocket webSocket, IPEndPoint remoteEndPoint)
            {
                _serverWebSocket = webSocket;
                _remoteEndPoint = remoteEndPoint;
                Task.Factory.StartNew(() => ServerMessageLoop(webSocket));
            }
            public override Task DisconnectAsync()
            {
                return _serverWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown procedure started", CancellationToken.None);
            }

            public override string ToString()
            {
                return _remoteEndPoint.ToString();
            }

            protected override Task SendTask(string message)
            {
                return _serverWebSocket.SendAsync(message.GetArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            private void ServerMessageLoop(WebSocket serverWebsocket)
            {
                try
                {
                    // Stores websocket data in byttes form
                    byte[] buffer = new byte[1024 * 8];
                    while (true)
                    {
                        if (serverWebsocket.State == WebSocketState.Aborted)
                        {
                            onClose?.Invoke();
                            return;
                        }
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                        WebSocketReceiveResult result = serverWebsocket.ReceiveAsync(segment, CancellationToken.None).Result;
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            onClose?.Invoke();
                            serverWebsocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "I am closing", CancellationToken.None).Wait();
                            return;
                        }
                        int count = result.Count;
                        while (!result.EndOfMessage)
                        {
                            if (count >= buffer.Length)
                            {
                                onClose?.Invoke();
                                serverWebsocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None).Wait();
                                return;
                            }
                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            result = serverWebsocket.ReceiveAsync(segment, CancellationToken.None).Result;
                            count -= result.Count;
                        }
                        string message = Encoding.UTF8.GetString(buffer, 0, count);
                        onMessage?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    serverWebsocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Connection has been broken because of an exception", CancellationToken.None).Wait();
                    onClose?.Invoke();
                }
            }
        }
    }
}
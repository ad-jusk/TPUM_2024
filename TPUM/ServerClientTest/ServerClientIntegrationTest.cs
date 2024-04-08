using Commons;
using ServerPresentation;
using Data;

namespace ServerClientTest
{
    [TestClass]
    public class ServerClientIntegrationTest
    {
        [TestMethod]
        public async Task Should_Connect_Client_And_Server()
        {
            WebSocketConnection _wserver = null;
            WebSocketConnection _wclient = null;
            const int _delay = 10;

            Uri uri = new Uri("ws://localhost:6966");
            List<string> logOutput = new List<string>();
            Task server = Task.Run(async () => await WebSocketServer.StartServer(uri.Port,
                _ws =>
                {
                    _wserver = _ws; _wserver.OnMessage = (data) =>
                    {
                        logOutput.Add($"Received message from client: {data}");
                    };
                }));
            await Task.Delay(_delay);
            _wclient = await WebSocketClient.Connect(uri, message => logOutput.Add(message));

            Assert.IsNotNull(_wserver);
            Assert.IsNotNull(_wclient);
        }
    }
}
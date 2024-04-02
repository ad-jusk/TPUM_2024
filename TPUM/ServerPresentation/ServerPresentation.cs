using ServerPresentation;
using Tpum.ServerLogic;
using Tpum.ServerLogic.Interfaces;

namespace Tpum.ServerPresentation
{
    internal class ServerPresentation
    {
        private LogicAbstractApi logic;
        private IStore store;
        
        static async Task Main(string[] args)
        {
            await new ServerPresentation().CreateServer();
        }

        private void ConnectionHandler(WebSocketConnection webSocketConnection)
        {
            Console.WriteLine("[Server]: Client connected");
            WebSocketServer.CurrentConnection = webSocketConnection;
            webSocketConnection.onMessage = ParseMessage;
            webSocketConnection.onClose = () => {
                Console.WriteLine("[Server]: Connection closed");
                WebSocketServer.CurrentConnection = null;
            };
            webSocketConnection.onError = () => {
                Console.WriteLine("[Server]: Connection error encountered");
                WebSocketServer.CurrentConnection = null;
            };
        }

        private async void ParseMessage(string message)
        {
            Console.WriteLine($"[Client]: {message}");

            if (message == "RequestInstruments")
                await SendAllInstruments();
            else if (message.StartsWith("CustomMessage"))
            {
                // Handle custom message
            }
            if (message.Contains("RequestTransaction"))
            {
                var json = message.Substring("RequestTransaction".Length);
                var instrumentToBuy = Serializer.JSONToInstrument(json);
                bool sellResult = store.SellInstrument(instrumentToBuy);
                int sellResultInt = sellResult ? 1 : 0;
                var instrument = store.GetInstrumentById(instrumentToBuy.Id);
                json = Serializer.InstrumentToJSON(instrument);

                await SendMessageAsync("TransactionResult" + sellResultInt.ToString() + (sellResult ? json : ""));
            }
        }

        private async Task SendAllInstruments()
        {
            var weapons = store.GetAvailableInstruments();
            var json = Serializer.InstrumentsToJSON(weapons);
            var message = "UpdateAll" + json;
            await SendMessageAsync(message);
        }

        private async Task SendMessageAsync(string message)
        {
            Console.WriteLine("[Server]: " + message);
            await WebSocketServer.CurrentConnection.SendAsync(message);
        }

        private async Task CreateServer()
        {
            Console.WriteLine("Server running...");
            logic = LogicAbstractApi.Create();
            store = logic.GetStore();

            //TODO: EVENTS
            store.PriceChange += async (sender, eventArgs) =>
            {
                try
                {
                    if (WebSocketServer.CurrentConnection != null)
                        await SendMessageAsync("PriceChanged" + eventArgs.NewFunds.ToString());
                    else
                        await SendMessageAsync("connection is null");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Server]: Error sending price change message: " + ex.Message);
                }
            };

            await WebSocketServer.Server(8080, ConnectionHandler);
        }
    }
}
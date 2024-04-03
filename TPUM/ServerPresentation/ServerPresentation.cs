using Commons;
using ServerPresentation;
using System;
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

            else if (message == "RequestFunds")
                await SendMessageAsync("ConsumerFundsChanged" + store.GetConsumerFunds().ToString() + "\n");

            else if (message.Contains("RequestInstrumentsById"))
                await SendAllInstrumentsById(message.Substring("RequestInstrumentsById".Length));

            else if (message == "RequestString")
                await SendInstrumentsByCategory("String");

            else if (message == "RequestWind")
                await SendInstrumentsByCategory("Wind");

            else if (message == "RequestPercussion")
                await SendInstrumentsByCategory("Percussion");

            else if (message.Contains("Echo"))
                await SendMessageAsync(message);

            else if (message.Contains("RequestTransaction"))
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
            var instruments = store.GetAvailableInstruments();
            var json = Serializer.InstrumentsToJSON(instruments);
            var message = "UpdateAll" + json;
            await SendMessageAsync(message);
        }

        private async Task SendAllInstrumentsById(string instruments)
        {
            var instrumentsDTO = Serializer.JSONToInstruments(instruments);
            var instrumentsToSend = new List<InstrumentDTO>();
            foreach(InstrumentDTO instr in instrumentsDTO)
            {
                instrumentsToSend.Add(store.GetInstrumentById(instr.Id));
            }
            var json = Serializer.InstrumentsToJSON(instrumentsToSend);
            var message = "UpdateSome" + json;
            await SendMessageAsync(message);
        }

        private async Task SendInstrumentsByCategory(string category)
        {
            var instruments = store.GetInstrumentsByCategory(category);
            var json = Serializer.InstrumentsToJSON(instruments);
            var message = "UpdateCategory" + json;
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

            //EVENTS
            store.PriceChange += async (sender, eventArgs) =>
            {
                try
                {
                    if (WebSocketServer.CurrentConnection != null)
                        await SendMessageAsync("PriceChanged\n");
                    else
                        await SendMessageAsync("connection is null");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Server]: Error sending price change message: " + ex.Message);
                }
            };

            store.ConsumerFundsChange += async (sender, eventArgs) =>
            {
                try
                {
                    if (WebSocketServer.CurrentConnection != null)
                        await SendMessageAsync("ConsumerFundsChanged " + eventArgs.Funds.ToString()+ "\n");
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
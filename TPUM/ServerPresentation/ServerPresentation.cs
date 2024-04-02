using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using ServerLogic;

namespace ServerPresentation
{
    internal class ServerPresentation
    {
        private readonly LogicAbstractApi logicAbstractApi;

        private WebSocketConnection? webSocketConnection;

        private ServerPresentation(LogicAbstractApi logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi;
            logicAbstractApi.GetShop().PriceChanged += HandleInflationChanged;
        }

        private static async Task Main(string[] args)
        {
            ServerPresentation sp = new ServerPresentation(LogicAbstractApi.Create());
            await sp.Start();
        }


        private async Task Start()
        {
            while (true)
            {
                Console.WriteLine("Waiting for connect...");
                await WebSocketServer.StartServer(8080, OnConnect);
            }
        }

        private void OnConnect(WebSocketConnection connection)
        {
            Console.WriteLine($"Connected to {connection}");

            connection.OnMessage = OnMessage;
            connection.OnError = OnError;
            connection.OnClose = OnClose;

            webSocketConnection = connection;
        }

        private async void OnMessage(string message)
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"New message: {message}");

            if (Serializer.GetHeaderForCommand(message) == GetItemsCommand.SHeader)
            {
                GetItemsCommand getItemsCommand = Serializer.Deserialize<GetItemsCommand>(message);
                await SendItems();
            }
            else if (Serializer.GetHeaderForCommand(message) == SellItemCommand.SHeader)
            {
                SellItemCommand sellItemCommand = Serializer.Deserialize<SellItemCommand>(message);

                TransactionResponse transactionResponse = new TransactionResponse();
                try
                {
                    logicAbstractApi.GetShop().SellItem(sellItemCommand.ItemId);
                    transactionResponse.Succeeded = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception while selling instrument: {exception.Message}");
                    transactionResponse.Succeeded = false;
                }

                string transactionMessage = Serializer.Serialize(transactionResponse);
                Console.WriteLine($"Send: {transactionMessage}");
                await webSocketConnection.SendAsync(transactionMessage);
            }
        }

        private async Task SendItems()
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Sending items...");

            UpdateAllResponse serverResponse = new UpdateAllResponse();
            List<IInstrumentLogic> items = logicAbstractApi.GetShop().GetItems();
            serverResponse.Items = items.Select(x => x.ToDTO()).ToArray();

            string responseJson = Serializer.Serialize(serverResponse);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private async void HandleInflationChanged(object? sender, PriceChangedEventArgsLogic args)
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"New inflation: {args.NewPrice}");

            List<IInstrumentLogic> items = logicAbstractApi.GetShop().GetItems();
            PriceChangedResponse inflationChangedResponse = new PriceChangedResponse();
            inflationChangedResponse.NewPrices = items.Select(x => new NewPriceDTO(x.Id, x.Price)).ToArray();

            string responseJson = Serializer.Serialize(inflationChangedResponse);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private void OnError()
        {
            Console.WriteLine($"Connection error");
        }

        private async void OnClose()
        {
            Console.WriteLine($"Connection closed");
            webSocketConnection = null;
        }
    }
}
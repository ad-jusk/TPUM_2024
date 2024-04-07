using Commons;
using ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerPresentation
{
    internal class ServerPresentation
    {
        private readonly LogicAbstractApi logicAbstractApi;
        private WebSocketConnection? webSocketConnection;
        private readonly JsonSerializer serializer;

        private ServerPresentation(LogicAbstractApi logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi;
            this.serializer = new JsonSerializer();
            logicAbstractApi.GetShop().InflationChanged += HandleInflationChanged;
        }

        private async Task StartConnection()
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

            if (serializer.GetHeader(message) == GetInstrumentsCommand.StaticHeader)
            {
                Task task = Task.Run(async () => await SendItems());
            }
            else if (serializer.GetHeader(message) == SellInstrumentCommand.StaticHeader)
            {
                SellInstrumentCommand command = serializer.Deserialize<SellInstrumentCommand>(message);

                TransactionResponse transactionResponse = new TransactionResponse();
                transactionResponse.TransactionId = command.TransactionID;
                try
                {
                    logicAbstractApi.GetShop().SellInstrument(command.InstrumentID);
                    transactionResponse.Succeeded = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception \"{exception.Message}\" caught during selling item");
                    transactionResponse.Succeeded = false;
                }

                transactionResponse.CustomerFunds = logicAbstractApi.GetShop().GetCustomerFunds();

                string transactionMessage = serializer.Serialize(transactionResponse);
                Console.WriteLine($"Send: {transactionMessage}");
                await webSocketConnection.SendAsync(transactionMessage);
            }
            else if(serializer.GetHeader(message) == GetInstrumentsAndFundsCommand.StaticHeader)
            {
                Task task = Task.Run(async () => await SendItemsAndFunds());
            }
        }

        private async Task SendItemsAndFunds()
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Sending instruments and funds...");

            GetInstrumentsAndFundsResponse serverResponse = new GetInstrumentsAndFundsResponse();
            List<IInstrumentLogic> instruments = logicAbstractApi.GetShop().GetInstruments();

            float funds = logicAbstractApi.GetShop().GetCustomerFunds();
            serverResponse.Instruments = instruments.Select(x => x.ToDTO()).ToArray();
            serverResponse.Funds = funds;

            string responseJson = serializer.Serialize(serverResponse);

            await webSocketConnection.SendAsync(responseJson);
        }

        private async Task SendItems()
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"Sending instruments...");

            UpdateAllResponse serverResponse = new UpdateAllResponse();
            List<IInstrumentLogic> instruments = logicAbstractApi.GetShop().GetInstruments();
            serverResponse.Instruments = instruments.Select(x => x.ToDTO()).ToArray();

            string responseJson = serializer.Serialize(serverResponse);
            Console.WriteLine(responseJson);

            await webSocketConnection.SendAsync(responseJson);
        }

        private async void HandleInflationChanged(object? sender, InflationChangedEventArgsLogic args)
        {
            if (webSocketConnection == null)
                return;

            Console.WriteLine($"New inflation: {args.NewInflation}");

            List<IInstrumentLogic> instruments = logicAbstractApi.GetShop().GetInstruments();
            InflationChangedResponse inflationChangedResponse = new InflationChangedResponse();
            inflationChangedResponse.NewInflation = args.NewInflation;
            inflationChangedResponse.NewPrices = instruments.Select(x => new NewPriceDTO(x.Id, x.Price)).ToArray();

            string responseJson = serializer.Serialize(inflationChangedResponse);
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


        private static async Task Main(string[] args)
        {
            ServerPresentation presentation = new ServerPresentation(LogicAbstractApi.Create());
            await presentation.StartConnection();
        }
    }
}
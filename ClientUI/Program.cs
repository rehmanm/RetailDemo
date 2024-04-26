using NServiceBus.Logging;
using Sales.Messages;

namespace ClientUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");
            // Choose JSON to serialize and deserialize messages
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop();
        }

        #region RunLoop

        static ILog log = LogManager.GetLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {

            while (true)
            {

                log.Info("Press 'P' to place an order, or 'Q' to quit");

                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        var commmand = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        log.Info($"Sending PlaceOrder Command, OrderId = {commmand.OrderId}");
                        await endpointInstance.Send(commmand);
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown Input, Please try again");
                        break;
                }
            }

        }

        #endregion
    }
}

using NServiceBus.Logging;
using Sales.Messages;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static Random random = new Random();
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            var orderPlaced = new OrderPlaced { OrderId = message.OrderId };

            if (random.Next(0, 5) == 0)
            {
                throw new Exception("Oops");
            }

            return context.Publish(orderPlaced);
        }
    }
}

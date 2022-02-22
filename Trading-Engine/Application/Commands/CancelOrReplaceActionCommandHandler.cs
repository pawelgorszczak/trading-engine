using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class CancelOrReplaceActionCommandHandler : ICommandHandler<CancelOrReplaceActionCommand>
    {
        private readonly OrderBook _orderBook;

        public CancelOrReplaceActionCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(CancelOrReplaceActionCommand actionCommand)
        {
            var foundOrder = _orderBook.GetOrder(actionCommand.OrderId);

            if (foundOrder != null && !(foundOrder is IceBeargOrder))
            {
                bool shouldDecreasePriority =
                    !(actionCommand.Price == foundOrder.Price && actionCommand.Quantity <= foundOrder.Quantity);

                foundOrder.Price = actionCommand.Price;
                foundOrder.Quantity = actionCommand.Quantity;

                if (shouldDecreasePriority)
                {
                    // it could be done differently, maybe more clean since it is fragile, but when properly covered by unit tests it shoould be ok
                    foundOrder.CreateTime = DateTimeOffset.Now;
                }
            }

            return null;
        }

    }
}
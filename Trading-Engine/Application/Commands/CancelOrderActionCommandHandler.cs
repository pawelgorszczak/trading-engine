using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class CancelOrderActionCommandHandler : ICommandHandler<CancelOrderActionCommand>
    {
        private readonly OrderBook _orderBook;

        public CancelOrderActionCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(CancelOrderActionCommand actionCommand)
        {
            _orderBook.RemoveOrder(actionCommand.OrderId);

            return null;
        }
    }
}
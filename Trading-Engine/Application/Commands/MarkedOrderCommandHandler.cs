using Trading_Engine.Application;
using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class MarkedOrderCommandHandler : ICommandHandler<MarketOrderCommand>
    {
        private readonly OrderBook _orderBook;

        public MarkedOrderCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(MarketOrderCommand command)
        {
            var nextOrder = GetNextOrder(command.SideType);
            var orderLeftTohandle = command.Quantity;
            int operationCost = 0;

            while (nextOrder != null && orderLeftTohandle > 0)
            {
                if (nextOrder.Quantity > orderLeftTohandle)
                {
                    operationCost += nextOrder.Price * orderLeftTohandle;

                    nextOrder.Quantity -= orderLeftTohandle;
                    orderLeftTohandle = 0;
                }
                else
                {
                    operationCost += nextOrder.Price * nextOrder.Quantity;

                    orderLeftTohandle -= nextOrder.Quantity;
                    nextOrder.Quantity = 0;
                }

                if (nextOrder.Quantity == 0)
                {
                    _orderBook.RemoveOrder(nextOrder.OrderId);
                }

                nextOrder = GetNextOrder(command.SideType);
            }

            return operationCost.ToString();
        }

        private Order GetNextOrder(SideType sideType)
        {
            return sideType == SideType.Buy ? _orderBook.SellOrders.FirstOrDefault() : _orderBook.BuyOrders.FirstOrDefault();
        }
    }
}
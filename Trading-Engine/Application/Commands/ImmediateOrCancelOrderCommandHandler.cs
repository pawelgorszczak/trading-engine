using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class ImmediateOrCancelOrderCommandHandler : ICommandHandler<ImmediateOrCancelOrderCommand>
    {
        private readonly OrderBook _orderBook;

        public ImmediateOrCancelOrderCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(ImmediateOrCancelOrderCommand command)
        {
            var nextOrder = GetNextOrder(command.SideType, command.Price);
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

                nextOrder = GetNextOrder(command.SideType, command.Price);
            }

            return operationCost.ToString();
        }

        private Order GetNextOrder(SideType sideType, int pricePoint)
        {
            var nextOrder = sideType == SideType.Buy ? _orderBook.SellOrders.FirstOrDefault() : _orderBook.BuyOrders.FirstOrDefault();
            if (nextOrder == null) return null;

            if (sideType == SideType.Buy && nextOrder.Price <= pricePoint || sideType == SideType.Sell && nextOrder.Price >= pricePoint)
            {
                return nextOrder;
            }
            return null;
        }
    }
}
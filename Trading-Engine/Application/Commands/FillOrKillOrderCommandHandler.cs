using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class FillOrKillOrderCommandHandler : ICommandHandler<FillOrKillOrderCommand>
    {
        private readonly OrderBook _orderBook;

        public FillOrKillOrderCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(FillOrKillOrderCommand command)
        {
            var nextOrder = GetNextOrder(command.SideType, command.Price);
            var orderLeftTohandle = command.Quantity;
            int operationCost = 0;

            if (nextOrder != null && orderLeftTohandle <= nextOrder.Quantity)
            {
                operationCost += nextOrder.Price * nextOrder.Quantity;

                orderLeftTohandle -= nextOrder.Quantity;
                nextOrder.Quantity -= orderLeftTohandle;
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
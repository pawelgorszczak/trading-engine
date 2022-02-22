using Trading_Engine.Domain;

namespace Trading_Engine.Application.Commands
{
    public class LimitOrderCommandHandler : ICommandHandler<LimitOrderCommand>
    {
        private readonly OrderBook _orderBook;

        public LimitOrderCommandHandler(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public string Handle(LimitOrderCommand command)
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

            if (orderLeftTohandle > 0)
            {
                AddRemainingOrder(command, orderLeftTohandle);
            }

            return operationCost.ToString();
        }

        private void AddRemainingOrder(LimitOrderCommand command, int leftQuantity)
        {
            var order = new Order(command.OrderId, command.Price, leftQuantity);
            _orderBook.AddOrder(order, command.SideType);
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
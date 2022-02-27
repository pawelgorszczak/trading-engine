using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    public class OrderBook3 : IOrderBook
    {
        private LinkedList<Order> _buyOrders;
        private LinkedList<Order> _sellOrders;

        public IReadOnlyList<Order> BuyOrders => _buyOrders.ToList().AsReadOnly();

        public IReadOnlyList<Order> SellOrders => _sellOrders.ToList().AsReadOnly();

        public OrderBook3()
        {
            _buyOrders = new LinkedList<Order>();
            _sellOrders = new LinkedList<Order>();
        }

        public void AddOrder(Order order, SideType sideType)
        {
            if (sideType == SideType.Buy)
            {
                AddBuyOrder(order);
            }
            else
            {
                AddSellOrder(order);
            }

        }

        public void AddBuyOrder(Order order)
        {
            var placementFound = false;
            LinkedListNode<Order> current = _buyOrders.First;
            while (current != null && !placementFound)
            {
                if (order.Price > current.Value.Price || order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime)
                {
                    _buyOrders.AddBefore(current, order);
                    placementFound = true;
                }
                else
                {
                    current = current.Next;
                }
            }

            if (current == null && !placementFound)
            {
                _buyOrders.AddLast(order);
            }
        }

        public void AddSellOrder(Order order)
        {
            var placementFound = false;
            LinkedListNode<Order> current = _sellOrders.First;
            while (current != null && !placementFound)
            {
                if (order.Price < current.Value.Price || order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime)
                {
                    _sellOrders.AddBefore(current, order);
                    placementFound = true;
                }
                else
                {
                    current = current.Next;
                }
            }

            if (current == null && !placementFound)
            {
                _sellOrders.AddLast(order);
            }
        }

        public void RemoveOrder(string commandOrderId)
        {
            if (!TryRemoveFromList(commandOrderId, _buyOrders))
            {
                TryRemoveFromList(commandOrderId, _sellOrders);
            }
        }

        public static bool TryRemoveFromList(string commandOrderId, LinkedList<Order> orders)
        {
            var foundOrder = orders.FirstOrDefault(order => order.OrderId == commandOrderId);
            if (foundOrder != null)
            {
                orders.Remove(foundOrder);
                if (foundOrder is IceBeargOrder)
                {
                    //var resubmition = new IceBeargOrder(foundOrder.OrderId, foundOrder.Price, );
                }
                return true;
            }

            return false;
        }

        public Order GetOrder(string orderId)
        {
            var foundOrder = _buyOrders.FirstOrDefault(order => order.OrderId == orderId);

            if (foundOrder == null)
            {
                foundOrder = _sellOrders.FirstOrDefault(order => order.OrderId == orderId);
            }

            return foundOrder;
        }
    }
}
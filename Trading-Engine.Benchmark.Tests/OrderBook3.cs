namespace Trading_Engine.Domain
{
    public class OrderBook3
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
                if (order.Price > current.Value.Price || (order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime))
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
                this._buyOrders.AddLast(order);
            }
        }
                
        public void AddSellOrder(Order order)
        {
            var placementFound = false;
            LinkedListNode<Order> current = _sellOrders.First;
            while (current != null && !placementFound)
            {
                if (order.Price < current.Value.Price || (order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime))
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
                this._sellOrders.AddLast(order);
            }
        }

        //private void SortSellOrders() => _sellOrders = _sellOrders.OrderBy(o => o.Price).ThenBy(o => o.CreateTime).ToList();

        //private void SortBuyOrders() => _buyOrders = _buyOrders.OrderByDescending(o => o.Price).ThenBy(o => o.CreateTime).ToList();

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
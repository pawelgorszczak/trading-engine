namespace Trading_Engine.Domain
{
    public class OrderBook
    {
        private List<Order> _buyOrders;
        private List<Order> _sellOrders;

        public IReadOnlyList<Order> BuyOrders => _buyOrders.AsReadOnly();

        public IReadOnlyList<Order> SellOrders => _sellOrders.AsReadOnly();

        public OrderBook()
        {
            _buyOrders = new List<Order>();
            _sellOrders = new List<Order>();
        }

        public void AddOrder(Order order, SideType sideType)
        {
            if (sideType == SideType.Buy)
            {
                _buyOrders.Add(order);
                SortBuyOrders();
            }
            else
            {
                _sellOrders.Add(order);
                SortSellOrders();
            }

        }

        private void SortSellOrders() => _sellOrders = _sellOrders.OrderBy(o => o.Price).ThenBy(o => o.CreateTime).ToList();

        private void SortBuyOrders() => _buyOrders = _buyOrders.OrderByDescending(o => o.Price).ThenBy(o => o.CreateTime).ToList();

        public void RemoveOrder(string commandOrderId)
        {
            if (!TryRemoveFromList(commandOrderId, _buyOrders))
            {
                TryRemoveFromList(commandOrderId, _sellOrders);
            }
        }

        public static bool TryRemoveFromList(string commandOrderId, List<Order> orders)
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
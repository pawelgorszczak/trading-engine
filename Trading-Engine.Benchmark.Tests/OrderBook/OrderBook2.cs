using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    public class OrderBook2 : IOrderBookTest
    {
        private List<Order> _buyOrders;
        private List<Order> _sellOrders;

        public IReadOnlyList<Order> BuyOrders => _buyOrders.AsReadOnly();

        public IReadOnlyList<Order> SellOrders => _sellOrders.AsReadOnly();

        public OrderBook2()
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


        //private void SortSellOrders() => _sellOrders = _sellOrders.OrderBy(o => o.Price).ThenBy(o => o.CreateTime).ToList();

        //private void SortBuyOrders() => _buyOrders = _buyOrders.OrderByDescending(o => o.Price).ThenBy(o => o.CreateTime).ToList();

        private void SortSellOrders()
        {
            Comparison<Order> comparer = (x, y) => { return x.Price - y.Price + x.CreateTime.Millisecond - y.CreateTime.Millisecond; };
            _sellOrders.Sort(comparer);
        }

        private void SortBuyOrders()
        {
            Comparison<Order> comparer = (x, y) => { return y.Price - x.Price + x.CreateTime.Millisecond - y.CreateTime.Millisecond; };
            _buyOrders.Sort(comparer);
        }

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

        public void InidializeBuyOrders(List<Order> orders)
        {
            _buyOrders = new List<Order>(orders);
        }

        public void InidializeSellOrders(List<Order> orders)
        {
            _sellOrders = new List<Order>(orders);
        }

        public override string? ToString()
        {
            return "OrderBook2";
        }
    }
}
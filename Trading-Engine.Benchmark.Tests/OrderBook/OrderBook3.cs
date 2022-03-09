using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    public class OrderBook3 : IOrderBookTest
    {
        private LinkedList<Order> _buyOrders;
        private LinkedList<Order> _sellOrders;

        private Dictionary<string, LinkedListNode<Order>> _buyOrdersLookUp;
        private Dictionary<string, LinkedListNode<Order>> _sellOrdersLookUp;

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
            LinkedListNode<Order>? addedElement = null;
            while (current != null && !placementFound)
            {
                if (order.Price > current.Value.Price || order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime)
                {
                    addedElement = _buyOrders.AddBefore(current, order);
                    placementFound = true;
                }
                else
                {
                    current = current.Next;
                }
            }

            if (current == null && !placementFound)
            {
                addedElement = _buyOrders.AddLast(order);
            }

            _buyOrdersLookUp.Add(addedElement.Value.OrderId, addedElement);
        }

        public void AddSellOrder(Order order)
        {
            var placementFound = false;
            LinkedListNode<Order> current = _sellOrders.First;
            LinkedListNode<Order>? addedElement = null;
            while (current != null && !placementFound)
            {
                if (order.Price < current.Value.Price || order.Price == current.Value.Price && order.CreateTime < current.Value.CreateTime)
                {
                    addedElement = _sellOrders.AddBefore(current, order);
                    placementFound = true;
                }
                else
                {
                    current = current.Next;
                }
            }

            if (current == null && !placementFound)
            {
                addedElement = _sellOrders.AddLast(order);
            }

            _buyOrdersLookUp.Add(addedElement.Value.OrderId, addedElement);
        }

        public void RemoveOrder(string commandOrderId)
        {
            if (!TryRemoveFromList(commandOrderId,_buyOrdersLookUp, _buyOrders))
            {
                TryRemoveFromList(commandOrderId, _sellOrdersLookUp, _sellOrders);
            }
        }

        public static bool TryRemoveFromList(string commandOrderId, Dictionary<string, LinkedListNode<Order>> lookupTable, LinkedList<Order> orders)
        {
            var foundOrder = lookupTable.GetValueOrDefault(commandOrderId);
            if (foundOrder != null)
            {
                orders.Remove(foundOrder);
                lookupTable.Remove(commandOrderId);
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
            var foundOrder = _buyOrdersLookUp.GetValueOrDefault(orderId)?.Value;

            if (foundOrder == null)
            {
                foundOrder = _sellOrdersLookUp.GetValueOrDefault(orderId)?.Value;
            }

            return foundOrder;
        }

        public void InidializeBuyOrders(List<Order> orders)
        {
            _buyOrders = new LinkedList<Order>(orders);
            _buyOrdersLookUp = new Dictionary<string, LinkedListNode<Order>>();
        }

        public void InidializeSellOrders(List<Order> orders)
        {
            _sellOrders = new LinkedList<Order>(orders);
            _sellOrdersLookUp = new Dictionary<string, LinkedListNode<Order>>();
        }

        public override string? ToString()
        {
            return "OrderBook3";
        }
    }
}
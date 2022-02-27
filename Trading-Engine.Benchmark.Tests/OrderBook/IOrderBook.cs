using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    public interface IOrderBook
    {
        IReadOnlyList<Order> BuyOrders { get; }
        IReadOnlyList<Order> SellOrders { get; }

        void AddOrder(Order order, SideType sideType);
        Order GetOrder(string orderId);
        void RemoveOrder(string commandOrderId);
    }
}
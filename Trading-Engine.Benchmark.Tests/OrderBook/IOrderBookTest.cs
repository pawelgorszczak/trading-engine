using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    public interface IOrderBookTest
    {
        IReadOnlyList<Order> BuyOrders { get; }
        IReadOnlyList<Order> SellOrders { get; }
        void InidializeBuyOrders(List<Order> orders);
        void InidializeSellOrders(List<Order> orders);
        void AddOrder(Order order, SideType sideType);
        Order GetOrder(string orderId);
        void RemoveOrder(string commandOrderId);
    }
}
using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests
{    
    [SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 5)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OrderBookTests
    {
        private OrderBook orderBook = new OrderBook();

        /*
            |             Method |     Mean |    Error |   StdDev |      Min |      Max |   Median |
            |------------------- |---------:|---------:|---------:|---------:|---------:|---------:|
            | AddBuyAndSellOrder | 4.799 ms | 4.923 ms | 1.279 ms | 3.085 ms | 6.164 ms | 4.572 ms |
        */
        [Benchmark]
        public void AddBuyAndSellOrder()
        {
            var buyOrder = new Order("YuFU", 13, 100);
            var sellOrder = new Order("IpD8", 14, 150);

            orderBook.AddOrder(buyOrder, SideType.Buy);
            orderBook.AddOrder(sellOrder, SideType.Sell);
        }

    }
}

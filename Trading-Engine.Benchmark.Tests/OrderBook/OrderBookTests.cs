using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    //https://arthurminduca.com/2016/04/25/choosing-the-right-collection/
    [SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 5)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OrderBookTests
    {
        private OrderBook1 orderBook = new OrderBook1();
        private OrderBook2 orderBook2 = new OrderBook2();
        private OrderBook3 orderBook3 = new OrderBook3();

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

        /*
             |              Method |     Mean |    Error |    StdDev |      Min |      Max |   Median |
             |-------------------- |---------:|---------:|----------:|---------:|---------:|---------:|
             |  AddBuyAndSellOrder | 4.519 ms | 4.514 ms | 1.1723 ms | 3.036 ms | 6.048 ms | 4.539 ms |
             | AddBuyAndSellOrder2 | 2.534 ms | 2.664 ms | 0.6919 ms | 1.626 ms | 3.295 ms | 2.573 ms |
         */
        [Benchmark]
        public void AddBuyAndSellOrder2()
        {
            var buyOrder = new Order("YuFU", 13, 100);
            var sellOrder = new Order("IpD8", 14, 150);

            orderBook2.AddOrder(buyOrder, SideType.Buy);
            orderBook2.AddOrder(sellOrder, SideType.Sell);
        }

        /*
             |              Method |     Mean |    Error |    StdDev |      Min |      Max |   Median |
             |-------------------- |---------:|---------:|----------:|---------:|---------:|---------:|
             |  AddBuyAndSellOrder | 4.718 ms | 4.467 ms | 1.1600 ms | 3.135 ms | 6.153 ms | 4.750 ms |
             | AddBuyAndSellOrder2 | 5.339 ms | 5.872 ms | 1.5248 ms | 3.484 ms | 7.386 ms | 5.223 ms |
             | AddBuyAndSellOrder3 | 1.748 ms | 2.021 ms | 0.5249 ms | 1.079 ms | 2.408 ms | 1.736 ms |
         */
        [Benchmark]
        public void AddBuyAndSellOrder3()
        {
            var buyOrder = new Order("YuFU", 13, 100);
            var sellOrder = new Order("IpD8", 14, 150);

            orderBook3.AddOrder(buyOrder, SideType.Buy);
            orderBook3.AddOrder(sellOrder, SideType.Sell);
        }
    }
}

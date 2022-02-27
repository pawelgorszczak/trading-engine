using BenchmarkDotNet.Attributes;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    //https://arthurminduca.com/2016/04/25/choosing-the-right-collection/
    [SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 5)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OrderBookTests
    {
        [Benchmark]
        public void AddBuyAndSellOrder1()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook1());
        }

        [Benchmark]
        public void AddBuyAndSellOrder2()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyAndSellOrder3()
        {
            AddBuyAndSellOrderSimpleScenario(new OrderBook3());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder1()
        {
            AddBuyOrderComplexScenrio(new OrderBook1());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder2()
        {
            AddBuyOrderComplexScenrio(new OrderBook2());
        }

        [Benchmark]
        public void AddBuyOrderComplexOrder3()
        {
            AddBuyOrderComplexScenrio(new OrderBook3());
        }

        /*
            |              Method |     Mean |    Error |   StdDev |      Min |      Max |   Median |
            |-------------------- |---------:|---------:|---------:|---------:|---------:|---------:|
            | AddBuyAndSellOrder1 | 757.5 ns | 43.87 ns | 11.39 ns | 749.0 ns | 775.8 ns | 751.3 ns |
            | AddBuyAndSellOrder2 | 368.4 ns |  8.48 ns |  2.20 ns | 365.2 ns | 370.4 ns | 368.4 ns |
            | AddBuyAndSellOrder3 | 363.6 ns |  5.27 ns |  1.37 ns | 362.2 ns | 365.6 ns | 363.3 ns |
         */
        private void AddBuyAndSellOrderSimpleScenario(IOrderBook orderBook)
        {
            var buyOrder = new Order("YuFU", 13, 100);
            var sellOrder = new Order("IpD8", 14, 150);

            orderBook.AddOrder(buyOrder, SideType.Buy);
            orderBook.AddOrder(sellOrder, SideType.Sell);
        }

        /*
            |                   Method |      Mean |     Error |    StdDev |       Min |       Max |    Median |
            |------------------------- |----------:|----------:|----------:|----------:|----------:|----------:|
            | AddBuyOrderComplexOrder1 | 10.677 us | 0.3275 us | 0.0507 us | 10.641 us | 10.751 us | 10.658 us |
            | AddBuyOrderComplexOrder2 |  6.974 us | 0.2668 us | 0.0693 us |  6.922 us |  7.080 us |  6.930 us |
            | AddBuyOrderComplexOrder3 |  3.185 us | 0.0467 us | 0.0121 us |  3.171 us |  3.198 us |  3.187 us |
         */
        private void AddBuyOrderComplexScenrio(IOrderBook orderBook)
        {
            // having buy store: 112, 80, 79, 50, 50, 50, 40, 20, 15, 10
            orderBook.AddOrder(new Order("YuFU", 112, 20), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 80, 30), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 79, 40), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 50, 50), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 50, 60), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 50, 70), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 40, 80), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 20, 90), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 15, 90), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 10, 70), SideType.Buy);

            // add 200, 190, 80, 10, 14, 50, 45
            orderBook.AddOrder(new Order("YuFU", 200, 20), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 190, 30), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 80, 40), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 10, 50), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 14, 40), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 50, 30), SideType.Buy);
            orderBook.AddOrder(new Order("YuFU", 45, 20), SideType.Buy);
        }
    }
}

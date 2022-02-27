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
    }
}

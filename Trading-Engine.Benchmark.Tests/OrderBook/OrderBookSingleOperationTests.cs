using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    /*
        |                         Method |               Categories | OrderBookParam |           Mean |          Error |         StdDev |         Median |            Min |            Max |
        |------------------------------- |------------------------- |--------------- |---------------:|---------------:|---------------:|---------------:|---------------:|---------------:|
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook1 | 245,234.160 us |  7,819.4040 us |  5,172.0516 us | 243,226.950 us | 240,235.700 us | 256,763.500 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook2 | 516,654.980 us |  2,909.8379 us |  1,924.6776 us | 516,539.950 us | 514,020.800 us | 520,186.400 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook3 |      14.750 us |     10.4262 us |      6.8963 us |      11.000 us |       9.800 us |      27.500 us |
        |                                |                          |                |                |                |                |                |                |                |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook1 | 162,152.050 us | 13,325.0776 us |  8,813.7138 us | 159,789.250 us | 152,789.750 us | 179,338.050 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook2 | 267,488.410 us |  2,015.1410 us |  1,332.8910 us | 267,290.650 us | 265,640.300 us | 270,163.900 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook3 |   9,959.710 us |    266.2954 us |    176.1379 us |   9,933.050 us |   9,672.650 us |  10,231.450 us |
        |                                |                          |                |                |                |                |                |                |                |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook1 | 242,672.580 us |  5,612.7712 us |  3,712.5006 us | 242,636.650 us | 238,365.800 us | 250,245.500 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook2 | 557,304.794 us | 41,778.9873 us | 24,862.0052 us | 556,528.650 us | 530,626.450 us | 609,284.450 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook3 |      10.463 us |      1.2003 us |      0.6278 us |      10.550 us |       9.400 us |      11.400 us |
        |                                |                          |                |                |                |                |                |                |                |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook1 | 243,091.240 us |  5,035.2444 us |  3,330.5024 us | 243,159.950 us | 239,126.400 us | 246,605.200 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook2 | 555,619.260 us | 19,457.1414 us | 12,869.6943 us | 553,921.150 us | 536,236.050 us | 577,760.550 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook3 |      10.606 us |      0.9766 us |      0.5812 us |      10.750 us |       9.550 us |      11.450 us |
        |                                |                          |                |                |                |                |                |                |                |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |     882.070 us |     32.2137 us |     21.3074 us |     888.350 us |     842.800 us |     907.900 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |  17,463.200 us |  2,586.4484 us |  1,539.1539 us |  16,816.100 us |  15,775.800 us |  20,208.000 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |     883.130 us |     43.6575 us |     28.8767 us |     880.400 us |     820.450 us |     926.950 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |  15,545.400 us |    243.9011 us |    127.5650 us |  15,547.200 us |  15,294.000 us |  15,678.500 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |       6.922 us |      1.1696 us |      0.6960 us |       6.800 us |       6.300 us |       8.500 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |       2.910 us |      0.5766 us |      0.3814 us |       2.900 us |       2.400 us |       3.500 us |
        |                                |                          |                |                |                |                |                |                |                |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  26,219.494 us |    734.1472 us |    436.8792 us |  26,068.350 us |  25,734.950 us |  27,019.450 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  42,043.275 us |  3,823.7950 us |  1,999.9187 us |  41,472.000 us |  40,299.100 us |  46,785.800 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  25,966.044 us |    894.9296 us |    532.5582 us |  25,934.600 us |  25,326.800 us |  26,676.300 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  41,682.644 us |  1,429.6696 us |    850.7734 us |  41,355.100 us |  40,919.200 us |  43,598.100 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |       3.350 us |      0.6774 us |      0.4031 us |       3.350 us |       2.650 us |       4.050 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |       6.950 us |      1.2049 us |      0.6302 us |       6.900 us |       6.100 us |       8.100 us |
        |                                |                          |                |                |                |                |                |                |                |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  14,949.712 us |    436.8193 us |    228.4650 us |  14,922.950 us |  14,646.800 us |  15,250.800 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  28,518.850 us |    858.3602 us |    510.7964 us |  28,517.250 us |  27,873.950 us |  29,413.850 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  14,833.610 us |  1,041.8345 us |    689.1090 us |  14,504.400 us |  14,150.500 us |  16,153.700 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  29,827.520 us |  2,286.4743 us |  1,512.3612 us |  29,248.650 us |  27,981.400 us |  32,655.100 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |       6.770 us |      0.9149 us |      0.6052 us |       7.050 us |       5.850 us |       7.550 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |       7.033 us |      0.8232 us |      0.4899 us |       7.200 us |       6.500 us |       7.900 us |
        |                                |                          |                |                |                |                |                |                |                |
        |            GetOrder_FromTheEnd |             GetLastOrder |     OrderBook1 |     893.556 us |     30.4464 us |     18.1182 us |     887.500 us |     871.400 us |     928.400 us |
        |            GetOrder_FromTheEnd |             GetLastOrder |     OrderBook2 |     897.120 us |     39.6366 us |     26.2172 us |     896.850 us |     846.300 us |     945.600 us |
        |            GetOrder_FromTheEnd |             GetLastOrder |     OrderBook3 |       7.080 us |      1.3779 us |      0.9114 us |       6.900 us |       5.500 us |       8.800 us |
        |                                |                          |                |                |                |                |                |                |                |
        |        GetSellOrder_FromTheEnd |         GetLastSellOrder |     OrderBook1 |  45,371.060 us |  5,678.6870 us |  3,756.0998 us |  44,030.300 us |  41,979.800 us |  53,285.800 us |
        |        GetSellOrder_FromTheEnd |         GetLastSellOrder |     OrderBook2 |  41,252.088 us |    780.6247 us |    408.2818 us |  41,117.350 us |  40,761.000 us |  41,871.200 us |
        |        GetSellOrder_FromTheEnd |         GetLastSellOrder |     OrderBook3 |       7.089 us |      1.6172 us |      0.9623 us |       6.700 us |       6.200 us |       9.200 us |
     */
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class OrderBookSingleOperationTests
    {
        // prepared dataset
        private List<Order> buyOrderList;

        // prepared dataset
        private List<Order> sellOrderList;

        private int ordersCount = 1000000;

        // max buy price
        private int maxBuyPrice;

        // max sell price
        private int maxSellPrice;

        [ParamsSource(nameof(OrderBooks))]
        public IOrderBookTest OrderBookParam { get; set; }

        // public property
        public IEnumerable<IOrderBookTest> OrderBooks => new IOrderBookTest[] { new OrderBook1(), new OrderBook2(), new OrderBook3() };

        public OrderBookSingleOperationTests()
        {
            buyOrderList = new List<Order>();
            maxBuyPrice = 0;
            for (int i = 0; i < ordersCount; i++)
            {
                maxBuyPrice += 2;
                buyOrderList.Add(new Order("Buy_OrderId" + i, maxBuyPrice, 20));
            }
            buyOrderList.Reverse();


            sellOrderList = new List<Order>();
            maxSellPrice = 0;
            for (int i = 0; i < ordersCount; i++)
            {
                maxBuyPrice += 2;
                sellOrderList.Add(new Order("Sell_OrderId" + i, maxBuyPrice, 20));
            }
        }

        [IterationSetup]
        public void Setup()
        {
            PrepareDataSet(OrderBookParam);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheBeginning), Benchmark]
        public void AddOrder_AtTheBeginning()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", maxBuyPrice + 2, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheEnd), Benchmark]
        public void AddOrder_AtTheEnd()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", 1, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheMiddle), Benchmark]
        public void AddOrder_InTheMiddle()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", (maxBuyPrice / 2) + 1, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.AddInTheMiddleDuplicated), Benchmark]
        public void AddOrder_InTheMiddleDuplicated()
        {
            OrderBookParam.AddOrder(new Order("Buy_OrderId", maxBuyPrice / 2, 20), SideType.Buy);
        }

        [BenchmarkCategory(SingleOperationType.RemoveAtTheBeginning), Benchmark]
        public void RemoveOrder_AtTheBeginning()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (ordersCount - 1));
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheEnd), Benchmark]
        public void RemoveOrder_AtTheEnd()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId0");
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheMiddle), Benchmark]
        public void RemoveOrder_InTheMiddle()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (ordersCount / 2));
        }

        [BenchmarkCategory(SingleOperationType.RemoveAtTheBeginning), Benchmark]
        public void RemoveSellOrder_AtTheBeginning()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId0");
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheEnd), Benchmark]
        public void RemoveSellOrder_AtTheEnd()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId" + (ordersCount - 1));
        }

        [BenchmarkCategory(SingleOperationType.RemoveInTheMiddle), Benchmark]
        public void RemoveSellOrder_InTheMiddle()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId" + (ordersCount / 2));
        }

        [BenchmarkCategory(SingleOperationType.GetLastOrder), Benchmark]
        public void GetOrder_FromTheEnd()
        {
            OrderBookParam.RemoveOrder("Buy_OrderId" + (ordersCount - 1));
        }

        [BenchmarkCategory(SingleOperationType.GetLastSellOrder), Benchmark]
        public void GetSellOrder_FromTheEnd()
        {
            OrderBookParam.RemoveOrder("Sell_OrderId" + (ordersCount - 1));
        }

        private void PrepareDataSet(IOrderBookTest orderBook)
        {
            orderBook.InidializeBuyOrders(buyOrderList);
            orderBook.InidializeSellOrders(sellOrderList);
        }
    }

    public static class SingleOperationType
    {
        public const string AddInTheBeginning = "AddInTheBeginning";
        public const string AddInTheMiddle = "AddInTheMiddle";
        public const string AddInTheMiddleDuplicated = "AddInTheMiddleDuplicated";
        public const string AddInTheEnd = "AddInTheEnd";
        public const string RemoveAtTheBeginning = "RemoveInTheBeginning";
        public const string RemoveInTheMiddle = "RemoveInTheMiddle";
        public const string RemoveInTheEnd = "RemoveInTheEnd";
        public const string GetLastOrder = "GetLastOrder";
        public const string GetLastSellOrder = "GetLastSellOrder";
    }
}

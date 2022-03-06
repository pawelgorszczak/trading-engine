using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Trading_Engine.Domain;

namespace Trading_Engine.Benchmark.Tests.OrderBook
{
    /*
        |                         Method |               Categories | OrderBookParam |          Mean |         Error |        StdDev |        Median |            Min |           Max |
        |------------------------------- |------------------------- |--------------- |--------------:|--------------:|--------------:|--------------:|---------------:|--------------:|
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook1 | 243,129.01 us |  2,982.903 us |  1,973.006 us | 242,593.45 us | 240,385.500 us | 247,041.80 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook2 | 530,376.25 us | 23,431.461 us | 15,498.460 us | 523,337.80 us | 513,079.000 us | 557,044.80 us |
        |        AddOrder_AtTheBeginning |        AddInTheBeginning |     OrderBook3 |      16.35 us |      6.235 us |      4.124 us |      18.85 us |       8.000 us |      19.70 us |
        |                                |                          |                |               |               |               |               |                |               |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook1 | 149,373.03 us |  2,831.958 us |  1,873.165 us | 149,502.60 us | 146,422.950 us | 152,653.75 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook2 | 276,462.77 us |  2,717.938 us |  1,617.401 us | 277,050.30 us | 273,235.600 us | 277,914.30 us |
        |              AddOrder_AtTheEnd |              AddInTheEnd |     OrderBook3 |  11,009.84 us |  1,580.900 us |    940.768 us |  10,522.20 us |  10,229.000 us |  12,926.60 us |
        |                                |                          |                |               |               |               |               |                |               |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook1 | 239,511.51 us |  3,849.461 us |  2,546.180 us | 240,148.95 us | 235,279.600 us | 243,717.70 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook2 | 518,189.32 us |  7,659.050 us |  4,557.778 us | 518,406.30 us | 513,278.000 us | 523,674.30 us |
        |           AddOrder_InTheMiddle |           AddInTheMiddle |     OrderBook3 |      13.65 us |      8.691 us |      5.748 us |      10.20 us |       8.600 us |      22.50 us |
        |                                |                          |                |               |               |               |               |                |               |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook1 | 231,348.11 us |    924.018 us |    483.279 us | 231,323.40 us | 230,655.700 us | 232,015.60 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook2 | 516,074.04 us |  4,570.235 us |  3,022.927 us | 515,154.15 us | 511,990.200 us | 521,017.30 us |
        | AddOrder_InTheMiddleDuplicated | AddInTheMiddleDuplicated |     OrderBook3 |      17.36 us |      8.594 us |      5.685 us |      19.35 us |       9.600 us |      26.90 us |
        |                                |                          |                |               |               |               |               |                |               |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |     901.43 us |     27.205 us |     16.190 us |     905.10 us |     864.900 us |     922.20 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook1 |  15,977.07 us |    534.425 us |    318.027 us |  15,824.00 us |  15,710.600 us |  16,649.70 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |     906.41 us |     36.014 us |     21.431 us |     909.20 us |     870.900 us |     946.80 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook2 |  16,271.67 us |    471.760 us |    280.737 us |  16,195.25 us |  15,985.450 us |  16,884.45 us |
        |     RemoveOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |      15.60 us |      1.401 us |      0.834 us |      15.50 us |      14.200 us |      16.80 us |
        | RemoveSellOrder_AtTheBeginning |     RemoveInTheBeginning |     OrderBook3 |  16,521.19 us |  3,062.261 us |  1,822.302 us |  15,540.45 us |  15,329.350 us |  19,962.35 us |
        |                                |                          |                |               |               |               |               |                |               |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  27,269.85 us |    677.000 us |    447.794 us |  27,206.15 us |  26,809.500 us |  28,083.60 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook1 |  43,462.02 us |  1,428.382 us |    850.007 us |  43,208.00 us |  42,678.300 us |  45,194.70 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  26,544.75 us |    682.631 us |    451.518 us |  26,434.15 us |  26,026.100 us |  27,341.50 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook2 |  44,524.79 us |  3,489.815 us |  2,076.733 us |  44,089.80 us |  42,661.500 us |  49,097.40 us |
        |           RemoveOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |  27,932.17 us |  3,048.594 us |  1,814.169 us |  26,908.40 us |  26,221.000 us |  30,975.10 us |
        |       RemoveSellOrder_AtTheEnd |           RemoveInTheEnd |     OrderBook3 |  43,264.19 us |    677.797 us |    354.501 us |  43,340.95 us |  42,813.900 us |  43,831.10 us |
        |                                |                          |                |               |               |               |               |                |               |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  15,201.62 us |  1,099.496 us |    654.293 us |  14,894.40 us |  14,739.600 us |  16,718.20 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook1 |  29,872.91 us |    265.172 us |    157.799 us |  29,843.10 us |  29,655.200 us |  30,179.30 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  15,004.24 us |    332.086 us |    197.619 us |  14,935.50 us |  14,818.600 us |  15,439.40 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook2 |  30,655.50 us |    542.822 us |    323.025 us |  30,572.40 us |  30,283.700 us |  31,290.00 us |
        |        RemoveOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |  15,147.18 us |    824.893 us |    490.881 us |  15,185.50 us |  14,486.600 us |  16,112.10 us |
        |    RemoveSellOrder_InTheMiddle |        RemoveInTheMiddle |     OrderBook3 |  29,414.36 us |    788.383 us |    521.467 us |  29,268.25 us |  28,910.450 us |  30,309.45 us |
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
    }
}

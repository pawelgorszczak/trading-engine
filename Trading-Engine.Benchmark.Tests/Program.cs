using BenchmarkDotNet.Running;
using Trading_Engine.Benchmark.Tests.OrderBook;

//new OrderBookTests().AddBuyOrders3IntoBigList();

//BenchmarkRunner.Run<OrderBookTests>();
BenchmarkRunner.Run<OrderBookSingleOperationTests>();
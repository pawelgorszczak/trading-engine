using BenchmarkDotNet.Running;
using Trading_Engine.Benchmark.Tests;

//new OrderBookTests().AddBuyAndSellOrder3();

BenchmarkRunner.Run<OrderBookTests>();
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks;

_ = BenchmarkRunner.Run<MergeSortBenchmarks>();

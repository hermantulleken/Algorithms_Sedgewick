using BenchmarkDotNet.Running;
using Benchmarks;

//_ = BenchmarkRunner.Run<LinkedListBenchMarks>(new DebugInProcessConfig());
_ = BenchmarkRunner.Run<MergeSortBenchmarks>();

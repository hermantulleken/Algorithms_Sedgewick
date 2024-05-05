#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<RadixSortBenchmarks>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<RadixSortBenchmarks>();
#endif

#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<BellmanFordBenchmarks>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<BellmanFordBenchmarks>();
#endif

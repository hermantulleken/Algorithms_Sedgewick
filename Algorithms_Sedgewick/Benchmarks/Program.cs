#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<CriticalEdgesBenchmarks>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<CriticalEdgesBenchmarks>();
#endif

#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<UniqueRandomInt>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<UniqueRandomInt>();
#endif

#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<HeapBasedMstAlgorithms>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<HeapBasedMstAlgorithms>();
#endif

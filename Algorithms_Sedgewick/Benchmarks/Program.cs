#if DEBUG
using BenchmarkDotNet.Configs;
#endif

using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG
_ = BenchmarkRunner.Run<Sort3Elements>(new DebugInProcessConfig());
#else
_ = BenchmarkRunner.Run<Sort3Elements>();
#endif

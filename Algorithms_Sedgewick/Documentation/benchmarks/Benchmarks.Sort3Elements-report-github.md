```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.3570/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2


```
| Method        | ItemCount | Mean     | Error    | StdDev   |
|-------------- |---------- |---------:|---------:|---------:|
| **QuickSort**     | **500000**    | **21.58 ms** | **0.148 ms** | **0.131 ms** |
| DutchFlagSort | 500000    | 13.20 ms | 0.264 ms | 0.508 ms |
| ShellShort    | 500000    | 42.88 ms | 0.600 ms | 0.561 ms |
| **QuickSort**     | **1000000**   | **49.62 ms** | **0.908 ms** | **0.849 ms** |
| DutchFlagSort | 1000000   | 28.50 ms | 0.531 ms | 0.522 ms |
| ShellShort    | 1000000   | 81.67 ms | 1.073 ms | 0.896 ms |

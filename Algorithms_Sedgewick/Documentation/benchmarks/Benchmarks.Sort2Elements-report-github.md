```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.3570/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2


```
| Method     | ItemCount | Mean     | Error     | StdDev    |
|----------- |---------- |---------:|----------:|----------:|
| **QuickSort**  | **50000**     | **1.964 ms** | **0.0112 ms** | **0.0104 ms** |
| Sort2      | 50000     | 1.160 ms | 0.0135 ms | 0.0120 ms |
| ShellShort | 50000     | 3.295 ms | 0.0651 ms | 0.0846 ms |
| **QuickSort**  | **100000**    | **4.008 ms** | **0.0305 ms** | **0.0255 ms** |
| Sort2      | 100000    | 2.304 ms | 0.0283 ms | 0.0236 ms |
| ShellShort | 100000    | 6.251 ms | 0.0430 ms | 0.0402 ms |

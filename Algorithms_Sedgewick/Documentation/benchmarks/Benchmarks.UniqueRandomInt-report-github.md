```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.3803/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                  | MaxValue | Fraction | Mean         | Error      | StdDev     |
|---------------------------------------- |--------- |--------- |-------------:|-----------:|-----------:|
| **UniqueUniformRandomInt_WithShuffledList** | **10**       | **0.05**     |     **28.41 ns** |   **0.388 ns** |   **0.363 ns** |
| UniqueUniformRandomInt_WithSet          | 10       | 0.05     |           NA |         NA |         NA |
| **UniqueUniformRandomInt_WithShuffledList** | **10**       | **0.1**      |     **31.66 ns** |   **0.396 ns** |   **0.351 ns** |
| UniqueUniformRandomInt_WithSet          | 10       | 0.1      |    190.56 ns |   0.845 ns |   0.790 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10**       | **0.15**     |     **31.05 ns** |   **0.374 ns** |   **0.331 ns** |
| UniqueUniformRandomInt_WithSet          | 10       | 0.15     |    186.51 ns |   1.208 ns |   1.130 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10**       | **0.2**      |     **34.32 ns** |   **0.240 ns** |   **0.225 ns** |
| UniqueUniformRandomInt_WithSet          | 10       | 0.2      |    224.45 ns |   0.603 ns |   0.471 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **100**      | **0.05**     |    **141.35 ns** |   **1.786 ns** |   **1.671 ns** |
| UniqueUniformRandomInt_WithSet          | 100      | 0.05     |    304.81 ns |   2.512 ns |   2.350 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **100**      | **0.1**      |    **154.93 ns** |   **2.062 ns** |   **1.928 ns** |
| UniqueUniformRandomInt_WithSet          | 100      | 0.1      |    448.16 ns |   1.711 ns |   1.429 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **100**      | **0.15**     |    **184.91 ns** |   **1.174 ns** |   **1.040 ns** |
| UniqueUniformRandomInt_WithSet          | 100      | 0.15     |    587.76 ns |   3.619 ns |   3.385 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **100**      | **0.2**      |    **212.57 ns** |   **1.430 ns** |   **1.338 ns** |
| UniqueUniformRandomInt_WithSet          | 100      | 0.2      |    798.95 ns |   4.138 ns |   3.871 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **1000**     | **0.05**     |  **1,129.70 ns** |  **13.116 ns** |  **12.269 ns** |
| UniqueUniformRandomInt_WithSet          | 1000     | 0.05     |  1,761.12 ns |   6.237 ns |   5.208 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **1000**     | **0.1**      |  **1,289.07 ns** |  **16.583 ns** |  **15.511 ns** |
| UniqueUniformRandomInt_WithSet          | 1000     | 0.1      |  3,334.79 ns |   9.541 ns |   8.458 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **1000**     | **0.15**     |  **1,460.16 ns** |   **9.897 ns** |   **8.773 ns** |
| UniqueUniformRandomInt_WithSet          | 1000     | 0.15     |  5,088.69 ns |  26.858 ns |  25.123 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **1000**     | **0.2**      |  **1,610.81 ns** |  **11.528 ns** |   **9.627 ns** |
| UniqueUniformRandomInt_WithSet          | 1000     | 0.2      |  6,434.26 ns | 110.239 ns | 131.232 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10000**    | **0.05**     | **10,443.89 ns** | **191.909 ns** | **179.512 ns** |
| UniqueUniformRandomInt_WithSet          | 10000    | 0.05     | 14,843.36 ns |  77.594 ns |  60.580 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10000**    | **0.1**      | **11,692.85 ns** |  **84.894 ns** |  **75.257 ns** |
| UniqueUniformRandomInt_WithSet          | 10000    | 0.1      | 30,093.98 ns |  71.663 ns |  67.034 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10000**    | **0.15**     | **13,277.57 ns** |  **67.118 ns** |  **56.046 ns** |
| UniqueUniformRandomInt_WithSet          | 10000    | 0.15     | 46,592.86 ns | 240.919 ns | 213.569 ns |
| **UniqueUniformRandomInt_WithShuffledList** | **10000**    | **0.2**      | **14,821.11 ns** |  **29.455 ns** |  **24.596 ns** |
| UniqueUniformRandomInt_WithSet          | 10000    | 0.2      | 61,902.77 ns | 227.212 ns | 212.535 ns |

Benchmarks with issues:
  UniqueRandomInt.UniqueUniformRandomInt_WithSet: DefaultJob [MaxValue=10, Fraction=0.05]

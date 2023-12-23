```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.3803/22H2/2022Update)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method        | EdgeFraction | ParameterIndex | Mean         | Error       | StdDev      |
|-------------- |------------- |--------------- |-------------:|------------:|------------:|
| **LazyPrimMst**   | **0.2**          | **10**             |     **6.827 μs** |   **0.1013 μs** |   **0.0846 μs** |
| PrimMst       | 0.2          | 10             |     6.679 μs |   0.0832 μs |   0.0737 μs |
| KruskalMst    | 0.2          | 10             |     7.622 μs |   0.1493 μs |   0.2877 μs |
| MstVyssotsky3 | 0.2          | 10             |    14.193 μs |   0.2286 μs |   0.1909 μs |
| **LazyPrimMst**   | **0.2**          | **100**            | **1,897.718 μs** |  **14.1126 μs** |  **12.5105 μs** |
| PrimMst       | 0.2          | 100            | 1,822.155 μs |  36.1011 μs |  33.7690 μs |
| KruskalMst    | 0.2          | 100            | 1,867.562 μs |  37.2267 μs |  34.8218 μs |
| MstVyssotsky3 | 0.2          | 100            | 2,921.747 μs |  53.4612 μs |  50.0076 μs |
| **LazyPrimMst**   | **0.2**          | **50**             |   **289.458 μs** |   **1.5967 μs** |   **1.4154 μs** |
| PrimMst       | 0.2          | 50             |   264.253 μs |   3.2595 μs |   3.0490 μs |
| KruskalMst    | 0.2          | 50             |   309.219 μs |   5.7904 μs |  11.2938 μs |
| MstVyssotsky3 | 0.2          | 50             |   548.228 μs |   9.5177 μs |  13.0279 μs |
| **LazyPrimMst**   | **0.5**          | **10**             |    **12.505 μs** |   **0.2467 μs** |   **0.2937 μs** |
| PrimMst       | 0.5          | 10             |    11.634 μs |   0.2316 μs |   0.2166 μs |
| KruskalMst    | 0.5          | 10             |    13.004 μs |   0.2510 μs |   0.2685 μs |
| MstVyssotsky3 | 0.5          | 10             |    29.177 μs |   0.5692 μs |   0.6990 μs |
| **LazyPrimMst**   | **0.5**          | **100**            | **5,241.812 μs** |  **50.1452 μs** |  **44.4524 μs** |
| PrimMst       | 0.5          | 100            | 4,890.379 μs |  68.3271 μs |  83.9119 μs |
| KruskalMst    | 0.5          | 100            | 5,034.270 μs |  37.7426 μs |  33.4578 μs |
| MstVyssotsky3 | 0.5          | 100            | 7,337.987 μs |  79.4265 μs |  66.3247 μs |
| **LazyPrimMst**   | **0.5**          | **50**             |   **705.265 μs** |   **9.7165 μs** |   **8.6134 μs** |
| PrimMst       | 0.5          | 50             |   623.729 μs |   7.0348 μs |   6.5803 μs |
| KruskalMst    | 0.5          | 50             |   673.480 μs |  13.1964 μs |  22.7631 μs |
| MstVyssotsky3 | 0.5          | 50             | 1,185.862 μs |  23.6828 μs |  22.1529 μs |
| **LazyPrimMst**   | **0.7**          | **10**             |    **16.529 μs** |   **0.3277 μs** |   **0.5565 μs** |
| PrimMst       | 0.7          | 10             |    14.500 μs |   0.2878 μs |   0.6008 μs |
| KruskalMst    | 0.7          | 10             |    17.205 μs |   0.3412 μs |   0.9109 μs |
| MstVyssotsky3 | 0.7          | 10             |    37.919 μs |   0.4996 μs |   0.4172 μs |
| **LazyPrimMst**   | **0.7**          | **100**            | **6,691.359 μs** | **131.1357 μs** | **175.0624 μs** |
| PrimMst       | 0.7          | 100            | 6,202.276 μs | 103.8729 μs | 127.5653 μs |
| KruskalMst    | 0.7          | 100            | 6,263.519 μs |  61.5954 μs |  54.6027 μs |
| MstVyssotsky3 | 0.7          | 100            | 9,216.029 μs | 119.8803 μs | 100.1055 μs |
| **LazyPrimMst**   | **0.7**          | **50**             |   **934.619 μs** |  **16.5032 μs** |  **14.6296 μs** |
| PrimMst       | 0.7          | 50             |   804.488 μs |   8.7499 μs |   7.7565 μs |
| KruskalMst    | 0.7          | 50             |   847.568 μs |   7.4488 μs |   6.2201 μs |
| MstVyssotsky3 | 0.7          | 50             | 1,512.781 μs |  20.3120 μs |  18.9998 μs |

The following shows the various implementations of Boruvka's algorithm perform roughly the same. 
```csharp

| Method                     | EdgeFraction | ParameterIndex | Mean     | Error    | StdDev   | Median   |
|----------------------------|------------- |--------------- |---------:|---------:|---------:|---------:|
| MstBoruvka                 | 0.7          | 200            | 59.05 ms | 1.164 ms | 2.404 ms | 60.01 ms |
| BoruvkasAlgorithmImproved  | 0.7          | 200            | 58.39 ms | 1.203 ms | 3.527 ms | 58.42 ms |
| BoruvkasAlgorithmImproved2 | 0.7          | 200            | 60.19 ms | 1.195 ms | 3.290 ms | 59.87 ms |

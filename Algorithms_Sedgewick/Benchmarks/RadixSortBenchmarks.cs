namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.List;
using AlgorithmsSW.Sort;
using AlgorithmsSW.String;
using BenchmarkDotNet.Attributes;

[MedianColumn]
public class RadixSortBenchmarks
{
	private const int MaxValue = 1 << 10;
	
	[Params(1 << 7, 1 << 10, 1 << 13, 1 << 16, 1 << 19)]
	public int ItemCount { get; set; }

	private ResizeableArray<int> list;
	
	[Benchmark(Baseline = true)]
	public int QuickSort()
	{
		ResetList();
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
		
		return list[0];
	}
	
	[Benchmark]
	public int Radix2AndInsertion()
	{
		ResetList();
		StringSort.Radix2Insertion(list, 10);
		
		return list[0];
	}
	
	[Benchmark]
	public int Radix()
	{
		ResetList();
		StringSort.RadixSort(list, MaxValue);
		
		return list[0];
	}
	
	[Benchmark]
	public int Radix2()
	{
		ResetList();
		StringSort.RadixSort2(list, MaxValue);
		
		return list[0];
	}
	
	[Benchmark]
	public int Radix4AndInsertion()
	{
		ResetList();
		StringSort.Radix4Insertion(list, 10);
		
		return list[0];
	}

	private void ResetList()
		=> list = Generator.UniformRandomInt(MaxValue)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

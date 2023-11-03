using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;

[MedianColumn]
[RPlotExporter, HtmlExporter]
[SimpleJob(RunStrategy.ColdStart, launchCount: 1, iterationCount: 5)]
public class SortBenchmarks
{
	
	[Params(100, 1_000, 10_000, 100_000)]
	public int ItemCount { get; set; }

	private ResizeableArray<int> list;
	
	
	//[Benchmark(Baseline = true)]
	public int InsertionSort()
	{
		ResetList();
		Sort.InsertionSort(list);
		
		return list[0];
	}
	
	[Benchmark]
	public int ShellSortWithPrattSequence()
	{
		ResetList();
		Sort.ShellSortWithPrattSequence(list);
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort1()
	{
		ResetList();
		Sort.ShellSort(list, new []{ 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort2()
	{
		ResetList();
		Sort.ShellSort(list, new []{3, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort3()
	{
		ResetList();
		Sort.ShellSort(list, new []{7, 3, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort4()
	{
		ResetList();
		Sort.ShellSort(list, new []{19, 7, 3, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort5()
	{
		ResetList();
		Sort.ShellSort(list, new []{59, 19, 7, 3, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort6()
	{
		ResetList();
		Sort.ShellSort(list, new []{179, 59, 19, 7, 3, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort7()
	{
		ResetList();
		Sort.ShellSort(list, new []{15359, 3839, 959, 239, 59, 15, 4, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort8()
	{
		ResetList();
		Sort.ShellSort(list, new []{701, 301, 132, 57, 23, 10, 4, 1});
		
		return list[0];
	}
	
	//[Benchmark]
	public int ShellSort11()
	{
		ResetList();
		Sort.ShellSort(list, new []{10001,6001,1001, 61, 1});
		
		return list[0];
	}
	
	[Benchmark]
	public int ShellSort12()
	{
		ResetList();
		Sort.ShellSort(list, new []{20001, 10001,9001,7001, 6001, 5001,3001,1001, 91, 71, 61, 51, 31, 19, 1});
		
		return list[0];
	}
	
	[Benchmark]
	public int ShellSort13()
	{
		ResetList();
		Sort.ShellSort(list, new []{1001, 51, 1001, 3, 1});
		
		return list[0];
	}
	
	
	//[Benchmark]
	public int HeapSort()
	{
		ResetList();
		Sort.HeapSort(list);
		
		return list[0];
	}
	
	//[Benchmark]
	public int MergeSort()
	{
		ResetList();
		Sort.MergeSort(list);
		
		return list[0];
	}
	
	//[Benchmark]
	public int QuickSort()
	{
		ResetList();
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
		
		return list[0];
	}

	private void ResetList()
		=> list = Generator.UniformRandomInt(int.MaxValue)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

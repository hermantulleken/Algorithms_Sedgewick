using Algorithms_Sedgewick;
using Algorithms_Sedgewick.Sort;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;


[MedianColumn]
[RPlotExporter, HtmlExporter]
[SimpleJob(RunStrategy.ColdStart, launchCount: 10, iterationCount: 10)] 
public class SearchBenchmarks
{
	private readonly int[] array;
	private readonly int[] keys;

	public SearchBenchmarks()
	{
		const int size = 100_000;
		const int range = 100000000;
		const int keyCount = 80_000;
			
		var list = Generator
			.UniformRandomInt(range)
			.Take(size)
			.ToResizableArray(size);
			
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
		array = list.ToArray();
			
		keys = Generator.UniformRandomInt(range)
			.Take(keyCount)
			.ToArray();
	}

	[Benchmark]
	public void InterpolationSearch()
	{
		foreach (int key in keys)
		{
			array.InterpolationSearch(key);
		}
	}
	
	[Benchmark]
	public void BinarySearch()
	{
		foreach (int key in keys)
		{
			array.BinarySearch(key);
		}
	}
	
	[Benchmark]
	public void CSharpBinarySearch()
	{
		foreach (int key in keys)
		{
			Array.BinarySearch(array, key);
		}
	}
	
	[Benchmark]
	public void SequentialSearch()
	{
		foreach (int key in keys)
		{
			array.SequentialSearch(key);
		}
	}
}

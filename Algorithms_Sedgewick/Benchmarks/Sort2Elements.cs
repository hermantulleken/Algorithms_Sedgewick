using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Sort;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class Sort2Elements
{
	private const int Count = 100_000;
	
	private ResizeableArray<int> list;
	[Params( /*Count / 16, Count / 8, Count / 4, */Count / 2, Count)]
	public int ItemCount { get; set; }

	[Benchmark]
	public void QuickSort()
	{
		ResetList();
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
	}
	
	[Benchmark]
	public void Sort2()
	{
		ResetList();
		Sort.Sort2(list);
	}

	[Benchmark]
	public void ShellShort()
	{
		ResetList();
		Sort.ShellSortWithPrattSequence(list);
	}
	
	private void ResetList()
		=> list = Generator.UniformRandomInt(2)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

public class Sort3Elements
{
	private const int Count = 1_000_000;
	
	private ResizeableArray<int> list;
	[Params( /*Count / 16, Count / 8, Count / 4, */Count / 2, Count)]
	public int ItemCount { get; set; }

	[Benchmark]
	public void QuickSort()
	{
		ResetList();
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
	}
	
	[Benchmark]
	public void DutchFlagSort()
	{
		ResetList();
		Sort.DutchFlagSort(list);
	}

	[Benchmark]
	public void ShellShort()
	{
		ResetList();
		Sort.ShellSortWithPrattSequence(list);
	}
	
	private void ResetList()
		=> list = Generator.UniformRandomInt(3)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Pool;
using Algorithms_Sedgewick.Queue;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;

[MedianColumn]
[RPlotExporter, HtmlExporter]
[SimpleJob(RunStrategy.Throughput, launchCount: 1, iterationCount: 5)]
public class MergeSortBenchmarks
{
	
	private class QueueFactory<T> : IFactory<QueueWithLinkedList<T>>
	{
		public QueueWithLinkedList<T> GetNewInstance() => new();
		public void DestroyInstance(QueueWithLinkedList<T> instance)
		{
			instance.Clear();
		}
	}

	private const int Count = 1 << 17;

	[Params( Count / 16, Count / 8, Count / 4, Count / 2, Count)]
	public int ItemCount { get; set; }

	private ResizeableArray<int> list;
	private readonly FixedPreInitializedPool<QueueWithLinkedList<int>> minorQueuePool;
	private readonly FixedPreInitializedPool<QueueWithLinkedList<QueueWithLinkedList<int>>> majorQueuePool;
	
	public MergeSortBenchmarks()
	{
		minorQueuePool = new FixedPreInitializedPool<QueueWithLinkedList<int>>(
			new QueueFactory<int>(),
			Count << 1,
			QueueWithLinkedList<int>.Comparer);
		
		majorQueuePool = new FixedPreInitializedPool<QueueWithLinkedList<QueueWithLinkedList<int>>>(
			new QueueFactory<QueueWithLinkedList<int>>(),
			Count << 1,
			QueueWithLinkedList<QueueWithLinkedList<int>>.Comparer);
	}

	private void ResetList()
		=> list = Generator.UniformRandomInt(int.MaxValue)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);

	//[Benchmark(Baseline = true)]
	public void MergeSort()
	{
		ResetList();
		Sort.MergeSort(list);
	}
	
	//[Benchmark]
	public void MergeSort_SkipMergeIfPossible()
	{
		ResetList();
		Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true});
	}
	
	//[Benchmark]
	public void MergeSort_FastMerge()
	{
		ResetList();
		Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true});
	}
	
	//[Benchmark]
	public void MergeSort_InsertSortSmallArrays()
	{
		ResetList();
		Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8});
	}
	
	//[Benchmark]
	public void MergeSort_AllOptimizations()
	{
		ResetList();
		Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true, UseFastMerge = true, SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8});
	}
	
	//[Benchmark]
	public void MergeSortBottomUp()
	{
		ResetList();
		Sort.MergeSortBottomUp(list);
	}
	
	//[Benchmark]
	public void MergeSortBottomUp_SkipMergeIfPossible()
	{
		ResetList();
		Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true});
	}
	
	//[Benchmark]
	public void MergeSortBottomUp_FastMerge()
	{
		ResetList();
		Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true});
	}
	
	//[Benchmark]
	public void MergeSortBottomUp_InsertSortSmallArrays()
	{
		ResetList();
		Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8});
	}
	
	//[Benchmark]
	public void MergeSortBottomUp_AllOptimizations()
	{
		ResetList();
		Sort.MergeSortBottomUp(list,
			Sort.MergeSortConfig.Vanilla with { SkipMergeWhenSorted = true, UseFastMerge = true, SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8 });
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueues()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list);
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueuesPool()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool, minorQueuePool);
	}
	
	//[Benchmark]
	public void Merge3Sort()
	{
		ResetList();
		Sort.Merge3Sort(list);
	}
	
	//[Benchmark]
	public void MergeKSort3()
	{
		ResetList();
		Sort.MergeKSort(list, 3);
	}
	
	//[Benchmark]
	public void MergeKSort4()
	{
		ResetList();
		Sort.MergeKSort(list, 4);
	}
	
	//[Benchmark]
	public void MergeKSortBottomUp3()
	{
		ResetList();
		Sort.MergeKSortBottomUp(list, 3);
	}
	
	//[Benchmark]
	public void MergeKSortBottomUp4()
	{
		ResetList();
		Sort.MergeKSortBottomUp(list, 4);
	}
	
	//[Benchmark]
	public void MergeSortNatural()
	{
		ResetList();
		Sort.MergeSortNatural(list);
	}
}

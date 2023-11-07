using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Object;
using Algorithms_Sedgewick.Pool;
using Algorithms_Sedgewick.Queue;
using Algorithms_Sedgewick.Sort;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;

[MedianColumn]
[SimpleJob(RunStrategy.Throughput, launchCount: 1, iterationCount: 1)]
public class MergeSortBenchmarks
{
	private const int Count = 1 << 10;

	[Params( /*Count / 16, Count / 8, Count / 4, */Count / 2, Count)]
	public int ItemCount { get; set; }

	private ResizeableArray<int> list;
	
	private readonly FixedPreInitializedPool<IQueue<int>> minorQueuePool_Fixed;
	private readonly FixedPreInitializedPool<IQueue<IQueue<int>>> majorQueuePool_Fixed;
	private readonly FixedPreInitializedPool<IQueue<int>> minorQueuePool_Linked;
	private readonly FixedPreInitializedPool<IQueue<IQueue<int>>> majorQueuePool_Linked;
	private static FixedPreInitializedPool<LinkedListWithPooledNodes<IQueue<int>>.Node> majorNodePool;
	private static FixedPreInitializedPool<LinkedListWithPooledNodes<int>.Node> minorNodePool;

	public MergeSortBenchmarks()
	{
		minorQueuePool_Fixed = CreateMinorQueuePool_Fixed();
		majorQueuePool_Fixed = CreateMajorQueuePool_Fixed();
		
		minorQueuePool_Linked = CreateMinorQueuePool_Linked();
		majorQueuePool_Linked = CreateMajorQueuePool_Linked();
	}

	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool_Fixed()
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new FixedCapacityQueue<IQueue<int>>(Count), queue => queue.Clear()),
			Count << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool_Fixed()
	{
		return new FixedPreInitializedPool<IQueue<int>>(
			Factory.Create<IQueue<int>>(() => new FixedCapacityQueue<int>(Count), queue => queue.Clear()),
			Count << 1);
	}
	
	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool_Linked()
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new QueueWithLinkedListAndNodePool<IQueue<int>>(Count), queue => queue.Clear()),
			Count << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool_Linked()
	{
		return new FixedPreInitializedPool<IQueue<int>>(
			Factory.Create<IQueue<int>>(() => new QueueWithLinkedListAndNodePool<int>(Count), queue => queue.Clear()),
			Count << 1);
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
			Sort.MergeSortConfig.Vanilla 
				with 
				{ 
					SkipMergeWhenSorted = true, UseFastMerge = true, 
					SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, 
					SmallArraySize = 8
				});
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueues()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list);
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueuesPool_FF()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool_Fixed, minorQueuePool_Fixed);
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueuesPoolAndNodesPool_LL()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool_Linked, minorQueuePool_Linked);
		
		/*Debug.Assert(majorQueuePool.AliveElementCount == 0);
		Debug.Assert(minorQueuePool.AliveElementCount == 0);*/
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueuesPoolAndNodesPool_LF()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool_Linked, minorQueuePool_Fixed);
		
		/*Debug.Assert(majorQueuePool.AliveElementCount == 0);
		Debug.Assert(minorQueuePool.AliveElementCount == 0);*/
	}
	
	[Benchmark]
	public void MergeSortBottomsUpWithQueuesPoolAndNodesPool_FL()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool_Fixed, minorQueuePool_Linked);
		
		/*Debug.Assert(majorQueuePool.AliveElementCount == 0);
		Debug.Assert(minorQueuePool.AliveElementCount == 0);*/
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

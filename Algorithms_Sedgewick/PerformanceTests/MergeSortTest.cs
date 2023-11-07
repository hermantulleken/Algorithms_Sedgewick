using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Pool;
using Algorithms_Sedgewick.Queue;
using Algorithms_Sedgewick.Sort;

namespace PerformanceTests;

public class MergeSortTest
{
	private const int ItemCount = 5_000;
	private const int IterationCount = 100000;
	
	private readonly FixedPreInitializedPool<IQueue<int>> minorQueuePool_Fixed;
	private readonly FixedPreInitializedPool<IQueue<IQueue<int>>> majorQueuePool_Fixed;
	private readonly FixedPreInitializedPool<IQueue<int>> minorQueuePool_Linked;
	private readonly FixedPreInitializedPool<IQueue<IQueue<int>>> majorQueuePool_Linked;
	
	private ResizeableArray<int> list;

	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool_Fixed()
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new FixedCapacityQueue<IQueue<int>>(ItemCount), queue => queue.Clear()),
			ItemCount << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool_Fixed()
	{
		return new FixedPreInitializedPool<IQueue<int>>(
			Factory.Create<IQueue<int>>(() => new FixedCapacityQueue<int>(ItemCount), queue => queue.Clear()),
			ItemCount << 1);
	}
	
	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool_Linked()
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new QueueWithLinkedListAndNodePool<IQueue<int>>(ItemCount), queue => queue.Clear()),
			ItemCount << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool_Linked()
	{
		return new FixedPreInitializedPool<IQueue<int>>(
			Factory.Create<IQueue<int>>(() => new QueueWithLinkedListAndNodePool<int>(ItemCount), queue => queue.Clear()),
			ItemCount << 1);
	}
	
	public MergeSortTest()
	{
		minorQueuePool_Fixed = CreateMinorQueuePool_Fixed();
		majorQueuePool_Fixed = CreateMajorQueuePool_Fixed();
		
		minorQueuePool_Linked = CreateMinorQueuePool_Linked();
		majorQueuePool_Linked = CreateMajorQueuePool_Linked();
		
		Console.WriteLine("Creation done.");
	}

	public void Run()
	{
		for (int i = 0; i < IterationCount; i++)
		{
			RunIteration();
		}
	}

	public void RunIteration()
	{
		ResetList();
		Sort.MergeSortBottomsUpWithQueues(list, majorQueuePool_Fixed, minorQueuePool_Linked);
	}
	
	private void ResetList()
		=> list = Generator.UniformRandomInt(int.MaxValue)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

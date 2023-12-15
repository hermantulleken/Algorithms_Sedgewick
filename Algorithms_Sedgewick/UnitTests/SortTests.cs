using System.Linq;
using AlgorithmsSW;
using AlgorithmsSW.List;
using AlgorithmsSW.Object;
using AlgorithmsSW.Pool;
using AlgorithmsSW.Queue;
using NUnit.Framework;
using Support;
using static AlgorithmsSW.Sort.Sort;

namespace UnitTests;

[Parallelizable]
public class SortTests
{
	private static readonly Action<IRandomAccessList<int>, int, int>[] PartialSortFunctions =
	{
		InsertionSort,
	};
	
	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool(int count)
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new QueueWithLinkedList<IQueue<int>>(), queue => queue.Clear()),
			count << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool(int count)
	{
		return new FixedPreInitializedPool<IQueue<int>>(
			Factory.Create<IQueue<int>>(() => new QueueWithLinkedList<int>(), queue => queue.Clear()),
			count << 1);
	}
	
	private static FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool1(int count)
	{
		return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
			Factory.Create<IQueue<IQueue<int>>>(() => new QueueWithLinkedListAndNodePool<IQueue<int>>(count), queue => queue.Clear()),
			count << 1);
	}

	private static FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool1(int count)
	{
		return new FixedPreInitializedPool<IQueue<int>>( 
			Factory.Create<IQueue<int>>(() => new QueueWithLinkedListAndNodePool<int>(count), queue => queue.Clear()),
			count << 1);
	}

	[DatapointSource]
	private static readonly Action<IRandomAccessList<int>>[] SortFunctions = 
	{
		SelectionSort,
		InsertionSort,
		ShellSortWithPrattSequence,
		list => ShellSort(list, new[] { 7, 3, 1 }),
		DequeueSortWithDeque,
		DequeueSortWithQueue,
		GnomeSort,
		HeapSort,
		MergeSort,
		list => MergeSort(list, MergeSortConfig.Vanilla with { SkipMergeWhenSorted = true}),
		list => MergeSort(list, MergeSortConfig.Vanilla with {UseFastMerge = true}),
		list => MergeSort(list, MergeSortConfig.Vanilla with {SmallArraySortAlgorithm = MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		MergeSortBottomUp,
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{UseFastMerge = true}),
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		list => MergeSortBottomsUpWithQueues(list),
		
		list => MergeSortBottomsUpWithQueues(
			list, 
			CreateMajorQueuePool(list.Count),
			CreateMinorQueuePool(list.Count)),
		list => MergeSortBottomsUpWithQueues(
			list, 
			CreateMajorQueuePool1(list.Count),
			CreateMinorQueuePool1(list.Count)),
		Merge3Sort,
		list => MergeKSort(list, 3),
		list => MergeKSort(list, 4),
		list => MergeKSortBottomUp(list, 3),
		list => MergeKSortBottomUp(list, 4),
		MergeSortNatural,
		list => QuickSort(list, QuickSortConfig.Vanilla),
		list => QuickSort(list, QuickSortConfig.Vanilla with {PivotSelection = QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst}),
	};

	private static readonly IReadonlyRandomAccessList<int> TestArray = new[] { 5, 9, 1, 23, 6, 2, 6, 18, 2, 3, 7, 6, 11, 71, 8, 4, 19 }.ToRandomAccessList();

	[DatapointSource] 
	private IReadonlyRandomAccessList<int>[] lists = 
	{
		new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToRandomAccessList(),
		new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }.ToRandomAccessList(),
		TestArray,
	};

	[TestCaseSource(nameof(SortFunctions))]
	public void SortCornerCasesTest(Action<IRandomAccessList<int>> sortFunction)
	{
		var list1 = Array.Empty<int>().ToRandomAccessList();
		sortFunction(list1);
		Assert.That(list1, Is.Empty);
		
		var list2 = new[]{1}.ToRandomAccessList();
		sortFunction(list2);
		Assert.That(list2.Count, Is.EqualTo(1));
		Assert.That(list2[0], Is.EqualTo(1));
	}

	[Theory]
	public void SortFunctionTest(IReadonlyRandomAccessList<int> input, Action<IRandomAccessList<int>> sortFunction)
	{
		int[] expectedOutput = input.OrderBy(x => x).ToArray();
		var copy = input.Copy(); // Do not sort the original
		sortFunction(copy);
		Assert.That(copy, Is.EqualTo(expectedOutput));
		Console.WriteLine(copy.Pretty());
	}

	[TestCaseSource(nameof(SortFunctions))]
	public void SortTest(Action<IRandomAccessList<int>> sortFunction)
	{
		var list = TestArray.Copy();
		sortFunction(list);
		int[] expected = TestArray.OrderBy(x => x).ToArray();
		Assert.That(list, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(PartialSortFunctions))]
	public void TestPartialSort(Action<IRandomAccessList<int>, int, int> sortFunction)
	{
		int startIndex = 4;
		int endIndex = 7;
		
		var list = TestArray.Copy();

		sortFunction(list, startIndex, endIndex);
		
		Assert.That(AreElementsEqual(TestArray, list, 0, startIndex), Is.True);
		Assert.That(IsSortedAscending(list, startIndex, endIndex));
		Assert.That(AreElementsEqual(TestArray, list, endIndex, TestArray.Count), Is.True);
	}

	[Test]
	public void TestSelect()
	{
		var list = new ResizeableArray<int>() { 5, 3, 6, 2, 8, 2, 1, 7 };
		var config = QuickSortConfig.Vanilla;
		
		Assert.That(SelectNthLowest(list, 0, config), Is.EqualTo(1));
		Assert.That(SelectNthLowest(list, 1, config), Is.EqualTo(2));
		Assert.That(SelectNthLowest(list, 2, config), Is.EqualTo(2));
		Assert.That(SelectNthLowest(list, 5, config), Is.EqualTo(6));
		Assert.That(SelectNthLowest(list, 7, config), Is.EqualTo(8));
	}

	[Test]
	public void Test_MergeSortBottomsUpWithQueues()
	{
		int count = 1 << 8;
		
		FixedPreInitializedPool<IQueue<IQueue<int>>> CreateMajorQueuePool1()
		{
			return new FixedPreInitializedPool<IQueue<IQueue<int>>>(
				Factory.Create<IQueue<IQueue<int>>>(() => new QueueWithLinkedListAndNodePool<IQueue<int>>(count), queue => queue.Clear()),
				count << 1);
		}

		FixedPreInitializedPool<IQueue<int>> CreateMinorQueuePool1()
		{
			return new FixedPreInitializedPool<IQueue<int>>(
				Factory.Create<IQueue<int>>(() => new QueueWithLinkedListAndNodePool<int>(count), queue => queue.Clear()),
				count << 1);
		}
		
		var list = Generator.UniformRandomInt(int.MaxValue)
			.Take(count)
			.ToResizableArray(count);
		
		MergeSortBottomsUpWithQueues(
			list, 
			CreateMajorQueuePool1(),
			CreateMinorQueuePool1());

	}
}

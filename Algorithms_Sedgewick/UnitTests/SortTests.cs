using System.Linq;
using Algorithms_Sedgewick.List;
using NUnit.Framework;
using Support;
using static Algorithms_Sedgewick.Sort;

namespace UnitTests;

[Parallelizable]
public class SortTests
{
	private static readonly Action<IReadonlyRandomAccessList<int>, int, int>[] PartialSortFunctions =
	{
		InsertionSort,
	};

	[DatapointSource]
	private static readonly Action<IReadonlyRandomAccessList<int>>[]SortFunctions = 
	{
		SelectionSort,
		InsertionSort,
		ShellSortWithPrattSequence,
		DequeueSortWithDeque,
		DequeueSortWithQueue,
		GnomeSort,
		HeapSort,
		MergeSort,
		list => MergeSort(list, MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
		list => MergeSort(list, MergeSortConfig.Vanilla with{UseFastMerge = true}),
		list => MergeSort(list, MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		MergeSortBottomUp,
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{UseFastMerge = true}),
		list => MergeSortBottomUp(list, MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		MergeSortBottomsUpWithQueues,
		Merge3Sort,
		list => MergeKSort(list, 3),
		list => MergeKSort(list, 4),
		list => MergeKSortBottomUp(list, 3),
		list => MergeKSortBottomUp(list, 4),
		MergeSortNatural,
		list => QuickSort(list, QuickSortConfig.Vanilla),
		list => QuickSort(list, QuickSortConfig.Vanilla with {PivotSelection = QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst}),
	};

	private static readonly IReadonlyRandomAccessList<int> TestArray = new []{5, 9, 1, 23, 6, 2, 6, 18, 2, 3, 7, 6, 11, 71, 8, 4,  19}.ToRandomAccessList();

	[DatapointSource] 
	private IReadonlyRandomAccessList<int>[] lists = 
	{
		new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToRandomAccessList(),
		new [] {9, 8, 7, 6, 5, 4, 3, 2, 1 }.ToRandomAccessList(),
		TestArray
	};

	[TestCaseSource(nameof(SortFunctions))]
	public void SortCornerCasesTest(Action<IReadonlyRandomAccessList<int>> sortFunction)
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
	public void SortFunctionTest(IReadonlyRandomAccessList<int> input, Action<IReadonlyRandomAccessList<int>> sortFunction)
	{
		int[] expectedOutput = input.OrderBy(x => x).ToArray();

		sortFunction(input);
		Assert.That(input, Is.EqualTo(expectedOutput));
		Console.WriteLine(input.Pretty());
	}

	[TestCaseSource(nameof(SortFunctions))]
	public void SortTest(Action<IReadonlyRandomAccessList<int>> sortFunction)
	{
		var list = TestArray.Copy();
		sortFunction(list);
		int[] expected = TestArray.OrderBy(x => x).ToArray();
		Assert.That(list, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(PartialSortFunctions))]
	public void TestPartialSort(Action<IReadonlyRandomAccessList<int>, int, int> sortFunction)
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
}

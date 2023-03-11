using System;
using System.Collections;
using System.Linq;
using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Sort;
using NUnit.Framework;
using Support;

namespace Algorithms_Sedgewick_Tests;

public class SortTests
{
	private static readonly IRandomAccessList<int> TestArray = new []{5, 9, 1, 23, 6, 2, 6, 18, 2, 3, 7, 6, 11, 71, 8, 4,  19}.ToRandomAccessList();

	[DatapointSource]
	private static readonly Action<IRandomAccessList<int>>[]SortFunctions = 
	{
		Sort.SelectionSort,
		Sort.InsertionSort,
		Sort.ShellSortWithPrattSequence,
		Sort.DequeueSortWithDeque,
		Sort.DequeueSortWithQueue,
		Sort.GnomeSort,
		Sort.MergeSort,
		list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
		list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
		list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		Sort.MergeSortBottomUp,
		list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
		list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
		list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 8}),
		Sort.MergeSortBottomsUpWithQueues,
	};

	private static readonly Action<IRandomAccessList<int>, int, int>[] PartialSortFunctions =
	{
		Sort.InsertionSort,
	};

	[DatapointSource]
	private static IEnumerable SortTestCases()
	{
		yield return new TestCaseData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToRandomAccessList())
			.SetName("Sorts in ascending order");

		yield return new TestCaseData(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }.ToRandomAccessList())
			.SetName("Sorts in descending order");

		yield return new TestCaseData(TestArray.OrderBy(x => x).ToArray().ToRandomAccessList())
			.SetName("Leaves the original array intact");
	}

	[TestCaseSource(nameof(SortFunctions))]
	public void SortTest(Action<IRandomAccessList<int>> sortFunction)
	{
		var list = TestArray.Copy();

		var listStr1 = list.Pretty();
		
		sortFunction(list);
		var listStr2 = list.Pretty();
		var expected = TestArray.OrderBy(x => x).ToArray();
		Assert.That(list, Is.EqualTo(expected));
	}

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
	public void SortFunctionTest(IRandomAccessList<int> input, Action<IRandomAccessList<int>> sortFunction)
	{
		int[] expectedOutput = input.OrderBy(x => x).ToArray();

		//foreach (var sortFunction in SortFunctions)
		{
			sortFunction(input);
			Assert.That(input, Is.EqualTo(expectedOutput));
			Console.WriteLine(input.Pretty());
		}
	}

	[TestCaseSource(nameof(PartialSortFunctions))]
	public void TestPartialSort(Action<IRandomAccessList<int>, int, int> sortFunction)
	{
		//Action<IRandomAccessList<int>, int, int> sortAlgorithm = Sort.InsertionSort;
		int startIndex = 4;
		int endIndex = 7;
		
		var list = TestArray.Copy();

		sortFunction(list, startIndex, endIndex);
		
		Assert.That(Sort.AreElementsEqual(TestArray, list, 0, startIndex), Is.True);
		Assert.That(Sort.IsSortedAscending(list, startIndex, endIndex));
		Assert.That(Sort.AreElementsEqual(TestArray, list, endIndex, TestArray.Count), Is.True);
	}
}

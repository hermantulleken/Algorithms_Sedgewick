using System;
using System.Collections;
using System.Linq;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Sort;
using NUnit.Framework;
using Support;

namespace Algorithms_Sedgewick_Tests;

public class SortTests
{
	private static readonly IRandomAccessList<int> TestArray = new []{5, 2, 8, 4, 9, 1, 3, 7, 6}.ToRandomAccessList();

	private static readonly Action<IRandomAccessList<int>>[]SortFunctions = 
	{
		Sort.SelectionSort,
		Sort.InsertionSort,
		Sort.ShellSort,
		Sort.DequeueSortWithDeque,
		Sort.DequeueSortWithQueue,
		Sort.GnomeSort,
		Sort.MergeSort
	};

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
		sortFunction(list);

		Assert.That(list, Is.EqualTo(TestArray.OrderBy(x => x).ToArray()));
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

	[TestCaseSource(nameof(SortTestCases))]
	public void SortFunctionTest(IRandomAccessList<int> input)
	{
		int[] expectedOutput = input.OrderBy(x => x).ToArray();

		foreach (var sortFunction in SortFunctions)
		{
			sortFunction(input);
			Assert.That(input, Is.EqualTo(expectedOutput));
			Console.WriteLine(input.Pretty());
		}
	}
}

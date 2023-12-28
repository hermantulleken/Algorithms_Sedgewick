namespace UnitTests;

using AlgorithmsSW.List;
using AlgorithmsSW.Sort;

[TestFixture]
public class DutchFlagSortTests
{
	// Test with 0 elements
	[Test]
	public void TestEmptyList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int>();
		Sort.DutchFlagSort(list);
		Assert.That(list.Count, Is.EqualTo(0));
	}

	// Test with 1 element
	[Test]
	[TestCase(0)]
	[TestCase(1)]
	[TestCase(2)] // Added case for the third element
	public void TestSingleElementList(int element)
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { element };
		Sort.DutchFlagSort(list);
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(list[0], Is.EqualTo(element));
	}

	// Test with 3 elements in different orders
	[Test]
	[TestCase(0, 1, 2)]
	[TestCase(2, 1, 0)]
	[TestCase(1, 2, 0)]
	[TestCase(2, 0, 1)]
	// ... Add other combinations as needed
	public void TestThreeElementList(int a, int b, int c)
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { a, b, c };
		Sort.DutchFlagSort(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 1, 2 }));
	}

	// Test with all same elements
	[Test]
	[TestCase(0)]
	[TestCase(1)]
	[TestCase(2)]
	public void TestAllSameElements(int element)
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { element, element, element };
		Sort.DutchFlagSort(list);
		Assert.That(list, Is.EqualTo(new[] { element, element, element }));
	}

	// Test already sorted list
	[Test]
	public void TestAlreadySortedList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 0, 1, 2 };
		Sort.DutchFlagSort(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 1, 2 }));
	}

	// Test larger list with mixed elements
	[Test]
	public void TestLargerList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 2, 1, 0, 1, 2, 0, 2, 1, 0 };
		Sort.DutchFlagSort(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 }));
	}
}

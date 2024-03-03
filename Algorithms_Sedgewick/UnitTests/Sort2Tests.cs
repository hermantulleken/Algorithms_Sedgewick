namespace UnitTests;

using AlgorithmsSW.List;
using AlgorithmsSW.Sort;

[TestFixture]
public class Sort2Tests
{
	// Test with 0 elements
	[Test]
	public void TestEmptyList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int>();
		Sort.Sort2(list);
		Assert.That(list.Count, Is.EqualTo(0));
	}

	// Test with 1 element
	[Test]
	[TestCase(0)]
	[TestCase(1)]
	public void TestSingleElementList(int element)
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { element };
		Sort.Sort2(list);
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(list[0], Is.EqualTo(element));
	}

	// Test with 2 elements: 0, 1
	[Test]
	public void TestTwoElementList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 0, 1 };
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 1 }));
	}

	// Test with 3 elements: unsorted order
	[Test]
	public void TestThreeElementList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 1, 0, 1 };
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 1, 1 }));
	}

	// Test all 0s
	[Test]
	public void TestAllZeroes()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 0, 0, 0 };
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 0, 0 }));
	}

	// Test all 1s
	[Test]
	public void TestAllOnes()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 1, 1, 1 };
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 1, 1, 1 }));
	}

	// Test already sorted list
	[Test]
	public void TestAlreadySortedList()
	{
		IRandomAccessList<int> list = new ResizeableArray<int> { 0, 0, 1, 1 };
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 0, 1, 1 }));
	}
	
	[Test]
	public void Test10Elements()
	{
		ResizeableArray<int> list = [1, 0, 1, 0, 0, 1, 0, 1, 1, 0];
		Sort.Sort2(list);
		Assert.That(list, Is.EqualTo(new[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }));
	}
}

using System.Collections.Generic;
using Algorithms_Sedgewick.List;
using NUnit.Framework;

namespace UnitTests;

[TestOf(typeof(ResizeableArray<>))]
public class ResizeableArrayTest
{
	[Test]
	public void Add_WhenFull_IncreasesCapacity()
	{
		var arr = new ResizeableArray<int>(2);
		arr.Add(1);
		arr.Add(2);
		arr.Add(3);
		Assert.AreEqual(3, arr.Count);
		Assert.AreEqual(4, arr.Capacity);
	}

	[Test]
	public void Add_WhenNotFull_IncreasesCount()
	{
		var arr = new ResizeableArray<int>(10);
		arr.Add(1);
		Assert.AreEqual(1, arr.Count);
	}

	[Test]
	public void Capacity_WithCapacityParameter_ReturnsCapacity()
	{
		var arr = new ResizeableArray<int>(10);
		Assert.AreEqual(10, arr.Capacity);
	}

	///
	[Test]
	public void Count_EmptyArray_ReturnsZero()
	{
		var arr = new ResizeableArray<int>();
		Assert.AreEqual(0, arr.Count);
	}

	[Test]
	public void Enumerator_ReturnsAllElements()
	{
		var arr = new ResizeableArray<int> { 1, 2, 3 };

		var result = new List<int>();

		foreach (int item in arr)
		{
			result.Add(item);
		}

		Assert.AreEqual(3, result.Count);
		Assert.AreEqual(1, result[0]);
		Assert.AreEqual(2, result[1]);
		Assert.AreEqual(3, result[2]);
	}

	[Test]
	public void Enumerator_ReturnsItemsInCorrectOrder()
	{
		var array = new ResizeableArray<int>();
		array.Add(1);
		array.Add(2);
		array.Add(3);

		using var result = array.GetEnumerator();

		Assert.That(result.MoveNext(), Is.True);
		Assert.That(result.Current, Is.EqualTo(1));
		Assert.That(result.MoveNext(), Is.True);
		Assert.That(result.Current, Is.EqualTo(2));
		Assert.That(result.MoveNext(), Is.True);
		Assert.That(result.Current, Is.EqualTo(3));
		Assert.That(result.MoveNext(), Is.False);
	}

	[Test]
	public void Enumerator_WhenModifiedDuringIteration_ThrowsInvalidOperationException()
	{
		var array = new ResizeableArray<int>();
		array.Add(1);
		array.Add(2);
		var enumerator = array.GetEnumerator();
		enumerator.MoveNext();

		array.Add(3);

		Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
	}

	[Test]
	public void Indexer_Get_WhenIndexInRange_ReturnsValue()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr.Add(2);
		var result = arr[1];
		Assert.AreEqual(2, result);
	}

	[Test]
	public void Indexer_Get_WhenIndexOutOfRange_ThrowsIndexOutOfRangeException()
	{
		var arr = new ResizeableArray<int>();
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_ = arr[0];
		});
	}

	[Test]
	public void Indexer_Set_WhenIndexInRange_SetsValue()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr[0] = 2;
		Assert.AreEqual(2, arr[0]);
	}

	[Test]
	public void Indexer_Set_WhenIndexOutOfRange_ThrowsIndexOutOfRangeException()
	{
		var arr = new ResizeableArray<int>();
		Assert.Throws<ArgumentOutOfRangeException>(() => arr[0] = 1);
	}

	[Test]
	public void RemoveLast_WhenEmpty_ThrowsInvalidOperationException()
	{
		var arr = new ResizeableArray<int>();
		Assert.Throws<InvalidOperationException>(() => arr.RemoveLast());
	}

	[Test]
	public void RemoveLast_WhenNotEmpty_DecreasesCount()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr.Add(2);
		arr.RemoveLast();
		Assert.AreEqual(1, arr.Count);
	}

	[Test]
	public void RemoveLast_WhenNotEmpty_ReturnsLastElement()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr.Add(2);
		var result = arr.RemoveLast();
		Assert.AreEqual(2, result);
	}

	[Test]
	public void TestAdd()
	{
		var list = new ResizeableArray<int> { 0, 1, 2 };
		list.Add(3);
		Assert.That(list.Count, Is.EqualTo(4));
		Assert.That(list[3], Is.EqualTo(3));
	}

	[Test]
	public void TestGrowsCapacity()
	{
		var list = new ResizeableArray<int>(4) { 0, 1, 2, 3 };
		Assert.That(list.Capacity, Is.EqualTo(4));
		list.Add(5);
		Assert.That(list.Capacity, Is.GreaterThanOrEqualTo(5));
	}

	[Test]
	public void TestIndexing()
	{
		var list = new ResizeableArray<int> { 0, 1, 2 };

		Assert.That(list[0], Is.EqualTo(0));
		list[0] = 7;
		Assert.That(list[0], Is.EqualTo(7));
	}

	[Test]
	public void TestIndexOutOfRangeThrows()
	{
		var list = new ResizeableArray<int>{0, 1, 2 };
		// ReSharper disable once NotAccessedVariable
		int n = 0;
		Assert.That(() => { n = list[4];}, Throws.Exception);
	}

	[Test]
	public void TestIsEmpty()
	{
		var list = new ResizeableArray<int>();
		
		Assert.That(list.IsEmpty, Is.True);
		
		list.Add(0);
		
		Assert.That(list.IsEmpty, Is.False);
	}

	[Test]
	public void TestRemove()
	{
		var list = new ResizeableArray<int> { 0, 1, 2 };
		list.RemoveLast();
		Assert.That(list.Count, Is.EqualTo(2));
	}

	[Test]
	public void TestRemoveLastThrowsWhenEmpty()
	{
		var list = new ResizeableArray<int>();
		
		Assert.That(() => { list.RemoveLast();}, Throws.InvalidOperationException);
	}

	[Test]
	public void TestToString()
	{
		var list = new ResizeableArray<int>{0, 1, 2};
		var str = list.ToString();
		Assert.That(str, Is.EqualTo("[0, 1, 2]"));
	}
}

namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW.List;

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
		
		Assert.That(arr, Has.Count.EqualTo(3));
		Assert.That(arr.Capacity, Is.EqualTo(ResizeableArray.DefaultCapacity));
	}

	[Test]
	public void Add_WhenNotFull_IncreasesCount()
	{
		var arr = new ResizeableArray<int>(10);
		
		arr.Add(1);
		
		Assert.That(arr, Has.Count.EqualTo(1));
	}

	[Test]
	public void Capacity_WithCapacityParameter_ReturnsCapacity()
	{
		var arr = new ResizeableArray<int>(10);
		Assert.That(arr.Capacity, Is.EqualTo(10));
	}

	[Test]
	public void Count_EmptyArray_ReturnsZero()
	{
		var arr = new ResizeableArray<int>();
		Assert.That(arr, Is.Empty);
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

		Assert.That(result, Has.Count.EqualTo(3));
		Assert.That(result[0], Is.EqualTo(1));
		Assert.That(result[1], Is.EqualTo(2));
		Assert.That(result[2], Is.EqualTo(3));
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
		enumerator.Dispose();
	}

	[Test]
	public void Indexer_Get_WhenIndexInRange_ReturnsValue()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr.Add(2);
		var result = arr[1];
		Assert.That(result, Is.EqualTo(2));
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
		Assert.That(arr[0], Is.EqualTo(2));
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
		Assert.That(arr, Has.Count.EqualTo(1));
	}

	[Test]
	public void RemoveLast_WhenNotEmpty_ReturnsLastElement()
	{
		var arr = new ResizeableArray<int>();
		arr.Add(1);
		arr.Add(2);
		var result = arr.RemoveLast();
		Assert.That(result, Is.EqualTo(2));
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
		ResizeableArray<int> list = [0, 1, 2];
		// ReSharper disable once NotAccessedVariable
		int n = 0;
		Assert.That(() => { n = list[4]; }, Throws.Exception);
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
		Assert.That(list, Has.Count.EqualTo(2));
	}

	[Test]
	public void TestRemoveLastThrowsWhenEmpty()
	{
		var list = new ResizeableArray<int>();
		
		Assert.That(() => { list.RemoveLast(); }, Throws.InvalidOperationException);
	}

	[Test]
	public void TestToString()
	{
		ResizeableArray<int> list = [0, 1, 2];
		string str = list.ToString();
		Assert.That(str, Is.EqualTo("[0, 1, 2]"));
	}
}

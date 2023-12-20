namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW.PriorityQueue;
using NUnit.Framework;

[TestFixture, Parallelizable]
public class IndexPriorityQueueTests
{
	private IndexPriorityQueue<int> queue;
	private IComparer<int> comparer;

	[SetUp]
	public void Setup()
	{
		comparer = Comparer<int>.Default;
		queue = new(10, comparer);
	}
	
	[Test]
	public void Insert_And_PopMin_Should_ReturnItemsInPriorityOrder()
	{
		queue.Insert(0, 5);
		queue.Insert(1, 3);
		queue.Insert(2, 4);

		Assert.AreEqual((1, 3), queue.PopMin());
		Assert.AreEqual((2, 4), queue.PopMin());
		Assert.AreEqual((0, 5), queue.PopMin());
	}
	
	[Test]
	public void Change_Should_AffectPopOrder()
	{
		queue.Insert(0, 5);
		queue.Insert(1, 3);
		
		Assert.AreEqual((1, 3), queue.PeekMin());
		
		queue.UpdateValue(0, 2); // Change priority of index 0 to be higher

		Assert.AreEqual((0, 2), queue.PeekMin());
		
		Assert.AreEqual((0, 2), queue.PopMin());
		Assert.AreEqual((1, 3), queue.PopMin());
	}
	
	[Test]
	public void Contains_Should_ReturnTrueForInsertedItems()
	{
		queue.Insert(0, 5);
		queue.Insert(1, 3);

		Assert.IsTrue(queue.Contains(0));
		Assert.IsTrue(queue.Contains(1));
		Assert.IsFalse(queue.Contains(2));
	}
	
	[Test]
	public void IsEmpty_Should_ReturnTrueWhenQueueIsEmpty()
	{
		Assert.IsTrue(queue.IsEmpty);

		queue.Insert(0, 5);
		queue.PopMin();

		Assert.IsTrue(queue.IsEmpty);
	}
}

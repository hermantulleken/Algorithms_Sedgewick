namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW.PriorityQueue;
using NUnit.Framework.Legacy;

[TestFixture, Parallelizable]
public class IndexPriorityQueueTests
{
	private IndexPriorityQueue<int> queue = null!;
	private IComparer<int> comparer = null!;

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

		ClassicAssert.AreEqual((1, 3), queue.PopMin());
		ClassicAssert.AreEqual((2, 4), queue.PopMin());
		ClassicAssert.AreEqual((0, 5), queue.PopMin());
	}
	
	[Test]
	public void Change_Should_AffectPopOrder()
	{
		queue.Insert(0, 5);
		queue.Insert(1, 3);
		
		ClassicAssert.AreEqual((1, 3), queue.PeekMin());
		
		queue.UpdateValue(0, 2); // Change priority of index 0 to be higher

		ClassicAssert.AreEqual((0, 2), queue.PeekMin());
		
		ClassicAssert.AreEqual((0, 2), queue.PopMin());
		ClassicAssert.AreEqual((1, 3), queue.PopMin());
	}
	
	[Test]
	public void Contains_Should_ReturnTrueForInsertedItems()
	{
		queue.Insert(0, 5);
		queue.Insert(1, 3);

		ClassicAssert.IsTrue(queue.Contains(0));
		ClassicAssert.IsTrue(queue.Contains(1));
		ClassicAssert.IsFalse(queue.Contains(2));
	}
	
	[Test]
	public void IsEmpty_Should_ReturnTrueWhenQueueIsEmpty()
	{
		ClassicAssert.IsTrue(queue.IsEmpty);

		queue.Insert(0, 5);
		queue.PopMin();

		ClassicAssert.IsTrue(queue.IsEmpty);
	}
}

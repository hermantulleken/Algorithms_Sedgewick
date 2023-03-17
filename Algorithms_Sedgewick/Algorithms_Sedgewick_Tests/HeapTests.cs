using System;
using Algorithms_Sedgewick.PriorityQueue;
using NUnit.Framework;
using Support;
namespace Algorithms_Sedgewick_Tests;

[Parallelizable]
public class HeapTests
{
	private static Func<IPriorityQueue<int>>[] priorityQueueFactories = 
	{
		() => new FixedCapacityMinBinaryHeap<int>(10),
		() => new FixedCapacityMin3Heap<int>(10),
		() => new PriorityTree<int>(),
		() => new PriorityQueueWithOrderedArray<int>(10),
		() => new PriorityQueueWithUnorderedArray<int>(),
		() => new PriorityQueueWithOrderedLinkedList<int>(),
		() => new PriorityQueueWithUnorderedLinkedList<int>()
	};


	[TestCaseSource(nameof(priorityQueueFactories))]
	public void TestPushPop(Func<IPriorityQueue<int>> queueFactory)
	{
		var heap = queueFactory();
		
		heap.Push(3);
		heap.Push(5);
		heap.Push(1);
		heap.Push(4);
		heap.Push(2);

		var s = heap.Pretty().Log();
		
		Assert.That(heap.PopMin(), Is.EqualTo(1));
		Assert.That(heap.PopMin(), Is.EqualTo(2));
		Assert.That(heap.PopMin(), Is.EqualTo(3));
		Assert.That(heap.PopMin(), Is.EqualTo(4));
		Assert.That(heap.PopMin(), Is.EqualTo(5));
	}
}

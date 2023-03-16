using Algorithms_Sedgewick.PriorityQueue;
using Algorithms_Sedgewick.Sort;
using NUnit.Framework;

namespace Algorithms_Sedgewick_Tests;

public class HeapTests
{
	[Test]
	public void TestPushPop()
	{
		var heap = new FixedCapacityMinBinaryHeap<int>(10);
		
		heap.Push(3);
		heap.Push(5);
		heap.Push(1);
		heap.Push(4);
		heap.Push(2);

		Assert.That(heap.PopMin(), Is.EqualTo(1));
		Assert.That(heap.PopMin(), Is.EqualTo(2));
		Assert.That(heap.PopMin(), Is.EqualTo(3));
		Assert.That(heap.PopMin(), Is.EqualTo(4));
		Assert.That(heap.PopMin(), Is.EqualTo(5));
	}
}

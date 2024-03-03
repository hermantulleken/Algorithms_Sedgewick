namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW.PriorityQueue;
using NUnit.Framework;
using Support;

[Parallelizable]
public class HeapTests
{
	public class Person : IComparable<Person>
	{
		public int Age { get; }
		
		public string Name { get; }

		public Person(string name, int age)
		{
			Name = name;
			Age = age;
		}

		public int CompareTo(Person other) => Age.CompareTo(other.Age);
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void Count_IsZero_ForEmptyPriorityQueue(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		Assert.That(queue.Count, Is.EqualTo(0));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void PeekMin_ReturnsElement_WithoutRemovingIt(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(5);
		var result = queue.PeekMin;
		Assert.That(result, Is.EqualTo(5));
		Assert.That(queue.Count, Is.EqualTo(1));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void PeekMin_ThrowsException_ForEmptyPriorityQueue(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		Assert.Throws<InvalidOperationException>(() => _ = queue.PeekMin);
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void PopMin_RemovesElement_AndDecreasesCount(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(5);
		queue.Push(3);
		queue.Push(7);
		queue.Push(1);
		var result = queue.PopMin();
		Assert.That(result, Is.EqualTo(1));
		Assert.That(queue.Count, Is.EqualTo(3));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void PopMin_RemovesElements_InCorrectOrder(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(5);
		queue.Push(3);
		queue.Push(7);
		queue.Push(1);
		Assert.That(queue.PopMin(), Is.EqualTo(1));
		Assert.That(queue.PopMin(), Is.EqualTo(3));
		Assert.That(queue.PopMin(), Is.EqualTo(5));
		Assert.That(queue.PopMin(), Is.EqualTo(7));
	}

	[TestCaseSource(nameof(personPriorityQueueFactories))]
	public void PopMin_RemovesElements_InCorrectOrder_ForCustomType(Func<IPriorityQueue<Person>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(new Person("Alice", 30));
		queue.Push(new Person("Bob", 25));
		queue.Push(new Person("Charlie", 40));
		Assert.That(queue.PopMin().Name, Is.EqualTo("Bob"));
		Assert.That(queue.PopMin().Name, Is.EqualTo("Alice"));
		Assert.That(queue.PopMin().Name, Is.EqualTo("Charlie"));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void PopMin_ThrowsException_ForEmptyPriorityQueue(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		Assert.Throws<InvalidOperationException>(() => queue.PopMin());
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void Push_AddsElement_AndIncreasesCount(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(5);
		Assert.That(queue.Count, Is.EqualTo(1));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void Push_AddsElement_InCorrectOrder(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(5);
		queue.Push(3);
		queue.Push(7);
		queue.Push(1);
		Assert.That(queue.PeekMin, Is.EqualTo(1));
	}

	[TestCaseSource(nameof(personPriorityQueueFactories))]
	public void Push_AddsElement_InCorrectOrder_ForCustomType(Func<IPriorityQueue<Person>> queueFactory)
	{
		var queue = queueFactory();
		queue.Push(new Person("Alice", 30));
		queue.Push(new Person("Bob", 25));
		queue.Push(new Person("Charlie", 40));
		Assert.That(queue.PeekMin.Name, Is.EqualTo("Bob"));
	}

	[TestCaseSource(nameof(personPriorityQueueFactories))]
	public void Push_NullElement_ThrowsException(Func<IPriorityQueue<Person>> queueFactory)
	{
		var queue = queueFactory();
		Assert.Throws<ArgumentNullException>(() => queue.Push(null!));
	}

	[TestCaseSource(nameof(intPriorityQueueFactories))]
	public void TestPushPop(Func<IPriorityQueue<int>> queueFactory)
	{
		var queue = queueFactory();
		
		queue.Push(3);

		queue.Log();
		
		queue.Push(5);
		queue.Log();
		
		queue.Push(1);
		queue.Log();
		
		queue.Push(4);
		queue.Log();
		
		queue.Push(2);
		queue.Log();
		
		Assert.That(queue.PeekMin, Is.EqualTo(1));
		Assert.That(queue.PopMin(), Is.EqualTo(1));
		
		Assert.That(queue.PeekMin, Is.EqualTo(2));
		Assert.That(queue.PopMin(), Is.EqualTo(2));
		
		Assert.That(queue.PeekMin, Is.EqualTo(3));
		Assert.That(queue.PopMin(), Is.EqualTo(3));
		
		Assert.That(queue.PeekMin, Is.EqualTo(4));
		Assert.That(queue.PopMin(), Is.EqualTo(4));
		
		Assert.That(queue.PeekMin, Is.EqualTo(5));
		Assert.That(queue.PopMin(), Is.EqualTo(5));
	}

	[Test]
	public void TestGrow()
	{
		var priorityQueue = new ResizeableMinBinaryHeap<int>(10, Comparer<int>.Default);

		const int itemCount = 10;
		
		for (int i = itemCount; i >= 0; i--)
		{
			priorityQueue.Push(i);
		}

		for (int i = 0; i < itemCount; i++)
		{
			int item = priorityQueue.PeekMin;
			Assert.That(item, Is.EqualTo(i));
			item = priorityQueue.PopMin();
			Assert.That(item, Is.EqualTo(i));
		}
	}

	private static Func<IPriorityQueue<int>>[] intPriorityQueueFactories = 
	{
		() => new FixedCapacityMinBinaryHeap<int>(10, Comparer<int>.Default),
		() => new FixedCapacityMin3Heap<int>(10, Comparer<int>.Default),
		() => new FixedCapacityMinNHeap<int>(5, 10, Comparer<int>.Default),
		() => new PriorityTree<int>(Comparer<int>.Default),
		() => new PriorityQueueWithOrderedArray<int>(10, Comparer<int>.Default),
		() => new PriorityQueueWithUnorderedArray<int>(Comparer<int>.Default),
		() => new PriorityQueueWithOrderedLinkedList<int>(Comparer<int>.Default),
		() => new PriorityQueueWithUnorderedLinkedList<int>(Comparer<int>.Default),
		() => new ResizeableMinBinaryHeap<int>(10, Comparer<int>.Default),
	};

	private static Func<IPriorityQueue<Person>>[] personPriorityQueueFactories = 
	{
		() => new FixedCapacityMinBinaryHeap<Person>(10, Comparer<Person>.Default),
		() => new FixedCapacityMin3Heap<Person>(10, Comparer<Person>.Default),
		() => new FixedCapacityMinNHeap<Person>(5, 10, Comparer<Person>.Default),
		() => new PriorityTree<Person>(Comparer<Person>.Default),
		() => new PriorityQueueWithOrderedArray<Person>(10, Comparer<Person>.Default),
		() => new PriorityQueueWithUnorderedArray<Person>(Comparer<Person>.Default),
		() => new PriorityQueueWithOrderedLinkedList<Person>(Comparer<Person>.Default),
		() => new PriorityQueueWithUnorderedLinkedList<Person>(Comparer<Person>.Default),
		() => new ResizeableMinBinaryHeap<Person>(10, Comparer<Person>.Default),
	};
}

namespace Algorithms_Sedgewick_Tests;

using Algorithms_Sedgewick.PriorityQueue;
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

	private static Func<IPriorityQueue<int>>[] intPriorityQueueFactories = 
	{
		() => new FixedCapacityMinBinaryHeap<int>(10),
		() => new FixedCapacityMin3Heap<int>(10),
		() => new PriorityTree<int>(),
		() => new PriorityQueueWithOrderedArray<int>(10),
		() => new PriorityQueueWithUnorderedArray<int>(),
		() => new PriorityQueueWithOrderedLinkedList<int>(),
		() => new PriorityQueueWithUnorderedLinkedList<int>(),
	};

	private static Func<IPriorityQueue<Person>>[] personPriorityQueueFactories = 
	{
		() => new FixedCapacityMinBinaryHeap<Person>(10),
		() => new FixedCapacityMin3Heap<Person>(10),
		() => new PriorityTree<Person>(),
		() => new PriorityQueueWithOrderedArray<Person>(10),
		() => new PriorityQueueWithUnorderedArray<Person>(),
		() => new PriorityQueueWithOrderedLinkedList<Person>(),
		() => new PriorityQueueWithUnorderedLinkedList<Person>(),
	};
}

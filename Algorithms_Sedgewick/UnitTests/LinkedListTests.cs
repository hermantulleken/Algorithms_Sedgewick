namespace UnitTests;

using Algorithms_Sedgewick.List;
using NUnit.Framework;

[Parallelizable]
public class LinkedListTests
{
	private const int ArbitraryElement = 100;

	[Test]
	public void TestEmptyListCountIsZero()
	{
		var list = new LinkedList<int>();
		Assert.That(list.Count, Is.EqualTo(0));
	}

	[Test]
	public void TestInsertAtBack()
	{
		var list = new LinkedList<int>();
		list.InsertAtBack(ArbitraryElement);
		
		Assert.That(list.Count, Is.EqualTo(1));
	
		Assert.That(list.First.Item, Is.EqualTo(ArbitraryElement));
		Assert.That(list.Last.Item, Is.EqualTo(ArbitraryElement));
	}

	[Test]
	public void TestInsertAtBackTwice()
	{
		var list = new LinkedList<int>();
		list.InsertAtBack(200);
		list.InsertAtBack(300);
	
		Assert.That(list.Count, Is.EqualTo(2));
	
		Assert.That(list.First.Item, Is.EqualTo(200));
		Assert.That(list.Last.Item, Is.EqualTo(300));
	}

	[Test]
	public void TestInsertAtFront()
	{
		var list = new LinkedList<int>();
		list.InsertAtFront(ArbitraryElement);
		
		Assert.That(list.Count, Is.EqualTo(1));
		
		Assert.That(list.First.Item, Is.EqualTo(ArbitraryElement));
		Assert.That(list.Last.Item, Is.EqualTo(ArbitraryElement));
	}

	[Test]
	public void TestInsertAtFrontTwice()
	{
		var list = new LinkedList<int>();
		list.InsertAtFront(200);
		list.InsertAtFront(300);
		
		Assert.That(list.Count, Is.EqualTo(2));
		
		Assert.That(list.First.Item, Is.EqualTo(300));
		Assert.That(list.Last.Item, Is.EqualTo(200));
	}

	[Test]
	public void TestRemoveFromFront()
	{
		var list = new LinkedList<int>();
		list.InsertAtBack(200);
		list.InsertAtBack(300);

		int front = list.RemoveFromFront().Item;
		
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(front, Is.EqualTo(200));
		Assert.That(list.First.Item, Is.EqualTo(300));
	}

	[Test]
	public void TestRemoveFromFrontTwice()
	{
		var list = new LinkedList<int>();
		list.InsertAtBack(200);
		list.InsertAtBack(300);

		list.RemoveFromFront();
		int front = list.RemoveFromFront().Item;
		
		Assert.That(list.Count, Is.EqualTo(0));
		Assert.That(front, Is.EqualTo(300));
	}

	[Test]
	public void TestRemoveAfter()
	{
		var list = new LinkedList<int>();
		var node1 = list.InsertAtFront(1);
		var node2 = list.InsertAtFront(2);
		var node3 = list.InsertAtFront(3);

		var removedNode = list.RemoveAfter(node2);
		
		Assert.That(removedNode, Is.EqualTo(node1));
		Assert.That(list.Count, Is.EqualTo(2));
		Assert.That(list.First, Is.EqualTo(node3));
		Assert.That(list.Last, Is.EqualTo(node2));
	}
}

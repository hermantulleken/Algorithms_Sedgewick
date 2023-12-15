using AlgorithmsSW.List;
using NUnit.Framework;

namespace UnitTests;

[Parallelizable]
public class LinkedListWithPooledNodes
{
	private const int ArbitraryElement = 100;

	[Test]
	public void TestEmptyListCountIsZero()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		Assert.That(list.Count, Is.EqualTo(0));
	}

	[Test]
	public void TestInsertAtBack()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtBack(ArbitraryElement);
		
		Assert.That(list.Count, Is.EqualTo(1));
	
		Assert.That(list.First.Item, Is.EqualTo(ArbitraryElement));
		Assert.That(list.Last.Item, Is.EqualTo(ArbitraryElement));
	}

	[Test]
	public void TestInsertAtBackTwice()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtBack(200);
		list.InsertAtBack(300);
	
		Assert.That(list.Count, Is.EqualTo(2));
	
		Assert.That(list.First.Item, Is.EqualTo(200));
		Assert.That(list.Last.Item, Is.EqualTo(300));
	}

	[Test]
	public void TestInsertAtFront()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtFront(ArbitraryElement);
		
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(list.First.Item, Is.EqualTo(ArbitraryElement));
		Assert.That(list.Last.Item, Is.EqualTo(ArbitraryElement));
	}

	[Test]
	public void TestInsertAtFrontTwice()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtFront(200);
		list.InsertAtFront(300);
		
		Assert.That(list.Count, Is.EqualTo(2));
		
		Assert.That(list.First.Item, Is.EqualTo(300));
		Assert.That(list.Last.Item, Is.EqualTo(200));
	}

	[Test]
	public void TestRemoveFromFront()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtBack(200);
		list.InsertAtBack(300);

		list.RemoveFromFront();
		
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(list.First.Item, Is.EqualTo(300));
	}

	[Test]
	public void TestRemoveFromFrontTwice()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		list.InsertAtBack(200);
		list.InsertAtBack(300);

		list.RemoveFromFront();
		list.RemoveFromFront();
		
		Assert.That(list.Count, Is.EqualTo(0));
	}

	[Test]
	public void TestRemoveAfter()
	{
		var list = new LinkedListWithPooledNodes<int>(10);
		var node1 = list.InsertAtFront(1);
		var node2 = list.InsertAtFront(2);
		var node3 = list.InsertAtFront(3);

		list.RemoveAfter(node2);
		
		Assert.That(list.Count, Is.EqualTo(2));
		Assert.That(list.First, Is.EqualTo(node3));
		Assert.That(list.Last, Is.EqualTo(node2));
	}
}

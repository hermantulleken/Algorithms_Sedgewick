
using System.Linq;
using Algorithms_Sedgewick;
using NUnit.Framework;

namespace Algorithms_Sedgewick_Tests;

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
	public void TestRemoveFromFront()
	{
		var list = new LinkedList<int>();
		list.InsertAtBack(200);
		list.InsertAtBack(300);

		int front = list.RemoveFromFront().Item;
		
		Assert.That(list.Count, Is.EqualTo(1));
		Assert.That(front , Is.EqualTo(200));
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
		Assert.That(front , Is.EqualTo(300));
	}
}

using System.Linq;

namespace UnitTests;

using AlgorithmsSW.List;
using NUnit.Framework;

[TestFixture]
public class CircularLinkedListTests
{
	[Test]
	public void TestIsEmpty()
	{
		var list = new CircularLinkedList<int>();
		Assert.That(list.IsEmpty, Is.True);
		list.InsertAtFront(1);
		Assert.That(list.IsEmpty, Is.False);
	}

	[Test]
	public void TestIsSingleton()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtFront(1);
		Assert.That(list.IsSingleton, Is.True);
		list.InsertAtFront(2);
		Assert.That(list.IsSingleton, Is.False);
	}

	[Test]
	public void TestInsertAtFront()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtFront(1);
		list.InsertAtFront(2);
		
		Assert.That(list.First.Item, Is.EqualTo(2));
		Assert.That(list.First.NextNode.Item, Is.EqualTo(1));
	}

	[Test]
	public void TestInsertAtBack()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtBack(1);
		list.InsertAtBack(2);
		Assert.That(list.First.Item, Is.EqualTo(1));
		Assert.That(list.First.NextNode.Item, Is.EqualTo(2));
	}

	[Test]
	public void TestNodes()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtFront(1);
		list.InsertAtFront(2);
		list.InsertAtFront(3);

		var nodes = list.Nodes.Take(list.Count).ToList(); // We take the first count since this list is circular
		Assert.That(nodes.Count, Is.EqualTo(3));
		Assert.That(nodes[0].Item, Is.EqualTo(3));
		Assert.That(nodes[1].Item, Is.EqualTo(2));
		Assert.That(nodes[2].Item, Is.EqualTo(1));
	}

	[Test]
	public void TestInsertAfter()
	{
		var list = new CircularLinkedList<int>();
		var firstNode = list.InsertAtFront(1);
		list.InsertAfter(firstNode, 2);
		Assert.That(firstNode.NextNode.Item, Is.EqualTo(2));
	}

	[Test]
	public void TestRemoveAfter()
	{
		var list = new CircularLinkedList<int>();
		var firstNode = list.InsertAtFront(1);
		list.InsertAtFront(2);
		list.RemoveAfter(firstNode);
		Assert.That(firstNode.NextNode.Item, Is.EqualTo(1)); // Because of the circular nature
	}

	[Test]
	public void TestRemoveFromFront()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtFront(1);
		list.InsertAtFront(2);
		list.RemoveFromFront();
		Assert.That(list.First.Item, Is.EqualTo(1));
	}

	[Test]
	public void TestClear()
	{
		var list = new CircularLinkedList<int>();
		list.InsertAtFront(1);
		list.Clear();
		Assert.That(list.IsEmpty, Is.True);
	}

	[Test]
	public void TestRemoveAfterOnSingletonList()
	{
		var list = new CircularLinkedList<int>();
		var firstNode = list.InsertAtFront(1);
		list.RemoveAfter(firstNode);
		Assert.That(list.IsEmpty, Is.True);
	}

	[Test]
	public void TestRemoveFromFrontOnEmptyList()
	{
		var list = new CircularLinkedList<int>();
		Assert.Throws<InvalidOperationException>(() => list.RemoveFromFront());
	}
}

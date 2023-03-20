using System.Diagnostics;
using static Algorithms_Sedgewick.ThrowHelper;

namespace Algorithms_Sedgewick.PriorityQueue;

//Ex. 2.4.3
public sealed class PriorityQueueWithUnorderedLinkedList<T> : IPriorityQueue<T> where T : IComparable<T>
{
	private readonly List.LinkedList<T> items = new();

	public int Count => items.Count;

	public T PeekMin
	{
		get
		{
			if (this.IsEmpty())
			{
				ThrowContainerEmpty();
			}

			return items.First.Item;
		}
	}

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var minNode = items.RemoveFromFront();
		if (Count > 1)
		{
			MoveMinToFront();
		}
		
		return minNode.Item;
	}

	public void Push(T item)
	{
		item.ThrowIfNull();

		items.InsertAtFront(item);
		if (Count > 1)
		{
			MoveMinToFront();
		}
	}

	private List.LinkedList<T>.Node GetNodeBeforeMinNode()
	{
		Debug.Assert(Count > 1);

		List.LinkedList<T>.Node nodeBeforeMinNode = null;
		var minNode = items.First;
		
		foreach (var node in items.Nodes)
		{
			if(node.NextNode != null && Sort.Less(node.NextNode.Item, minNode.Item))
			{
				nodeBeforeMinNode = node;
				minNode = node.NextNode;
			}
		}

		return nodeBeforeMinNode;
	}

	private void MoveMinToFront()
	{
		Debug.Assert(Count > 1);
		var nodeBeforeMinNode = GetNodeBeforeMinNode();

		if (nodeBeforeMinNode != null)
		{
			var minNode = items.RemoveAfter(nodeBeforeMinNode);
			items.InsertAtFront(minNode.Item);
		}
		//else minNode is at the front already. 
	}
}

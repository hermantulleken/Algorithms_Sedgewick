namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.Debug;
using static Support.Tools;

public sealed class CircularLinkedList<T> : IEnumerable<T>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = DataTransferStruct)]
	public sealed record Node
	{
		public readonly T Item;
		public Node NextNode = null!;
		
		// To have the list be circular, we cannot always construct this field in the constructor.
		// See: InsertFirstItem
		
		public Node(T item, Node nextNode = null!)
		{
			Item = item;
			NextNode = nextNode;
		}
	}

	private Node? front;

	private int version = 0;

	public int Count { get; private set; } = 0;

	public Node First
	{
		get
		{
			ValidateNotEmpty();
			Assert(front != null);
				
			return front;
		}
	}

	[MemberNotNullWhen(false, nameof(front))]
	public bool IsEmpty => front == null;

	public bool IsSingleton => !IsEmpty && front!.NextNode == front;

	public IEnumerable<Node> Nodes
	{
		get
		{
			if (IsEmpty)
			{
				yield break;
			}
			
			Assert(front != null);
			
			var current = front;
			int versionAtStartOfIteration = version;
			
			do
			{
				ValidateVersion(versionAtStartOfIteration);
				yield return current;
				current = current.NextNode;
			}
			while (current != front);
		}
	}

	public void Clear()
	{
		front = null;
		Count = 0;
		version++;
	}

	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	public Node InsertAfter(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		var newNode = new Node(item, node.NextNode);

		node.NextNode = newNode;

		return newNode;
	}

	public void InsertAtBack(T item)
	{
		InsertAtFront(item);
		Assert(front != null);
		front = front.NextNode; // Rotate so that new item is at the back
	}

	public Node InsertAtFront(T item)
	{
		if (IsEmpty)
		{
			return InsertFirstItem(item);
		}

		Assert(front != null);

		var newHead = new Node(item, front);
		front = newHead;
		Count++;
		version++;

		return front;
	}

	public Node RemoveAfter(Node node)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		if (node.NextNode == First)
		{
			return RemoveFromFront();
		}

		Assert(!IsSingleton); // otherwise would have executed the block above
		
		var newNextNode = node.NextNode.NextNode;
		var removedNode = node.NextNode;

		node.NextNode = newNextNode;
		
		return removedNode;
	}

	public Node RemoveFromFront()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		Assert(front != null);
		var removedNode = front;
		
		if (IsSingleton)
		{
			front = null;
			return removedNode;
		}

		front = front.NextNode;
		Count--;
		version++;
			
		return removedNode;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private Node InsertFirstItem(T item)
	{
		front = new Node(item);
		front.NextNode = front;
		
		return front;
	}

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}

	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			ThrowHelper.ThrowIteratingOverModifiedContainer();
		}
	}
}

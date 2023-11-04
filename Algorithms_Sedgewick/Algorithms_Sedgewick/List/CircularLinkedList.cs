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
	private Node? back;

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

	public Node Back
	{
		get
		{
			ValidateNotEmpty();
			Assert(back != null);

			return back;
		}
	}

	[MemberNotNullWhen(false, nameof(front))]
	[MemberNotNullWhen(false, nameof(back))]
	public bool IsEmpty => front == null;

	public bool IsSingleton => !IsEmpty && front == back;

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
		back = null;
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
		
		Count++;
		version++;

		if (node == back)
		{
			back = newNode;
		}

		return newNode;
	}

	public void InsertAtBack(T item)
	{
		if (IsEmpty)
		{
			InsertFirstItem(item);
		}
		else
		{
			InsertAfter(back, item);
		}
	}

	public Node InsertAtFront(T item)
	{
		if (IsEmpty)
		{
			return InsertFirstItem(item);
		}

		Assert(front != null);

		var oldBack = back;
		var newNode = InsertAfter(back, item);
		front = back;
		back = oldBack;
		return newNode;
	}
	
	public void RotateLeft()
	{
		if (IsEmpty)
		{
			return;
		}

		Assert(front != null);

		var oldFront = front;
		front = front.NextNode;
		back = oldFront;
	}

	public Node RemoveAfter(Node node)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		if (node == back)
		{
			return RemoveFromFront();
		}

		Assert(!IsSingleton); // otherwise would have executed the block above
		
		var newNextNode = node.NextNode.NextNode;
		var removedNode = node.NextNode;

		node.NextNode = newNextNode;
		Count--;
		version++;
		
		return removedNode;
	}

	public Node RemoveFromFront()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		if (IsSingleton)
		{
			var removedNode = front;
			Clear();
			return removedNode;
		}

		var oldFront = front;
		front = front.NextNode;
		back.NextNode = front;
		
		Count--;
		version++;
		return oldFront;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private Node InsertFirstItem(T item)
	{
		Assert(IsEmpty);
		Assert(Count == 0);
		
		front = back = new Node(item);
		front.NextNode = front;
		Count++;
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

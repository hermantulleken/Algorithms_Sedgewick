using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.List;

[ExerciseReference(1, 3, 31)]
public sealed class DoublyLinkedList<T> : IEnumerable<T>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/
	[SuppressMessage(
		"StyleCop.CSharp.MaintainabilityRules", 
		"SA1401:Fields should be private", 
		Justification = Tools.DataTransferStruct)]
	public sealed record Node(T Item)
	{
		public T Item = Item;
		
		public Node? NextNode;
		
		public Node? PreviousNode;
	}

	private Node? back;

	private Node? front;
	
	private int version = 0;

	public int Count
	{
		get; 
		private set;
	} = 0;

	public Node First
	{
		get
		{
			ValidateNotEmpty();
			Assert(front != null);
				
			return front;
		}
	}

	[MemberNotNullWhen(false, nameof(front), nameof(back))]
	public bool IsEmpty => front == null;
	
	public bool IsSingleton => front == back;

	public Node Last
	{
		get
		{
			ValidateNotEmpty();
			Assert(back != null);
				
			return back;
		}
	}

	public IEnumerable<Node> Nodes
	{
		get
		{
			// Also work when empty, since front will be null
			var current = front;
			int versionAtStartOfIteration = version;
			
			while (current != null)
			{
				ValidateVersion(versionAtStartOfIteration);
				yield return current;
				current = current.NextNode;
			}
		}
	}

	public void Clear()
	{
		front = back = null;
		Count = 0;
		version++;
	}

	public void Concat(DoublyLinkedList<T> other)
	{
		if (other.IsEmpty)
		{
			// Nothing to do
			return;
		}
		
		var otherFront = other.front;
		var otherBack = other.back;
		int otherCount = other.Count;
		
		other.Clear(); // Clear so there is no nodes part of both lists

		if (!IsEmpty)
		{
			back.NextNode = otherFront;
			otherFront.PreviousNode = back;
			back = otherBack;
		}
		else
		{
			front = otherFront;
			back = otherBack;
		}
		
		Count += otherCount;
		version++;
	}

	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	public Node InsertAfter(Node node, T item)
	{
		node.ThrowIfNull();

		return InsertNode(node, node.NextNode, item);
	}

	/* So we can use collection initializers and expressions. */
	public void Add(T item) => InsertAtBack(item);

	public Node InsertAtBack(T item)
	{
		if (IsEmpty)
		{
			Count++;
			version++;
			return InsertFirstItem(item);
		}

		back.NextNode = new Node(item) { PreviousNode = back };
		back = back.NextNode;

		Count++;
		version++;

		return back;
	}

	public Node InsertAtFront(T item)
	{
		if (IsEmpty)
		{
			Count++;
			version++;
			return InsertFirstItem(item);
		}

		var newHead = new Node(item)
		{
			NextNode = front,
		};

		front.PreviousNode = newHead;
		front = newHead;

		Count++;
		version++;

		return front;
	}

	public Node InsertBefore(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		return InsertNode(node.PreviousNode, node, item);
	}

	public Node RemoveAfter(Node node)
	{
		node.ThrowIfNull();

		if (node.NextNode == null)
		{
			throw new Exception("Nothing to remove.");
		}
		
		return RemoveNode(node.NextNode);
	}

	public Node RemoveBefore(Node node)
	{
		node.ThrowIfNull();
		
		if (node.PreviousNode == null)
		{
			throw new Exception("Nothing to remove.");
		}

		return RemoveNode(node.PreviousNode);
	}

	public Node RemoveFromBack()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var removedNode = back;

		if (IsSingleton)
		{
			front = back = null;
			Count--;
			version++;
			return removedNode;
		}

		back = back.PreviousNode!; // back has a previous node since the list is not empty nor a singleton
		back.NextNode = null;
		
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

		var removedNode = front;

		if (IsSingleton)
		{
			front = back = null;
			Count--;
			version++;
			return removedNode;
		}
		
		front = front.NextNode!; // front has a NextNode since the list is not empty or a singleton
		front.PreviousNode = null;
		Count--;
		version++;

		return removedNode;
	}

	public Node RemoveNode(Node nodeToRemove)
	{
		nodeToRemove.ThrowIfNull();
		
		var nodeBeforeRemoval = nodeToRemove.PreviousNode;

		if (nodeBeforeRemoval == null)
		{
			return RemoveFromFront();
		}

		var nodeAfterRemoval = nodeToRemove.NextNode;
		if (nodeAfterRemoval == null)
		{
			return RemoveFromBack();
		}

		nodeBeforeRemoval.NextNode = nodeAfterRemoval;
		nodeAfterRemoval.PreviousNode = nodeBeforeRemoval;

		return nodeToRemove;
	}

	public void Reverse()
	{
		if (IsEmpty || IsSingleton)
		{
			return; // Nothing to do
		}
		
		var current = front;
		var oldFront = front;
		var oldBack = back;

		while (current != null)
		{
			(current.PreviousNode, current.NextNode) = (current.NextNode, current.PreviousNode);
			current = current.PreviousNode; // This is the next node in the original order
		}
		
		front = oldBack;
		back = oldFront;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private Node InsertFirstItem(T item) => front = back = new Node(item);

	private Node InsertNode(Node? nodeBeforeInsertion, Node? nodeAfterInsertion, T item)
	{
		if (nodeBeforeInsertion == null)
		{
			return InsertAtFront(item);
		}

		if (nodeAfterInsertion == null)
		{
			return InsertAtBack(item);
		}

		var newNode = new Node(item)
		{
			PreviousNode = nodeBeforeInsertion,
			NextNode = nodeAfterInsertion,
		};

		nodeBeforeInsertion.NextNode = newNode;
		nodeAfterInsertion.PreviousNode = newNode;
		
		return newNode;
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

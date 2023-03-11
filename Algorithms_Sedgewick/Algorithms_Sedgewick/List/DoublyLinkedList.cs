using System.Collections;
using System.Diagnostics;

namespace Algorithms_Sedgewick;

public sealed class DoublyLinkedList<T> : IEnumerable<T>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/ 
	public sealed record Node
	{
		public T Item;
		public Node PreviousNode;
		public Node NextNode;
	}

	private Node front;
	private Node back;
	private int version = 0;

	public bool IsEmpty => front == null;
	public bool IsSingleton => front == back;

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
			Debug.Assert(front != null);
				
			return front;
		}
	}
		
	public Node Last
	{
		get
		{
			ValidateNotEmpty();
			Debug.Assert(back != null);
				
			return back;
		}
	}

	public Node InsertAtBack(T item)
	{
		if (IsEmpty)
		{
			Count++;
			version++;
			return InsertFirstItem(item);
		}

		back.NextNode = new Node{Item = item, PreviousNode = back};
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

		var newHead = new Node
		{
			Item = item,
			NextNode = front
		};

		front.PreviousNode = newHead;
		front = newHead;

		Count++;
		version++;

		return front;
	}

	public Node RemoveFromFront()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}

		var removedNode = front;

		if (IsSingleton)
		{
			front = back = null;
			Count--;
			version++;
			return removedNode;
		}

		
		front = front.NextNode;
		front.PreviousNode = null;
		Count--;
		version++;

		return removedNode;
	}

	public Node RemoveFromBack()
	{
		if(IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}
		
		var removedNode = back;

		if (IsSingleton)
		{
			front = back = null;
			Count--;
			version++;
			return removedNode;
		}

		back = back.PreviousNode;
		back.NextNode = null;
		
		Count--;
		version++;

		return removedNode;
	}

	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	public IEnumerable<Node> Nodes
	{
		get
		{
			//Also work when empty, since front will be null
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

	public Node InsertAfter(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		return InsertNode(node, node.NextNode, item);
	}
	
	public Node InsertBefore(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		return InsertNode(node.PreviousNode, node, item);
	}

	private Node InsertNode(Node nodeBeforeInsertion, Node nodeAfterInsertion, T item)
	{
		if (nodeBeforeInsertion == null)
		{
			return InsertAtFront(item);
		}

		if (nodeAfterInsertion == null)
		{
			return InsertAtBack(item);
		}

		var newNode = new Node
		{
			Item = item,
			PreviousNode = nodeBeforeInsertion,
			NextNode = nodeAfterInsertion,
		};

		nodeBeforeInsertion.NextNode = newNode;
		nodeAfterInsertion.PreviousNode = newNode;
		
		return newNode;
	}

	public Node RemoveBefore(Node node)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		return RemoveNode(node.PreviousNode);
	}
	
	public Node RemoveAfter(Node node)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		return RemoveNode(node.NextNode);
	}

	public Node RemoveNode(Node nodeToRemove)
	{
		if (nodeToRemove == null)
		{
			throw new Exception("No node to remove.");
		}
		
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
			return; //Nothing to do
		}
		
		var current = front;
		var oldFront = front;
		var oldBack = back;

		while (current != null)
		{
			(current.PreviousNode, current.NextNode) = (current.NextNode, current.PreviousNode);
			current = current.PreviousNode; //This is the next node in the original order
		}
		
		front = oldBack;
		back = oldFront;
	}

	public void Clear()
	{
		front = back = null;
		Count = 0;
		version++;
	}
	
	public void Concat(DoublyLinkedList<T> other)
	{
		var otherFront = other.front;
		var otherBack = other.back;
		int otherCount = other.Count;
		
		other.Clear(); //Clear so there is no nodes part of both lists
		
		back.NextNode = otherFront;
		otherFront.PreviousNode = back;
		back = otherBack;
		Count += otherCount;
		version++;
	}
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private Node InsertFirstItem(T item) => front = back = new Node { Item = item};
		
	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			throw new InvalidOperationException(ContainerErrorMessages.IteratingOverModifiedList);
		}
	}
		
	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}
	}
}

using System.Collections;
using System.Diagnostics;

namespace Algorithms_Sedgewick;

public sealed class CircularLinkedList<T> : IEnumerable<T>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/ 
	public sealed record Node
	{
		public T Item;
		public Node NextNode;
	}

	private Node front;

	private int version = 0;

	public bool IsEmpty => front == null;
	public bool IsSingleton => front.NextNode == front;
	public int Count { get; private set; } = 0;

	public Node First
	{
		get
		{
			ValidateNotEmpty();
			Debug.Assert(front != null);
				
			return front;
		}
	}
	

	public Node InsertAtFront(T item)
	{
		if (IsEmpty)
		{
			return InsertFirstItem(item);
		}

		var newHead = new Node
		{
			Item = item,
			NextNode = front
		};
			
		front = newHead;

		Count++;
		version++;

		return front;
	}
	
	public void InsertAtBack(T item)
	{
		InsertAtFront(item);
		front = front.NextNode; //Rotate so that new item is at the back
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
			front = null;
			return removedNode;
		}
		
		
		front = front.NextNode;
		Count--;
		version++;
			
		return removedNode;
	}

	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	public IEnumerable<Node> Nodes
	{
		get
		{
			if (IsEmpty)
			{
				yield break;
			}
			
			var current = front;
			int versionAtStartOfIteration = version;
			
			do
			{
				ValidateVersion(versionAtStartOfIteration);
				yield return current;
				current = current.NextNode;
			} while (current != front);
		}
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
		
		Debug.Assert(!IsSingleton); //otherwise would have executed the block above
		
		var newNextNode = node.NextNode.NextNode;
		var removedNode = node.NextNode;

		node.NextNode = newNextNode;
		
		return removedNode;
	}
	
	public Node InsertAfter(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		var newNode = new Node
		{
			Item = item,
			NextNode = node.NextNode
		};

		node.NextNode = newNode;

		return newNode;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private Node InsertFirstItem(T item)
	{
		front = new Node { Item = item };
		front.NextNode = front;
		
		return front;
	}

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

	public void Clear()
	{
		front = null;
		Count = 0;
		version++;
	}
}

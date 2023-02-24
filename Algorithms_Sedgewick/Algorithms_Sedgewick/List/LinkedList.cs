using System.Collections;
using System.Diagnostics;

namespace Algorithms_Sedgewick;

public sealed class LinkedList<T> : IEnumerable<T>
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
	private Node back;
	private int version = 0;

	public bool IsEmpty => front == null;
	public bool IsSingleton => front == back;
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
			return InsertFirstItem(item);
		}

		back.NextNode = new Node{Item = item};
		back = back.NextNode;

		Count++;
		version++;

		return back;
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

	public Node RemoveFromFront()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}

		var removedNode = front;
		//Also works when front is the last node, since then NextNode is null.
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

	public Node RemoveAfter(Node node)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		if (node.NextNode == null)
		{
			throw new Exception("No node after");
		}

		//Also works if node.NextNode.NextNode is null
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
	
	public void Reverse()
	{
		if (IsEmpty || IsSingleton)
		{
			return; //Nothing to do
		}
		
		var first = front;
		var oldFront = front;
		Node reverse = null;
		
		while (first != null)
		{
			var second = first.NextNode;
			first.NextNode = reverse;
			reverse = first;
			first = second;
		}
		
		front = reverse;
		back = oldFront;
	}
	
	public void Clear()
	{
		front = back = null;
		Count = 0;
		version++;
	}

	public void Concat(LinkedList<T> other)
	{
		var otherFront = other.front;
		var otherBack = other.back;
		int otherCount = other.Count;
		
		other.Clear(); //Clear so there is no nodes part of both lists
		
		back.NextNode = otherFront;
		back = otherBack;
		Count += otherCount;
		version++;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private Node InsertFirstItem(T item) => front = back = new Node { Item = item };
		
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

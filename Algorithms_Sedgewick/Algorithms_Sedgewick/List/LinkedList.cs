﻿namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static Support.Tools;

public sealed class LinkedList<T> : IEnumerable<T?>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/ 
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = DataTransferStruct)]
	public sealed record Node
	{
		#if WHITEBOXTESTING
		private const int RecursiveStringLimit = 100;
		#endif
		
		public T? Item;
		public Node? NextNode;
		
		public IEnumerable<Node> Rest
		{
			get
			{
				var node = this;
				while (node.NextNode != null)
				{
					yield return node.NextNode;
					node = node.NextNode;
				}
			}
		}
		
#if WHITEBOXTESTING
		public override string ToString() => ToDebugString();

		public string ToDebugString() => ToDebugString(RecursiveStringLimit);

		private string ToDebugString(int depth) =>
			NextNode == null 
				? ItemString!
				: depth == 0 
					? "..." 
					: $"{Item} [{NextNode.ToDebugString(depth - 1)}]";

		private string? ItemString => Item == null ? "null" : Item.ToString();
#else
		public override string ToString() => $"Node:{{{ItemString}}}";
#endif
	}

	private Node? front;
	private Node? back;
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
			Count++;
			version++;
			
			return InsertFirstItem(item);
		}

		Debug.Assert(back != null);
		back.NextNode = new Node { Item = item };
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
			NextNode = front,
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
			ThrowHelper.ThrowContainerEmpty();
		}

		var removedNode = front!;
		
		// Also works when front is the last node, since then NextNode is null.
		front = front!.NextNode;
		Count--;
		version++;
			
		return removedNode;
	}

	public IEnumerator<T?> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

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

	/// <summary>
	/// Removes the node after the specified node.
	/// </summary>
	/// <param name="node">The node whose successor is to be removed.</param>
	/// <returns>The removed node.</returns>
	/// <exception cref="System.ArgumentNullException">Thrown when the input node is null.</exception>
	/// <exception cref="System.InvalidOperationException">Thrown when the input node does not have a successor to remove.</exception>
	public Node RemoveAfter(Node node)
	{
		node.ThrowIfNull();
		
		if (node.NextNode == null)
		{
			throw new InvalidOperationException("There is no node after the given node that can be removed.");
		}

		// Also works if node.NextNode.NextNode is null
		var newNextNode = node.NextNode.NextNode;
		var removedNode = node.NextNode;

		node.NextNode = newNextNode;
		Count--;
		version++;
		return removedNode;
	}
	
	public Node InsertAfter(Node node, T item)
	{
		if (node == null)
		{
			throw new ArgumentNullException(nameof(node));
		}

		if (node == back)
		{
			return InsertAtBack(item);
		}
		
		var newNode = new Node
		{
			Item = item,
			NextNode = node.NextNode,
		};

		node.NextNode = newNode;
		
		Count++;
		version++;

		return newNode;
	}
	
	public void Reverse()
	{
		if (IsEmpty || IsSingleton)
		{
			return; // Nothing to do
		}
		
		var first = front;
		var oldFront = front;
		Node? reverse = null;
		
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
		
		other.Clear(); // Clear so there is no nodes part of both lists

		if (back == null)
		{
			front = otherFront;
		}
		else
		{
			back.NextNode = otherFront;
		}
		
		back = otherBack;
		Count += otherCount;
		version++;
	}

	public override string ToString() => First.ToString();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private Node InsertFirstItem(T item) => front = back = new Node { Item = item };
		
	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			ThrowHelper.ThrowIteratingOverModifiedContainer();
		}
	}
		
	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

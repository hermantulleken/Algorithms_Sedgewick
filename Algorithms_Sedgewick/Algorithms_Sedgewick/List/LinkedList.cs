using System.Diagnostics;
using Support;
using static System.Diagnostics.Debug;

namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static Support.Tools;

public sealed class LinkedList<T> : IEnumerable<T>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/
	[SuppressMessage(
		"StyleCop.CSharp.MaintainabilityRules", 
		"SA1401:Fields should be private", 
		Justification = DataTransferStruct)]
	public sealed record Node(T Item)
	{
#if WITH_INSTRUMENTATION
		private const int RecursiveStringLimit = 100;
#endif
		
		public T Item = Item;
		public Node? NextNode;

		private string? ItemString => Item == null ? "null" : Item.ToString();
		
#if WITH_INSTRUMENTATION
		public override string ToString() => ToDebugString();

		public string ToDebugString() => ToDebugString(RecursiveStringLimit);

		private string ToDebugString(int depth) =>
			NextNode == null 
				? ItemString!
				: depth == 0 
					? "..." 
					: $"{Item} [{NextNode.ToDebugString(depth - 1)}]";
#else
		public override string ToString() => $"Node:{{{ItemString}}}";
#endif
	}

	private Node? back;

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
		UpdateVersion();
	}

	public void Concat(LinkedList<T> other)
	{
		other.ThrowIfNull();
		
		if (other.IsEmpty)
		{
			// Nothing to do
			return;
		}
		
		var otherFront = other.front;
		var otherBack = other.back;
		int otherCount = other.Count;
		
		other.Clear(); // Clear so there are no nodes part of both lists

		if (IsEmpty)
		{
			front = otherFront;
		}
		else
		{
			back.NextNode = otherFront;
		}
		
		back = otherBack;
		Count += otherCount;
		UpdateVersion();
	}

	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	public Node InsertAfter(Node node, T item)
	{
		node.ThrowIfNull();
		
		if (node == back)
		{
			return InsertAtBack(item);
		}

		// newNode has node's NextNode
		var newNode = node with { Item = item };

		node.NextNode = newNode;
		
		Count++;
		UpdateVersion();

		return newNode;
	}

	public Node InsertAtBack(T item)
	{
		if (IsEmpty)
		{
			return InsertFirstItem(item);
		}
		
		back.NextNode = new Node(item);
		back = back.NextNode;
		
		Count++;
		UpdateVersion();

		return back;
	}

	public Node InsertAtFront(T item)
	{
		if (IsEmpty)
		{
			return InsertFirstItem(item);
		}

		var newHead = new Node(item)
		{
			NextNode = front,
		};
			
		front = newHead;

		Count++;
		UpdateVersion();

		return front;
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
		
		if (removedNode == back)
		{
			back = node;
		}
		
		Count--;
		UpdateVersion();
		return removedNode;
	}

	public Node RemoveFromFront()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		var removedNode = front!;

		if (IsSingleton)
		{
			front = back = null;
		}
		else
		{
			front = front!.NextNode;
		}
		
		Count--;
		UpdateVersion();

		return removedNode;
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

	public override string ToString() => IsEmpty ? "[]" : $"[{First}]";

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private Node InsertFirstItem(T item)
	{
		Count++;
		front = back = new Node(item);
		
		UpdateVersion();

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

	private void UpdateVersion()
	{
		version++;
		AssertInternalStateIsValid();
	}
	
	[Conditional(Diagnostics.DebugDefine)]
	private void AssertInternalStateIsValid()
	{
		if (IsEmpty)
		{
			Assert(Count == 0);
			Assert(front == null);
			Assert(back == null);
		}
		else if (IsSingleton)
		{
			Assert(Count == 1);
			Assert(front == back);
			Assert(front != null);
			Assert(back.NextNode == null);
		}
		else
		{
			Assert(Count > 1);
			Assert(front != null);
			Assert(back != null);
			Assert(front != back);
			Assert(back.NextNode == null);

			if (Count == 2)
			{
				Assert(front.NextNode == back);
			}
		}
	}
}

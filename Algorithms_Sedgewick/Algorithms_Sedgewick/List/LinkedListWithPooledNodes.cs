using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Algorithms_Sedgewick.Stack;
using Support;
using static System.Diagnostics.Debug;
using static Support.Tools;

namespace Algorithms_Sedgewick.List;

public class LinkedListWithPooledNodes<T> : IEnumerable<T?>
{
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms.
	*/
	[SuppressMessage(
		"StyleCop.CSharp.MaintainabilityRules", 
		"SA1401:Fields should be private", 
		Justification = DataTransferStruct)]
	public sealed record Node
	{
#if WITH_INSTRUMENTATION
		private const int RecursiveStringLimit = 100;
#endif
		
		public T? Item;
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
		/// <inheritdoc/>
		public override string ToString() => $"Node:{{{ItemString}}}";
#endif
	}
	
	private readonly FixedCapacityStack<Node> pool;
	
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

	public LinkedListWithPooledNodes(int capacity)
	{
		pool = new FixedCapacityStack<Node>(capacity);
		
		for (int i = 0; i < capacity; i++)
		{
			pool.Push(new Node());
		}
	}

	public void Clear()
	{
		Node? cachedNode = null;
		foreach (var node in Nodes)
		{
			if (cachedNode != null)
			{
				ReturnToPool(cachedNode);
			}

			cachedNode = node;
		}
		
		if (cachedNode != null)
		{
			ReturnToPool(cachedNode);
		}
		
		ClearWithoutRelease();
	}

	/// <summary>
	/// Concatenates the other list to the end of this list.
	/// </summary>
	/// <param name="other">The list to concatenate to this list.</param>
	/// <remarks>The other list is cleared after this operation.</remarks>
	public void Concat(LinkedListWithPooledNodes<T> other)
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
		
		other.ClearWithoutRelease(); // Clear so there are no nodes part of both lists, bit do not return the nodes to pool. 

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

	public IEnumerator<T?> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

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
		
		back.NextNode = pool.Pop();
		
		Assert(back.NextNode.NextNode == null);
		
		back.NextNode.Item = item;
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

		var newHead = pool.Pop() with
		{
			Item = item, 
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
	/// <exception cref="System.ArgumentNullException">Thrown when the input node is null.</exception>
	/// <exception cref="System.InvalidOperationException">Thrown when the input node does not have a successor to remove.</exception>
	/*
		Here and in RemoveFromFront below we do not return the removed node, since we return it to the pool.
		If we returned it, the caller would have to return it to the pool, which
		makes leaks more probable. 
	*/
	public void RemoveAfter(Node node)
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
		
		ReturnToPool(removedNode);
	}

	public void RemoveFromFront()
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
			
		ReturnToPool(removedNode);
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

	private void ClearWithoutRelease()
	{
		front = back = null;
		Count = 0;
		UpdateVersion();
	}
	
	private Node InsertFirstItem(T item)
	{
		Count++;
		front = back = pool.Pop() with { Item = item };
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
	
	private void ReturnToPool(Node cachedNode)
	{
		pool.Push(cachedNode);
		cachedNode.NextNode = null;
		cachedNode.Item = default;
	}
}

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Support;

namespace Algorithms_Sedgewick.List;

using static Debug;
using static Tools;

/// <summary>
/// Represents a singly linked list.
/// </summary>
/// <typeparam name="T">The type of elements in the <see cref="LinkedListWithClassNode{T}"/>.</typeparam>
public sealed class LinkedListWithClassNode<T> : IEnumerable<T>
{
	/// <summary>
	/// Represents a node in the <see cref="LinkedListWithClassNode{T}"/>.
	/// </summary>
	/*
		Exposing the node class makes the linked list a more useful container to
		use for implementing other algorithms, but more "unsafe" to use (since 
		by changing the node the list can be corrupted.
	*/
	[SuppressMessage(
		"StyleCop.CSharp.MaintainabilityRules", 
		"SA1401:Fields should be private", 
		Justification = DataTransferStruct)]
	public sealed class Node
	{
#if WITH_INSTRUMENTATION
		private const int RecursiveStringLimit = 100;
#endif
		
		/// <summary>
		/// The contents of this node. 
		/// </summary>
		public T Item;
		
		/// <summary>
		/// The next node in the linked list. <see langword="null"/> if this is the last node.
		/// </summary>
		public Node? NextNode;
		
		private string? ItemString => Item.AsText();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Node"/> class.
		/// </summary>
		/// <param name="item">The item contained in this node.</param>
		public Node(T item)
		{
			Item = item;
		}
		
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

	private Node? back;
	private Node? front;
	private int version = 0;

	/// <summary>
	/// Gets the number of elements contained in the <see cref="LinkedListWithClassNode{T}"/>.
	/// </summary>
	public int Count { get; private set; } = 0;

	/// <summary>
	/// Gets the first node of the <see cref="LinkedListWithClassNode{T}"/>.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">Thrown when the <see cref="LinkedListWithClassNode{T}"/> is empty.</exception>
	public Node First
	{
		get
		{
			ValidateNotEmpty();
			Assert(front != null);
				
			return front;
		}
	}

	/// <summary>
	/// Gets a value indicating whether the linked list is empty.
	/// </summary>
	[MemberNotNullWhen(false, nameof(front), nameof(back))]
	[MemberNotNullWhen(false, nameof(front), nameof(back))]
	public bool IsEmpty => front == null;

	/// <summary>
	/// Gets a value indicating whether the linked list has only one item.
	/// </summary>
	public bool IsSingleton => front == back;
	
	/// <summary>
	/// Gets the last node of the linked list.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">Thrown when the linked list is empty.</exception>
	public Node Last
	{
		get
		{
			ValidateNotEmpty();
			Assert(back != null);
				
			return back;
		}
	}

	/// <summary>
	/// Gets an enumerable collection of nodes in the linked list.
	/// </summary>
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
	/// Removes all nodes from this <see cref="LinkedListWithClassNode{T}"/>.
	/// </summary>
	public void Clear()
	{
		front = back = null;
		Count = 0;
		UpdateVersion();
	}

	/// <summary>
	/// Concatenates the current <see cref="LinkedListWithClassNode{T}"/> with another <see cref="LinkedListWithClassNode{T}"/>.
	/// </summary>
	/// <param name="other">The <see cref="LinkedListWithClassNode{T}"/> to concatenate with.</param>
	public void Concat(LinkedListWithClassNode<T> other)
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

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => Nodes.Select(node => node.Item).GetEnumerator();

	/// <summary>
	/// Inserts a new item after the specified node in the linked list.
	/// </summary>
	/// <param name="node">The node after which to insert the new item.</param>
	/// <param name="item">The item to insert.</param>
	/// <returns>The newly created node containing the inserted item.</returns>
	public Node InsertAfter(Node node, T item)
	{
		node.ThrowIfNull();
		
		if (node == back)
		{
			return InsertAtBack(item);
		}

		// newNode has node's NextNode
		var newNode = node;
		newNode.Item = item;

		node.NextNode = newNode;
		
		Count++;
		UpdateVersion();

		return newNode;
	}

	/// <summary>
	/// Inserts a new item at the end of the linked list.
	/// </summary>
	/// <param name="item">The item to insert.</param>
	/// <returns>The newly created node containing the inserted item.</returns>
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

	/// <summary>
	/// Inserts a new item at the beginning of the linked list.
	/// </summary>
	/// <param name="item">The item to insert.</param>
	/// <returns>The newly created node containing the inserted item.</returns>
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

	/// <summary>
	/// Removes the first item from the linked list.
	/// </summary>
	/// <returns>The removed node.</returns>
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

	/// <summary>
	/// Reverses the order of the nodes in the linked list.
	/// </summary>
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

	/// <inheritdoc/>
	public override string ToString() => IsEmpty ? "[]" : $"[{First}]";

	/// <inheritdoc/>
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

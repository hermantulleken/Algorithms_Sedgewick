using System.Collections;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// Queue implementation using a linked list.
/// </summary>
/// <typeparam name="T">The type of elements that can be inserted into the queue.</typeparam>s
public sealed class QueueWithLinkedList<T> : IQueue<T>
{
	private const bool ToStringShowsContents = false;

	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();

	private readonly List.LinkedList<T> items = new();

	/// <inheritdoc />
	public int Count => items.Count;

	/// <inheritdoc />
	public bool IsEmpty => items.IsEmpty;

	/// <inheritdoc />
	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items.First.Item;
		}
	}
	
	private int Id { get; } = IdGenerator.GetNextId();

	/// <inheritdoc />
	public void Clear() => items.Clear();

	/// <inheritdoc />
	public T Dequeue() => items.RemoveFromFront().Item;

	/// <inheritdoc />
	public void Enqueue(T item) => items.InsertAtBack(item);

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

#pragma warning disable CS0162 
	/// <inheritdoc />
	// ReSharper disable once HeuristicUnreachableCode
	// We use the constant bool for easy switching when we debug.
	public override string ToString() => ToStringShowsContents ? items.ToString() : "Q: " + Id;
#pragma warning restore CS0162
	
	/// <inheritdoc />
	public override int GetHashCode() => Id.GetHashCode();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

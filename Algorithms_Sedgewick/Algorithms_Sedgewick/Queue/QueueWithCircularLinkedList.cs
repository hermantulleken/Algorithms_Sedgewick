using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// Queue implementation using a linked list.
/// </summary>
/// <typeparam name="T">The type of elements that can be inserted into the queue.</typeparam>s
public sealed class QueueWithCircularLinkedList<T> : IQueue<T>
{
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();
	
	private readonly CircularLinkedList<T> items = new();

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
			return items.First.Item; // First item is not null when list is not empty.
		}
	}
	
	private int Id { get; } = IdGenerator.GetNextId();

	/// <inheritdoc />
	public void Clear() => items.Clear();

	/// <inheritdoc />
	public T Dequeue() => items.RemoveFromFront().Item!;

	/// <inheritdoc />
	public void Enqueue(T item) => items.InsertAtBack(item);

	/// <inheritdoc />
	public override int GetHashCode() => Id.GetHashCode();

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

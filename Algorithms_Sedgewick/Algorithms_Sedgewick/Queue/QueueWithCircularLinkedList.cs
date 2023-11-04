using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

public sealed class QueueWithCircularLinkedList<T> : IQueue<T>
{
	private readonly CircularLinkedList<T> items = new();

	private static IdGenerator idGenerator = new();
	
	public int Id { get; } = idGenerator.GetNextId();

	public int Count => items.Count;

	public bool IsEmpty => items.IsEmpty;

	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items.First()!; // First item is not null when list is not empty.
		}
	}

	public void Clear() => items.Clear();

	public T Dequeue() => items.RemoveFromFront().Item!;

	public void Enqueue(T item) => items.InsertAtBack(item);
	
	public override int GetHashCode() => Id.GetHashCode();

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

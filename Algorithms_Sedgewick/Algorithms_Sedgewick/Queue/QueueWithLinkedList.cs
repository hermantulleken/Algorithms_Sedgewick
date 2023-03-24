using System.Collections;

namespace Algorithms_Sedgewick.Queue;

public sealed class QueueWithLinkedList<T> : IQueue<T>
{
	private readonly List.LinkedList<T> items = new();

	public int Count => items.Count;

	public bool IsEmpty => items.IsEmpty;

	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items.First();
		}
	}

	public void Clear() => items.Clear();

	public T Dequeue() => items.RemoveFromFront().Item;

	public void Enqueue(T item) => items.InsertAtBack(item);

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	public override string ToString() => items.ToString();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

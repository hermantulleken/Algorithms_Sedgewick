using System.Collections;
using global::System.Collections.Generic;

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

	public void Enqueue(T item) => items.InsertAtBack(item);

	public T Dequeue() => items.RemoveFromFront().Item;
	public void Clear() => items.Clear();

	public override string ToString() => items.ToString();

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

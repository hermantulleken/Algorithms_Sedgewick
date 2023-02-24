using System.Collections;

namespace Algorithms_Sedgewick;

public sealed class BagWithLinkedList<T> : IBag<T>
{
	private readonly LinkedList<T> items = new ();

	public int Count => items.Count;

	public bool IsEmpty => Count == 0;

	public void Add(T item)
	{
		items.InsertAtFront(item);
	}

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

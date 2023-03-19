using System.Collections;
using global::System.Collections.Generic;

namespace Algorithms_Sedgewick.Bag;

public sealed class BagWithLinkedList<T> : IBag<T>
{
	private readonly List.LinkedList<T> items = new ();

	public int Count => items.Count;

	public bool IsEmpty => Count == 0;

	public void Add(T item)
	{
		items.InsertAtFront(item);
	}

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

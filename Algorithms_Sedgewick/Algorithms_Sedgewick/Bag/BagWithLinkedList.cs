namespace Algorithms_Sedgewick.Bag;

using System.Collections;

/// <summary>
/// An implementation of <see cref="IBag{T}"/> that uses a <see cref="List.LinkedList{T}"/> for its implementation.
/// </summary>
/// <inheritdoc />
public sealed class BagWithLinkedList<T> : IBag<T>
{
	private readonly List.LinkedList<T> items = new();
	
	/// <inheritdoc />
	public int Count => items.Count;

	/// <inheritdoc />
	public void Add(T item)
	{
		items.InsertAtFront(item);
	}

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

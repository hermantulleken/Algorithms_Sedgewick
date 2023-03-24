using System.Collections;
using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick.Deque;

public class DequeWithDoublyLinkedList<T> : IDeque<T>
{
	private readonly DoublyLinkedList<T> items = new();
	public int Count => items.Count;
	public T PeekLeft => items.First.Item;
	public T PeekRight => items.Last.Item;

	public void PushLeft(T item) => items.InsertAtFront(item);
	public void PushRight(T item) => items.InsertAtBack(item);
	public T PopLeft() => items.RemoveFromFront().Item;
	public T PopRight() => items.RemoveFromBack().Item;

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Clear() => items.Clear();

	public override string ToString() => Formatter.Pretty(this);
}

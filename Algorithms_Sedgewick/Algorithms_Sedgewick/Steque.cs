using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick;

public class Steque<T> : IStack<T>
{
	private readonly DoublyLinkedList<T> items = new();

	public int Count => items.Count;
	public T Peek => items.First.Item;

	public void Clear() => items.Clear();

	public void Enqueue(T item) => items.InsertAtBack(item);

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	public T Pop() => items.RemoveFromFront().Item;
	public void Push(T item) => items.InsertAtFront(item);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

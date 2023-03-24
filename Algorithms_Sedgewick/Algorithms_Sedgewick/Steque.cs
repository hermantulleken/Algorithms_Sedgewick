using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick;

public class Steque<T> : IStack<T>
{
	private readonly DoublyLinkedList<T> items = new();

	public int Count => items.Count;
	public T Peek => items.First.Item;
	public void Push(T item) => items.InsertAtFront(item);

	public T Pop() => items.RemoveFromFront().Item;

	public void Enqueue(T item) => items.InsertAtBack(item);

	public void Clear() => items.Clear();
	
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
}

using System.Collections;
using AlgorithmsSW.List;
using AlgorithmsSW.Stack;

namespace AlgorithmsSW;

/// <summary>
/// An implementation a linear container that allows insertion and removal at both ends.
/// </summary>
/// <typeparam name="T">The type of elements in the steque.</typeparam>
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

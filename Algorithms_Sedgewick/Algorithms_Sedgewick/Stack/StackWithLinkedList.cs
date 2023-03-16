using System.Collections;

namespace Algorithms_Sedgewick.Stack;

public sealed class StackWithLinkedList<T> : IStack<T>
{
	private readonly List.LinkedList<T> items = new List.LinkedList<T>();

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

	public void Push(T item) => items.InsertAtFront(item);

	public T Pop() => items.RemoveFromFront().Item;
	
	public void Clear() => items.Clear();

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

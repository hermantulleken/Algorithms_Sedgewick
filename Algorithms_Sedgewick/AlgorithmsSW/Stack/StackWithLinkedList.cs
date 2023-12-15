namespace AlgorithmsSW.Stack;

using System.Collections;

public sealed class StackWithLinkedList<T> : IStack<T>
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

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	public T Pop() => items.RemoveFromFront().Item;

	public void Push(T item) => items.InsertAtFront(item);

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

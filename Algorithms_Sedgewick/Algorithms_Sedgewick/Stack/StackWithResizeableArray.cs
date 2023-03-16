using System.Collections;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Stack;

public sealed class StackWithResizeableArray<T> : IStack<T>
{
	private readonly ResizeableArray<T> items;

	public int Capacity => items.Capacity;
	public int Count => items.Count;
	public bool IsFull => items.IsFull;
	public bool IsEmpty => items.IsEmpty;
		
	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items[^1];
		}
	}

	public StackWithResizeableArray(int capacity = ResizeableArray.DefaultCapacity)
	{
		items = new ResizeableArray<T>(capacity);
	}

	public void Push(T item) => items.Add(item);

	public T Pop() => items.RemoveLast();
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

using System.Collections;
using AlgorithmsSW.List;

namespace AlgorithmsSW.Stack;

public sealed class StackWithResizeableArray<T> : IStack<T>
{
	private readonly ResizeableArray<T> items;

	public int Capacity => items.Capacity;
	
	public int Count => items.Count;
	
	public bool IsEmpty => items.IsEmpty;
	
	public bool IsFull => items.IsFull;

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

	public void Clear() => items.Clear();

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = Count - 1; i >= 0; i--)
		{
			yield return items[i];
		}
	}

	public T Pop() => items.RemoveLast();

	public void Push(T item) => items.Add(item);

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}

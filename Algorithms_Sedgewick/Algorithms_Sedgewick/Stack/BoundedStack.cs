using System.Collections;
using Algorithms_Sedgewick.Deque;

namespace Algorithms_Sedgewick.Stack;

// With deck
public class BoundedStack<T> : IStack<T>
{
	private readonly DequeWithDoublyLinkedList<T> deque = new();
	
	public int Count => deque.Count;
	
	public bool IsEmpty => Count == 0;

	public bool IsFull => Count == MaxSize;
	
	public int MaxSize { get; }

	public T Peek => deque.PeekRight;

	public BoundedStack(int maxSize)
	{
		MaxSize = maxSize;
	}

	public void Clear()
	{
		deque.Clear();
	}

	public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

	public T Pop()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		return deque.PopRight();
	}

	public void Push(T item)
	{
		if (MaxSize == 0)
		{
			return;
		}

		if (IsFull)
		{
			deque.PopLeft();
		}
		
		deque.PushRight(item);
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

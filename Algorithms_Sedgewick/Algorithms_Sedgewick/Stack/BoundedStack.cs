using System.Collections;
using Algorithms_Sedgewick.Deque;
using global::System.Collections.Generic;

namespace Algorithms_Sedgewick.Stack;

//With deck
public class BoundedStack<T> : IStack<T>
{
	public int Count => deque.Count;

	public T Peek => deque.PeekRight;
	public int MaxSize { get; }
	public bool IsEmpty => Count == 0;
	
	public bool IsFull => Count == MaxSize;

	private readonly DequeWithDoublyLinkedList<T> deque = new();
	
	public BoundedStack(int maxSize)
	{
		MaxSize = maxSize;
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

	public T Pop()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		return deque.PopRight();
	}

	public void Clear()
	{
		deque.Clear();
	}
	
	public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using System.Collections;

namespace Algorithms_Sedgewick;

//With deck
public class BoundedStack<T> : IStack<T>
{
	public int Count { get; }
	public T Peek { get; }
	public int MaxSize { get; }
	public bool IsEmpty => Count == 0;
	
	public bool IsFull => Count == MaxSize;

	private DequeWithDoublyLinkedList<T> deque = new();
	
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
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
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

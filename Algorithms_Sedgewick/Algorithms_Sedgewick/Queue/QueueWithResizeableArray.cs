using System.Collections;

namespace Algorithms_Sedgewick.Queue;

public class QueueWithResizeableArray<T> : IQueue<T>
{
	private const int DefaultCapacity = 4;
	
	private T[] items;
	private int head;
	private int tail;
	
	public int Capacity => items.Length;
	
	public int Count { get; private set; }
	
	public bool IsEmpty => Count == 0;
	
	public bool IsFull => Count == Capacity;
	
	public QueueWithResizeableArray(int capacity = DefaultCapacity)
	{
		items = new T[capacity];
	}

	public T Peek { get; }

	public void Clear()
	{
		head = 0;
		tail = 0;
		Count = 0;
	}
	
	public T Dequeue()
	{
		ValidateNotEmpty();
		
		T item = items[head];
		items[head] = default!;
		head = (head + 1) % Capacity;
		Count--;
		
		if (Count > 0 && Count == Capacity / 4)
		{
			Resize(Capacity / 2);
		}
		
		return item;
	}
	
	public void Enqueue(T item)
	{
		if (IsFull)
		{
			Resize(Capacity * 2);
		}
		
		items[tail] = item;
		tail = (tail + 1) % Capacity;
		Count++;
	}
	
	private void Resize(int capacity)
	{
		var resized = new T[capacity];
		
		for (int i = 0; i < Count; i++)
		{
			resized[i] = items[(head + i) % Capacity];
		}
		
		items = resized;
		head = 0;
		tail = Count;
	}
	
	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < Count; i++)
		{
			yield return items[(head + i) % Capacity];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

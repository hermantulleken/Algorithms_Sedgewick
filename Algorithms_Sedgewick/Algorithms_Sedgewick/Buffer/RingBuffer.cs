using System.Collections;
using System.Diagnostics;

namespace Algorithms_Sedgewick.Buffer;

public sealed class RingBuffer<T> : IBuffer<T>
{
	private int front;
	private int back;
	private readonly T[] items;

	public int Count { get; private set; }
	public int Capacity { get; }
    
	public T First 
		=> Count > 0 
			? this[0] 
			: throw ThrowHelper.ContainerEmptyException;
	public T Last 
		=> Count > 0
			? this[Count - 1]
			: throw ThrowHelper.ContainerEmptyException;
    
	public T this[int index]
	{
		get
		{
			ValidateIndex(index);
            
			return items[GetRealIndex(index)];
		}
		set
		{
			ValidateIndex(index);
			items[GetRealIndex(index)] = value;
		}
	}

	public RingBuffer(int capacity)
	{
		if (capacity <= 0)
		{
			ThrowHelper.ThrowCapacityCannotBeNegativeOrZero(capacity);
		}
        
		Capacity = capacity;
		items = new T[capacity];
		ResetPointers();
	}

	public void Insert(T item)
	{
		items[back] = item;

		back++;

		if (back == Capacity)
		{
			back = 0;
		}

		if (Count < Capacity)
		{
			Count++;
		}
		else
		{
			front++;

			if (front == Capacity)
			{
				front = 0;
			}
		}

		AssertCountInvariants();
	}

	public void Clear()
	{
		ReInitialize();
		ResetPointers();
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (front < back)
		{
			for (int i = front; i < back; i++)
			{
				yield return items[i];
			}
		}
		else
		{
			for (int i = front; i < Capacity; i++)
			{
				yield return items[i];
			}
            
			for (int i = 0; i < back; i++)
			{
				yield return items[i];
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/*
        Not strictly necessary for value types. For reference types, 
        this prevents having ghost references to objects and 
        so leak memory. 
    */
	private void ReInitialize()
	{
		if (front < back)
		{
			for (int i = front; i < back; i++)
			{
				items[i] = default;
			}
		}
		else
		{
			for (int i = front; i < Capacity; i++)
			{
				items[i] = default;
			}
            
			for (int i = 0; i < back; i++)
			{
				items[i] = default;
			}
		}
	}

	public void ResetPointers()
	{
		front = 0;
		back = 0;
		Count = 0;
        
		AssertCountInvariants();
	}

	private void AssertCountInvariants()
	{
		Debug.Assert(Count <= Capacity);
        
		if (front < back)
		{
			Debug.Assert(Count == back - front);
		}
		else
		{
			Debug.Assert(Count == Capacity - front + back);
		}
	}
    
	private bool IndexInRange(int index) => 0 <= index && index < Count;
	private int GetRealIndex(int index)
	{
		Debug.Assert(IndexInRange(index));
		return (index + front) % Capacity;
	}
    
	private void ValidateIndex(int index)
	{
		if (!IndexInRange(index))
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
	}
}

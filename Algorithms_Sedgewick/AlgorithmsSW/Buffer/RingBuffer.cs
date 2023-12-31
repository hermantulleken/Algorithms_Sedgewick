namespace AlgorithmsSW.Buffer;

using System.Collections;
using static System.Diagnostics.Debug;

/// <summary>
/// An implementation of <see cref="IBuffer{T}"/> that uses a fixed-size array in a circular way.
/// </summary>
[ExerciseReference(1, 3, 39)]
public sealed class RingBuffer<T> : IBuffer<T>
{
	private readonly T[] items;
	private int back; // after last element
	private int front; // first element

	/// <inheritdoc />
	public bool IsFull => AsIBuffer.IsFull;

	/// <inheritdoc />
	public int Capacity { get; }

	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc />
	public T First 
		=> Count > 0 
			? this[0]
			: throw ThrowHelper.ContainerEmptyException;

	/// <summary>
	/// Gets the element at the specified index, where 0 corresponds to the front of the buffer (that is, the oldest
	/// element). 
	/// </summary>
	/* Design note: We need this to implement window operators. 
	*/
	public T this[int index]
	{
		get
		{
			ValidateIndex(index);
			
			return items[GetRealIndex(index)];
		}
	}

	/// <inheritdoc />
	public T Last 
		=> Count > 0
			? this[Count - 1]
			: throw ThrowHelper.ContainerEmptyException;
	
	private IBuffer<T> AsIBuffer => this;
	
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

	/// <inheritdoc />
	public void Clear()
	{
		ReInitialize();
		ResetPointers();
	}

	/// <inheritdoc />
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

	/// <inheritdoc />
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

	public void ResetPointers()
	{
		front = 0;
		back = 0;
		Count = 0;
		
		AssertCountInvariants();
	}
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void AssertCountInvariants()
	{
		Assert(Count <= Capacity);
		
		if (Count == 0 || Count == Capacity)
		{
			Assert(front == back);
		}
		else if (front < back)
		{
			Assert(Count == back - front);
		}
		else
		{
			Assert(Count == Capacity - front + back);
		}
	}

	private int GetRealIndex(int index)
	{
		Assert(IndexInRange(index));
		return (index + front) % Capacity;
	}

	private bool IndexInRange(int index) => index >= 0 && index < Count;

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
				items[i] = default!;
			}
		}
		else
		{
			for (int i = front; i < Capacity; i++)
			{
				items[i] = default!;
			}
            
			for (int i = 0; i < back; i++)
			{
				items[i] = default!;
			}
		}
	}

	private void ValidateIndex(int index)
	{
		if (!IndexInRange(index))
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
	}
}

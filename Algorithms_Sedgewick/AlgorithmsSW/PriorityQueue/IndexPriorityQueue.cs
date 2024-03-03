using AlgorithmsSW.List;

namespace AlgorithmsSW.PriorityQueue;

/// <summary>
/// Represents a generic index-based priority queue where elements are ordered based on their priority determined by an IComparer.
/// </summary>
/// <remarks>
/// This implementation of a priority queue uses an array-based binary heap. It associates each value with an index, allowing for efficient updates of the queue based on index.
/// </remarks>
/// <typeparam name="T">The type of elements in the priority queue.</typeparam>
public class IndexPriorityQueue<T> 
{
	private const int NotInQueue = -1;

	private readonly T?[] values;
	private readonly int[] priorityQueue; // Contains indexes of values
	private readonly int[] queuePosition;
	private readonly int capacity;
	private readonly IComparer<T> comparer;

	/// <summary>
	/// Gets the number of elements currently in the priority queue.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="IndexPriorityQueue{T}"/> class with a specified capacity and
	/// comparer.
	/// </summary>
	/// <param name="capacity">The maximum number of elements the priority queue can hold.</param>
	/// <param name="comparer">The IComparer to determine the priority of elements.</param>
	public IndexPriorityQueue(int capacity, IComparer<T> comparer)
	{
		comparer.ThrowIfNull();
		capacity.ThrowIfNegative();
		
		this.capacity = capacity;
		this.comparer = comparer;
		values = new T[capacity];
		priorityQueue = new int[capacity + 1];
		queuePosition = new int[capacity];
		
		for (int i = 0; i < capacity; i++)
		{
			queuePosition[i] = NotInQueue;
		}
		Count = 0;
	}

	/// <summary>
	/// Determines whether the priority queue contains the specified index.
	/// </summary>
	/// <param name="index">The index to check for presence in the queue.</param>
	/// <returns><see langword="true"/> if the index is in the queue; otherwise, <see langword="false"/>.</returns>
	public bool Contains(int index)
	{
		ValidateIndex(index);
		
		return queuePosition[index] != NotInQueue;
	}
	
	/// <summary>
	/// Gets a value indicating whether the priority queue is empty.
	/// </summary>
	/// <returns><see langword="true"/> if the queue is empty; otherwise, <see langword="false"/>.</returns>
	public bool IsEmpty => Count == 0;
	
	/// <summary>
	/// Inserts a value with an associated index into the priority queue.
	/// </summary>
	/// <param name="index">The index associated with the value.</param>
	/// <param name="value">The value to insert.</param>
	public void Insert(int index, T value)
	{
		ValidateIndex(index);
		ValidateIndexNotPresent(index);
		
		values[index] = value;
		Count++;
		priorityQueue[Count] = index;
		queuePosition[index] = Count;
		Swim(Count);
	}
	
	/// <summary>
	/// Returns the minimum element of the priority queue without removing it.
	/// </summary>
	/// <returns>A tuple containing the index and value of the minimum element.</returns>
	/*
		What to peek and pop exactly is an interesting question, and depends on the application really.
		I decided to peek and pop both, even though I'd have preferred to peek and pop the index only, because
		it seems in some applications the value may be needed, and the user would not be able to get it without
		maintaining their own dictionary of indexes to values.
	*/
	public (int index, T value) PeekMin()
	{
		ValidateNotEmpty();
		
		return (priorityQueue[1], values[priorityQueue[1]]!);
	}
	
	/// <summary>
	/// Removes and returns the minimum element from the priority queue.
	/// </summary>
	/// <returns>A tuple containing the index and value of the minimum element that was removed.</returns>
	public (int index, T value) PopMin()
	{
		ValidateNotEmpty();
		
		T minimumValue = values[priorityQueue[1]]!;
		int index = priorityQueue[1];
		values[priorityQueue[1]] = default;
		queuePosition[priorityQueue[1]] = NotInQueue;
		Swap(1, Count);
		Count--;
		Sink(1);
		
		return (index, minimumValue);
	}
	
	/// <summary>
	/// Updates the value of the element at the specified index.
	/// </summary>
	/// <param name="index">The index of the element to update.</param>
	/// <param name="newValue">The new value to replace the current value.</param>
	public void UpdateValue(int index, T newValue)
	{
		ValidateIndex(index);
		ValidateIndexPresent(index);

		int comparisonResult = comparer.Compare(newValue, values[index]);
		values[index] = newValue;

		switch (comparisonResult)
		{
			case < 0:
				Swim(queuePosition[index]);
				break;
			case > 0:
				Sink(queuePosition[index]);
				break;
			default:
				// If comparisonResult == 0, the key remains unchanged, so no action is needed
				break;
		}
	}
	
	private void Swim(int index)
	{
		while (index > 1 && IsLess(index, index / 2))
		{
			Swap(index, index / 2);
			index /= 2;
		}
	}
	
	private void Sink(int index)
	{
		while (2 * index <= Count)
		{
			int j = 2 * index;
			if (j < Count && IsLess(j + 1, j))
			{
				j++;
			}
			if (!IsLess(j, index))
			{
				break;
			}
			Swap(index, j);
			index = j;
		}
	}

	private bool IsLess(int i, int j)
	{
		return comparer.Compare(values[priorityQueue[i]], values[priorityQueue[j]]) < 0;
	}

	private void Swap(int i, int j)
	{
		priorityQueue.SwapAt(i, j);
		queuePosition.SwapAt(priorityQueue[i], priorityQueue[j]);
	}

	private void ValidateIndex(int index) => index.ThrowIfOutOfRange(0, capacity);

	private void ValidateIndexNotPresent(int index)
	{
		if (Contains(index))
		{
			throw new ArgumentException("Index is already in the priority queue.");
		}
	}
	
	private void ValidateIndexPresent(int index)
	{
		if (!Contains(index))
		{
			throw new ArgumentException("Index is not in the priority queue.");
		}
	}

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException("Priority queue is empty.");
		}
	}
}

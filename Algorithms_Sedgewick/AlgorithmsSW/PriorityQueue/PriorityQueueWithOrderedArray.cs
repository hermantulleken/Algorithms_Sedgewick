using AlgorithmsSW.List;
using static AlgorithmsSW.ThrowHelper;

namespace AlgorithmsSW.PriorityQueue;

using System.Collections;

// Ex. 2.4.3
public sealed class PriorityQueueWithOrderedArray<T> 
	: IPriorityQueue<T>
{
	private readonly ResizeableArray<T> items;
	private readonly IComparer<T> comparer;

	public int Count => items.Count;

	public T PeekMin
	{
		get
		{
			if (this.IsEmpty())
			{
				ThrowContainerEmpty();
			}

			return items[0];
		}
	}

	private int Capacity { get; }
	
	private bool IsFull => Count == Capacity;

	public PriorityQueueWithOrderedArray(int capacity, IComparer<T> comparer)
	{
		Capacity = capacity;
		this.comparer = comparer;
		items = new ResizeableArray<T>();
	}

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var min = items.RemoveAt();
		return min;
	}

	public void Push(T item)
	{
		item.ThrowIfNull();
		
		if (IsFull)
		{
			ThrowContainerFull();
		}

		items.InsertSorted(item, comparer);
	}

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using Algorithms_Sedgewick.List;
using static Algorithms_Sedgewick.ThrowHelper;

namespace Algorithms_Sedgewick.PriorityQueue;

//Ex. 2.4.3
public sealed class PriorityQueueWithOrderedArray<T> : IPriorityQueue<T> where T : IComparable<T>
{
	private int Capacity { get; }
	private bool IsFull => Count == Capacity;
	private readonly ResizeableArray<T> items;

	public PriorityQueueWithOrderedArray(int capacity)
	{
		Capacity = capacity;
		items = new ResizeableArray<T>();
	}

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

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var min = items.DeleteAt();
		return min;
	}

	public void Push(T item)
	{
		item.ThrowIfNull();
		
		if (IsFull)
		{
			ThrowContainerFull();
		}

		items.InsertSorted(item);
	}
}

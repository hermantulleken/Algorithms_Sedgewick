using Algorithms_Sedgewick.List;
using static Algorithms_Sedgewick.Sort.Sort;
using static Algorithms_Sedgewick.ThrowHelper;

namespace Algorithms_Sedgewick.PriorityQueue;

//Ex. 2.4.3
//Note: We maintain the minimum object in the last position, as it is the cheapest to delete from.
public sealed class PriorityQueueWithUnorderedArray<T> : IPriorityQueue<T> where T : IComparable<T>
{
	public bool IsFull => items.IsFull;

	private int LastIndex => items.Count - 1;
	private readonly ResizeableArray<T> items = new();

	public int Count => items.Count;

	public T PeekMin
	{
		get
		{
			if (this.IsEmpty())
			{
				ThrowContainerEmpty();
			}

			return items[LastIndex];
		}
	}

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var min = items[LastIndex];
		items.DeleteAt(LastIndex);

		if (Count > 1)
		{
			MoveMinToLast();
		}
		
		return min;
	}

	public void Push(T item)
	{
		item.ThrowIfNull();
		
		if (IsFull)
		{
			ThrowContainerFull();
		}

		items.Add(item);

		if (Count > 1)
		{
			MoveMinToLast();
		}
	}

	private void MoveMinToLast() => SwapAt(items,LastIndex, items.FindIndexOfMin());
}

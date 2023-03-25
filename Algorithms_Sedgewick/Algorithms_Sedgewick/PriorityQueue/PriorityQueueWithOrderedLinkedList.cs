using static Algorithms_Sedgewick.ThrowHelper;

namespace Algorithms_Sedgewick.PriorityQueue;

// Ex. 2.4.3
public sealed class PriorityQueueWithOrderedLinkedList<T> : IPriorityQueue<T> 
	where T : IComparable<T>
{
	private readonly List.LinkedList<T> items = new();

	public int Count => items.Count;

	public T PeekMin
	{
		get
		{
			if (this.IsEmpty())
			{
				ThrowContainerEmpty();
			}

			return items.First.Item;
		}
	}

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var minNode = items.RemoveFromFront();
		return minNode.Item;
	}

	public void Push(T item) => items.InsertSorted(item.ThrowIfNull());
}

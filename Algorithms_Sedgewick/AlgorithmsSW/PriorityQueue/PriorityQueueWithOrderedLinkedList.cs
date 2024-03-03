using static AlgorithmsSW.ThrowHelper;

namespace AlgorithmsSW.PriorityQueue;

using System.Collections;

[ExerciseReference(2, 4, 3)]
public sealed class PriorityQueueWithOrderedLinkedList<T>(IComparer<T> comparer)
	: IPriorityQueue<T> 
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

	public void Push(T item) => items.InsertSorted(item.ThrowIfNull(), comparer);

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

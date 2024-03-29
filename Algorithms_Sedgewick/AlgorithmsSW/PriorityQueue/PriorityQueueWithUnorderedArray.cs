﻿using AlgorithmsSW.List;
using static AlgorithmsSW.ThrowHelper;

namespace AlgorithmsSW.PriorityQueue;

using System.Collections;

[ExerciseReference(2, 4, 3)]
// Note: We maintain the minimum object in the last position, as it is the cheapest to delete from.
public sealed class PriorityQueueWithUnorderedArray<T>(IComparer<T> comparer)
	: IPriorityQueue<T> 
{
	private readonly ResizeableArray<T> items = new();

	public int Count => items.Count;
	
	public bool IsFull => items.IsFull;

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

	private int LastIndex => items.Count - 1;

	public T PopMin()
	{
		if (this.IsEmpty())
		{
			ThrowContainerEmpty();
		}

		var min = items[LastIndex];
		items.RemoveAt(LastIndex);

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

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private void MoveMinToLast() => items.SwapAt(LastIndex, items.FindIndexOfMin(comparer));
}

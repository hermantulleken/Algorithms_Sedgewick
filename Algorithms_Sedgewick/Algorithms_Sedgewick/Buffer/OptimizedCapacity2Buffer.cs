﻿using System.Collections;

namespace Algorithms_Sedgewick.Buffer;

/// <summary>
/// Represents a buffer with a fixed capacity of two items.
/// </summary>
/// <typeparam name="T">The type of items contained in the buffer.</typeparam>
public sealed class OptimizedCapacity2Buffer<T> : IBuffer<T?>, IPair<T>
{
	private readonly T?[] items = new T[2];

	public IEnumerator<T?> GetEnumerator()
	{
		if (Count == 1)
		{
			yield return items[1];
			yield break;
		}
		
		for (int i = 0; i < Count; i++)
		{
			yield return items[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public int Capacity => 2;
	
	public int Count { get; private set; }
	
	public T? First => Count switch
	{
		0 => throw ThrowHelper.ContainerEmptyException,
		1 => items[1],
		2 => items[0],
		_ => throw ThrowHelper.UnreachableCodeException,
	};
	
	public bool HasValue => Count > 0;
	
	public bool HasPreviousValue => Count > 1;

	public T? Last => Count switch
	{
		0 => throw ThrowHelper.ContainerEmptyException,
		1 or 2 => items[1],
		_ => throw ThrowHelper.UnreachableCodeException,
	};
	
	public void Clear()
	{
		Count = 0;
		items[0] = default;
		items[1] = default;
	}

	public void Insert(T? item)
	{
		items[0] = items[1];
		items[1] = item;
		
		if (Count < 2)
		{
			Count++;
		}
	}
}
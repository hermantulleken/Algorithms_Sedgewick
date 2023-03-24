﻿namespace Algorithms_Sedgewick.SymbolTable;

using List;

// Ex. 3.1.2
public class SymbolTableWithKeyArray<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;

	// TODO: Consider a parallel array structure
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	public int Count => keys.Count;

	public TValue this[TKey key]
	{
		get
		{
			if (TryFind(key, out int index))
			{
				return values[index];
			}
			
			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (TryFind(key, out int index))
			{
				values[index] = value;
			}
			else
			{
				keys.Add(key);
				values.Add(value);
			}
		}
	}

	public IEnumerable<TKey> Keys => keys;

	public SymbolTableWithKeyArray(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		keys = new ResizeableArray<TKey>();
		values = new ResizeableArray<TValue>();
	}

	public bool ContainsKey(TKey key) => TryFind(key, out _);

	public void RemoveKey(TKey key)
	{
		if (TryFind(key, out int index))
		{
			keys.DeleteAt(index);
			values.DeleteAt(index);
		}
		else
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}
	}

	private bool TryFind(TKey key, out int index)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			if (comparer.Equal(key, keys[i]))
			{
				index = i;
				return true;
			}
		}

		index = -1;
		return false;
	}
}

﻿namespace AlgorithmsSW.SymbolTable;

using List;

[ExerciseReference(3, 1, 2)]
public class SymbolTableWithKeyArray<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	// TODO: Should this rather be an equality comparer? 
	public IComparer<TKey> Comparer { get; }

	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	public int Count => keys.Count;

	public IEnumerable<TKey> Keys => keys;

	public SymbolTableWithKeyArray(IComparer<TKey> comparer)
        : this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	public SymbolTableWithKeyArray(int initialCapacity, IComparer<TKey> comparer)
	{
		this.Comparer = comparer;
		keys = new ResizeableArray<TKey>(initialCapacity);
		values = new ResizeableArray<TValue>(initialCapacity);
	}

	public void Add(TKey key, TValue value)
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

	public bool ContainsKey(TKey key) => TryFind(key, out _);

	public void RemoveKey(TKey key)
	{
		if (TryFind(key, out int index))
		{
			keys.RemoveAt(index);
			values.RemoveAt(index);
		}
		else
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFind(key, out int index);
		value = found ? values[index] : default!;

		return found;
	}

	private bool TryFind(TKey key, out int index)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			if (Comparer.Equal(key, keys[i]))
			{
				index = i;
				return true;
			}
		}

		index = -1;
		return false;
	}
}

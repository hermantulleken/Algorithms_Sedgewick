namespace Algorithms_Sedgewick.SymbolTable;

using System.Collections.Generic;
using List;


// Ex. 3.1.12
public class OrderedSymbolTableWithOrderedKeyArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	private readonly IComparer<TKey> comparer;
	
	public OrderedSymbolTableWithOrderedKeyArray(IComparer<TKey> comparer)
	{
		keys = new ResizeableArray<TKey>();
		values = new ResizeableArray<TValue>();
		this.comparer = comparer;
	}

	public int Count => keys.Count;
	public IEnumerable<TKey> Keys => keys;

	public TValue this[TKey key]
	{
		get
		{
			if (TryFindKey(key, out int index))
			{
				return values[index];
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (TryFindKey(key, out int index))
			{
				values[index] = value;
			}
			
			int newIndex = keys.InsertSorted(key, comparer);
			values.InsertAt(value, newIndex);
		}
	}

	private bool TryFindKey(TKey key, out int index)
	{
		index = keys.FindInsertionIndex(key, comparer);

		if (index == keys.Count)
		{
			index = -1;
			return false;
		}

		var insertionKey = keys[index];

		if (comparer.Equal(insertionKey, key))
		{
			return true;
		}

		index = -1;
		return false;
	}

	public void RemoveKey(TKey key)
	{
		if (TryFindKey(key, out int index))
		{
			keys.DeleteAt(index);
			values.DeleteAt(index);
		}
		
		ThrowHelper.ThrowKeyNotFound(key);
	}

	public bool ContainsKey(TKey key) => TryFindKey(key, out _);

	public TKey MinKey() => keys[0];

	public TKey MaxKey() => keys[^1];

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge casese
		int index = keys.FindInsertionIndex(key, comparer);

		return keys[index];
	}

	
	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge cases
		int index = keys.FindInsertionIndex(key, comparer);

		while (index < Count && comparer.Less(keys[index], key))
		{
			index++;
		}
		
		return keys[index];
	}

	public int RankOf(TKey key)
	{
		if (TryFindKey(key, out int index))
		{
			return index;
		}
		
		throw ThrowHelper.KeyNotFoundException(key);
	}

	//TODO verify index
	public TKey KeyWithRank(int rank) => keys[rank];

	public int CountRange(TKey start, TKey end)
	{
		if (TryFindKey(start, out int startIndex))
		{
			if (TryFindKey(end, out int endIndex))
			{
				return endIndex - startIndex + 1;
			}

			throw ThrowHelper.KeyNotFoundException(end);
		}

		throw ThrowHelper.KeyNotFoundException(start);
	}

	public IEnumerable<TKey> KeysRange(TKey start, TKey end)
	{
		if (TryFindKey(start, out int startIndex))
		{
			if (TryFindKey(end, out int endIndex))
			{
				//TODO Add range for array / random access list
				for (int i = startIndex; i < endIndex; i++)
				{
					yield return keys[i];
				}
			}

			throw ThrowHelper.KeyNotFoundException(end);
		}

		throw ThrowHelper.KeyNotFoundException(start);
	}
}

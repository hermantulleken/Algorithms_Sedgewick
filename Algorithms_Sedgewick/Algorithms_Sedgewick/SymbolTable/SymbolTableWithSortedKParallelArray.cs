namespace Algorithms_Sedgewick.SymbolTable;

using System.Collections.Generic;
using List;

public class SymbolTableWithSortedKParallelArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly ParallelArrays<TKey, TValue> arrays;

	private readonly IComparer<TKey> comparer;

	public int Count => arrays.Count;

	public TValue this[TKey key]
	{
		get
		{
			if (TryFindKey(key, out int index))
			{
				return arrays.Values[index];
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (TryFindKey(key, out int index))
			{
				arrays.Set(index, key, value);
			}
			
			int insertionIndex = arrays.Keys.FindInsertionIndex(key, comparer);
			arrays.InsertAt(insertionIndex, key, value);
		}
	}

	public IEnumerable<TKey> Keys => arrays.Keys;

	public SymbolTableWithSortedKParallelArray(IComparer<TKey> comparer)
	{
		arrays = new ParallelArrays<TKey, TValue>(100);
		this.comparer = comparer;
	}

	private bool TryFindKey(TKey key, out int index)
	{
		index = arrays.Keys.FindInsertionIndex(key, comparer);

		if (index == Count)
		{
			index = -1;
			return false;
		}

		var insertionKey = arrays.Keys[index];

		if (comparer.Equal(insertionKey, key))
		{
			return true;
		}

		index = -1;
		return false;
	}

	public bool ContainsKey(TKey key) => TryFindKey(key, out _);

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
					yield return arrays.Keys[i];
				}
			}

			throw ThrowHelper.KeyNotFoundException(end);
		}

		throw ThrowHelper.KeyNotFoundException(start);
	}

	//TODO verify index
	public TKey KeyWithRank(int rank) => arrays.Keys[rank];

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge casese
		int index = arrays.Keys.FindInsertionIndex(key, comparer);

		return arrays.Keys[index];
	}

	public TKey MaxKey() => arrays.Keys[^1];

	public TKey MinKey() => arrays.Keys[0];

	public int RankOf(TKey key)
	{
		if (TryFindKey(key, out int index))
		{
			return index;
		}
		
		throw ThrowHelper.KeyNotFoundException(key);
	}

	public void RemoveKey(TKey key)
	{
		if (TryFindKey(key, out int index))
		{
			arrays.DeleteAt(index);
		}
		
		ThrowHelper.ThrowKeyNotFound(key);
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge cases
		int index = arrays.Keys.FindInsertionIndex(key, comparer);

		while (index < Count && comparer.Less(arrays.Keys[index], key))
		{
			index++;
		}
		
		return arrays.Keys[index];
	}
}

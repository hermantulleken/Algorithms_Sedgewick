namespace Algorithms_Sedgewick.SymbolTable;

using System.Collections.Generic;
using List;

public class SymbolTableWithOrderedParallelArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
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

	public SymbolTableWithOrderedParallelArray(IComparer<TKey> comparer)
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
		int startIndex = RankOf(start);
		int endIndex = RankOf(end); 
		
		return endIndex - startIndex;
	}

	public IEnumerable<TKey> KeysRange(TKey start, TKey end)
	{
		int startIndex = RankOf(start);
		int endIndex = RankOf(end);
		
		for (int i = startIndex; i < endIndex; i++)
		{
			yield return arrays.Keys[i];
		}
	}

	//TODO verify index
	public TKey KeyWithRank(int rank) => arrays.Keys[rank];

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		int rank = arrays.Keys.BinaryRank(key, comparer);
		
		if (rank >= 0 && (rank < arrays.Keys.Count && comparer.Compare(arrays.Keys[rank], key) == 0))
		{
			// The key is in the list
			return arrays.Keys[rank];
		}
		
		if (rank > 0)
		{
			// The key is not in the list, but there are elements less than it
			return arrays.Keys[rank - 1];
		}
		
		throw new Exception("No keys less than given key.");
	}

	public TKey MaxKey() => arrays.Keys[^1];

	public TKey MinKey() => arrays.Keys[0];

	public int RankOf(TKey key) => arrays.Keys.BinaryRank(key, comparer);

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
		int rank = arrays.Keys.BinaryRank(key, comparer);
		
		if (rank < arrays.Keys.Count && comparer.Compare(arrays.Keys[rank], key) >= 0)
		{
			// The key is in the list or there is an element greater than it
			return arrays.Keys[rank];
		}
		
		if (rank < arrays.Keys.Count - 1)
		{
			// The key is not in the list, but there are elements greater than it
			return arrays.Keys[rank + 1];
		}

		throw new Exception("No keys greater than given key.");
	}
}

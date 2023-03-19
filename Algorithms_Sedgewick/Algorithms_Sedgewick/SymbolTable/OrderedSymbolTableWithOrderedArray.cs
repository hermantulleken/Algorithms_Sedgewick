using System.Collections.Generic;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.SymbolTable;

public class OrderedSymbolTableWithOrderedArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly ResizeableArray<KeyValuePair<TKey, TValue>> array;

	private readonly IComparer<TKey> comparer;
	private readonly IComparer<KeyValuePair<TKey, TValue>> pairComparer;

	public OrderedSymbolTableWithOrderedArray(IComparer<TKey> comparer)
	{
		array = new ResizeableArray<KeyValuePair<TKey, TValue>>();
		this.comparer = comparer;
		pairComparer = comparer.Convert<TKey, KeyValuePair<TKey, TValue>>(PairToKey);
	}

	public int Count => array.Count;
	public IEnumerable<TKey> Keys => array.Select(pair => pair.Key);

	public TValue this[TKey key]
	{
		get
		{
			if (TryFindKey(key, out int index))
			{
				return array[index].Value;
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (TryFindKey(key, out int index))
			{
				array[index] = new KeyValuePair<TKey, TValue>(key, value);
			}
			
			array.InsertSorted(new KeyValuePair<TKey, TValue>(key, value), pairComparer);
		}
	}

	private bool TryFindKey(TKey key, out int index)
	{
		var pair = new KeyValuePair<TKey, TValue>(key, default); 
		index = array.FindInsertionIndex(pair, pairComparer);

		if (index == array.Count)
		{
			index = -1;
			return false;
		}

		var insertionKey = array[index];

		if (comparer.Equal(key, insertionKey.Key))
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
			array.DeleteAt(index);
		}
		
		ThrowHelper.ThrowKeyNotFound(key);
	}

	public bool ContainsKey(TKey key) => TryFindKey(key, out _);

	public TKey MinKey() => array[0].Key;

	public TKey MaxKey() => array[^1].Key;

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge casese
		var pair = new KeyValuePair<TKey, TValue>(key, default);
		int index = array.FindInsertionIndex(pair, pairComparer);

		return array[index].Key;
	}

	
	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		//TODO: Handle edge cases
		var pair = new KeyValuePair<TKey, TValue>(key, default);
		int index = array.FindInsertionIndex(pair, pairComparer);

		while (index < Count && comparer.Less(array[index].Key, key))
		{
			index++;
		}
		
		return array[index].Key;
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
	public TKey KeyWithRank(int rank) => array[rank].Key;

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
					yield return array[i].Key;
				}
			}

			throw ThrowHelper.KeyNotFoundException(end);
		}

		throw ThrowHelper.KeyNotFoundException(start);
	}

	//TODO: Move somewhere more central
	internal static TKey PairToKey(KeyValuePair<TKey, TValue> pair) => pair.Key;
}

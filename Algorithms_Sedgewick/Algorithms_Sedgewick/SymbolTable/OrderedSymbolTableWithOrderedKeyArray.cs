namespace Algorithms_Sedgewick.SymbolTable;

using System.Collections.Generic;
using List;

// Ex. 3.1.12
public class OrderedSymbolTableWithOrderedKeyArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	public int Count => keys.Count;

	public TValue this[TKey key]
	{
		get => AsSymbolTable[key];
		set => AsSymbolTable[key] = value;
	}

	public IEnumerable<TKey> Keys => keys;

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	public OrderedSymbolTableWithOrderedKeyArray(IComparer<TKey> comparer)
	{
		keys = new ResizeableArray<TKey>();
		values = new ResizeableArray<TValue>();
		this.comparer = comparer;
	}

	public void Add(TKey key, TValue value)
	{
		throw new NotImplementedException();
	}

	public bool ContainsKey(TKey key) 
		=> TryFindKey(key, out _);

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

		return keys
			.Skip(startIndex)
			.Take(endIndex - startIndex);
	}

	// TODO verify index
	public TKey KeyWithRank(int rank) => keys[rank];

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		// TODO: Handle edge casese
		int index = keys.BinaryRank(key, comparer);

		return comparer.Equal(keys[index], key) ? keys[index] : keys[index - 1];
	}

	public TKey MaxKey() => keys[^1];

	public TKey MinKey() => keys[0];

	public int RankOf(TKey key) => keys.BinaryRank(key, comparer);

	public void RemoveKey(TKey key)
	{
		if (TryFindKey(key, out int index))
		{
			keys.DeleteAt(index);
			values.DeleteAt(index);
		}
		else
		{
			ThrowHelper.ThrowKeyNotFound(key);
		}
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		// TODO: Handle edge cases
		int index = keys.BinaryRank(key, comparer);

		while (index < Count && comparer.Less(keys[index], key))
		{
			index++;
		}
		
		return keys[index];
	}

	public bool TryGetValue(TKey key, out TValue value) => throw new NotImplementedException();

	private bool TryFindKey(TKey key, out int index)
	{
		index = keys.BinarySearch(key, comparer);

		return index != -1;
	}
}

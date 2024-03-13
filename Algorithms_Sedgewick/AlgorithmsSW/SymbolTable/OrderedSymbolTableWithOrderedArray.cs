namespace AlgorithmsSW.SymbolTable;

using List;

public class OrderedSymbolTableWithOrderedArray<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly ResizeableArray<KeyValuePair<TKey, TValue>> array;
	private readonly IComparer<KeyValuePair<TKey, TValue>> pairComparer;

	public int Count => array.Count;
	
	public IComparer<TKey> Comparer { get; }

	public IEnumerable<TKey> Keys => array.Select(pair => pair.Key);

	public OrderedSymbolTableWithOrderedArray(IComparer<TKey> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	public OrderedSymbolTableWithOrderedArray(int initialCapacity, IComparer<TKey> comparer)
	{
		array = new ResizeableArray<KeyValuePair<TKey, TValue>>(initialCapacity);
		this.Comparer = comparer;
		pairComparer = new PairComparer<TKey, TValue>(comparer);
	}

	public void Add(TKey key, TValue value)
	{
		if (TryFindKey(key, out int index))
		{
			array[index] = new KeyValuePair<TKey, TValue>(key, value);
			return;
		}
			
		array.InsertSorted(new KeyValuePair<TKey, TValue>(key, value), pairComparer);
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
			yield return array[i].Key;
		}
	}

	// TODO verify index
	public TKey KeyWithRank(int rank) => array[rank].Key;

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		int rank = array.BinaryRank(KeyToPair(key), pairComparer);
		
		if (rank >= 0 && (rank < array.Count && Comparer.Compare(array[rank].Key, key) == 0))
		{
			// The key is in the list
			return array[rank].Key;
		}
		
		if (rank > 0)
		{
			// The key is not in the list, but there are elements less than it
			return array[rank - 1].Key;
		}
		
		throw new Exception("No keys less than given key.");
	}

	public TKey MaxKey() => array[^1].Key;

	public TKey MinKey() => array[0].Key;

	public int RankOf(TKey key) => array.BinaryRank(KeyToPair(key), pairComparer);

	public void RemoveKey(TKey key)
	{
		if (!TryFindKey(key, out int index))
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}

		array.RemoveAt(index);
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		int rank = array.BinaryRank(KeyToPair(key), pairComparer);
		
		if (rank < array.Count && Comparer.Compare(array[rank].Key, key) >= 0)
		{
			// The key is in the list or there is an element greater than it
			return array[rank].Key;
		}
		
		if (rank < array.Count - 1)
		{
			// The key is not in the list, but there are elements greater than it
			return array[rank + 1].Key;
		}

		throw new Exception("No keys greater than given key.");
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFindKey(key, out int index);
		
		if (found)
		{
			value = array[index].Value;
		}
		else
		{
			value = default!;
		}

		return found;
	}

	internal static KeyValuePair<TKey, TValue> KeyToPair(TKey key) => new(key, default!);

	// TODO: Move somewhere more central
	internal static TKey PairToKey(KeyValuePair<TKey, TValue> pair) => pair.Key;

	private bool TryFindKey(TKey key, out int index)
	{
		var pair = new KeyValuePair<TKey, TValue>(key, default!); 
		index = array.BinarySearch(pair, pairComparer);

		return index != -1;
	}
}

namespace AlgorithmsSW.SymbolTable;

using List;

public class SymbolTableWithParallelArrays<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly ParallelArrays<TKey, TValue> arrays;
	private readonly IComparer<TKey> comparer;

	public int Count => arrays.Count;

	public IEnumerable<TKey> Keys => arrays.Keys;

	public SymbolTableWithParallelArrays(IComparer<TKey> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	public SymbolTableWithParallelArrays(int initialCapacity, IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		arrays = new ParallelArrays<TKey, TValue>(initialCapacity);
	}

	public void Add(TKey key, TValue value)
	{
		if (TryFind(key, out int index))
		{
			arrays.Set(index, key, value);
		}
		else
		{
			arrays.Add(key, value);
		}
	}

	public bool ContainsKey(TKey key) => TryFind(key, out _);

	public void RemoveKey(TKey key)
	{
		if (TryFind(key, out int index))
		{
			arrays.DeleteAt(index);
		}
		else
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFind(key, out int index);
		value = found ? arrays.Values[index] : default!;
		return found;
	}

	private bool TryFind(TKey key, out int index)
	{
		for (int i = 0; i < arrays.Count; i++)
		{
			if (comparer.Equal(key, arrays.Keys[i]))
			{
				index = i;
				return true;
			}
		}

		index = -1;
		return false;
	}
}

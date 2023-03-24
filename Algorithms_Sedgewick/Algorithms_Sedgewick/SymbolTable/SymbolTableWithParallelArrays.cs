namespace Algorithms_Sedgewick.SymbolTable;

using List;

public class SymbolTableWithParallelArrays<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly ParallelArrays<TKey, TValue> arrays;
	private readonly IComparer<TKey> comparer;

	public int Count => arrays.Count;

	public TValue this[TKey key]
	{
		get
		{
			if (TryFind(key, out int index))
			{
				return arrays.Values[index];
			}
			
			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
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
	}

	public IEnumerable<TKey> Keys => arrays.Keys;

	public SymbolTableWithParallelArrays(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		arrays = new ParallelArrays<TKey, TValue>(100);
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

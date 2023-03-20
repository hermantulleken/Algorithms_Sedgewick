namespace Algorithms_Sedgewick.SymbolTable;

using List;

// Ex. 3.1.2
public class SymbolTableWithKeyArray<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	// TODO: Consider a parallel array structure
	private ResizeableArray<TKey> keys;
	private ResizeableArray<TValue> values;
	private IComparer<TKey> comparer;
	
	public SymbolTableWithKeyArray(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		keys = new ResizeableArray<TKey>();
		values = new ResizeableArray<TValue>();
	}

	public int Count => keys.Count;
	
	public IEnumerable<TKey> Keys => keys;

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
			
			keys.Add(key);
			values.Add(value);
		}
	}

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

	public bool ContainsKey(TKey key) => TryFind(key, out _);

	private bool TryFind(TKey key, out int index)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			if (comparer.Equals(key))
			{
				index = i;
				return true;
			}
		}

		index = -1;
		return false;
	}
}

namespace Algorithms_Sedgewick.SymbolTable;

using List;

// Ex. 3.1.2
public class SymbolTableWithSelfOrderingKeyArray<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	// TODO: Consider a parallel array structure
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;
	private readonly IComparer<TKey> comparer;
	
	public SymbolTableWithSelfOrderingKeyArray(IComparer<TKey> comparer)
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
				MoveToFrontAt(index);
				return values[0];
			}
			
			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (TryFind(key, out int index))
			{
				values[index] = value;
				MoveToFrontAt(index);
				
				return;
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

	private void MoveToFrontAt(int index)
	{
		if (index != 0)
		{
			Sort.SwapAt(keys, 0, index);
			Sort.SwapAt(values, 0, index);
		}
	}
}

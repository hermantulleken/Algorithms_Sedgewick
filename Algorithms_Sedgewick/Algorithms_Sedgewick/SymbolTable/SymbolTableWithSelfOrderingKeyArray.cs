using static Algorithms_Sedgewick.List.ListExtensions;

namespace Algorithms_Sedgewick.SymbolTable;

using List;

// Ex. 3.1.2
public class SymbolTableWithSelfOrderingKeyArray<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;

	// TODO: Consider a parallel array structure
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	public int Count => keys.Count;

	public IEnumerable<TKey> Keys => keys;

	public SymbolTableWithSelfOrderingKeyArray(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		keys = new ResizeableArray<TKey>();
		values = new ResizeableArray<TValue>();
	}

	public void Add(TKey key, TValue value)
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

	public bool ContainsKey(TKey key) => TryFind(key, out _);

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

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFind(key, out int index);
		value = found ? values[index]! : default!;
		
		if (found)
		{
			MoveToFrontAt(index);
		}
		
		return found;
	}

	private void MoveToFrontAt(int index)
	{
		if (index == 0)
		{
			return;
		}
		
		SwapAt(keys, 0, index);
		SwapAt(values, 0, index);
	}

	private bool TryFind(TKey key, out int index)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			if (comparer.Equal(keys[i], key))
			{
				index = i;
				return true;
			}
		}

		index = -1;
		return false;
	}
}

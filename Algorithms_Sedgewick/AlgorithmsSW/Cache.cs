namespace AlgorithmsSW;

using SymbolTable;

/// <summary>
/// Class that caches values for keys.
/// </summary>
/// <typeparam name="TKey">The type of the keys.</typeparam>
/// <typeparam name="TValue">The type of the values.</typeparam>
public class Cache<TKey, TValue>
{
	private readonly ISymbolTable<TKey, TValue> cache;
	private readonly Func<TKey, TValue> valueFactory;
	
	/// <summary>
	/// Retrieves the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key whose value to get.</param>
	/// <remarks>If the key does not exist in the cache, the value is created using the value factory and added to the
	/// cache.</remarks>
	public TValue this[TKey key]
	{
		get
		{
			if (cache.TryGetValue(key, out var value))
			{
				return value;
			}

			value = valueFactory(key);
			cache[key] = value;
			return value;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Cache{TKey,TValue}"/> class.
	/// </summary>
	/// <param name="valueFactory">The function to use to create values for keys.</param>
	/// <param name="comparer">The comparer to use for comparing keys.</param>
	public Cache(Func<TKey, TValue> valueFactory, IComparer<TKey> comparer)
	{
		cache = DataStructures.HashTable<TKey, TValue>(comparer);
		this.valueFactory = valueFactory;
	}

	/// <summary>
	/// Clears the cache.
	/// </summary>
	public void Clear() => cache.Clear();
}

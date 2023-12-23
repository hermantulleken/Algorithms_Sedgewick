namespace Benchmarks;

using AlgorithmsSW.SymbolTable;

public class Cache<TKey, TValue>
{
	private readonly ISymbolTable<TKey, TValue> cache;
	private readonly Func<TKey, TValue> valueFactory;

	public Cache(ISymbolTable<TKey, TValue> cache, Func<TKey, TValue> valueFactory)
	{
		this.cache = cache;
		this.valueFactory = valueFactory;
	}

	public TValue Get(TKey key)
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
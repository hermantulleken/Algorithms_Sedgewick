namespace Algorithms_Sedgewick.HashTable;

using System.Diagnostics.CodeAnalysis;
using SymbolTable;

public class HashTableWithSeparateChaining<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly ISymbolTable<TKey, TValue>[] table;
	private readonly int tableSize;

	public int Count => table.Select(t => t.Count).Sum();

	public IEnumerable<TKey> Keys 
		=> table.SelectMany(st => st.Keys);

	public HashTableWithSeparateChaining(IComparer<TKey> comparer)
		: this(997, comparer)
	{
	}

	public HashTableWithSeparateChaining(int tableSize, IComparer<TKey> comparer)
	{ 
		this.tableSize = tableSize;
		table = new ISymbolTable<TKey, TValue>[tableSize];
		
		for (int i = 0; i < tableSize; i++)
		{
			table[i] = new SymbolTableWithKeyArray<TKey, TValue>(comparer);
		}
	}

	public void Add(TKey key, TValue value)
	{
		key.ThrowIfNull();
		table[GetHash(key)][key] = value;
	}

	public bool ContainsKey(TKey key)
	{
		key.ThrowIfNull();
		
		return table[GetHash(key)].ContainsKey(key);
	}

	public void RemoveKey(TKey key)
	{
		key.ThrowIfNull();
		table[GetHash(key)].RemoveKey(key);
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		key.ThrowIfNull();
		
		return table[GetHash(key)].TryGetValue(key, out value);
	}
	
#if WHITEBOXTESTING
	// 3.4.30
	public double ChiSquare()
	{
		double Sqr(double x) => x * x;
		
		double fractionCountOfTableSize = Count / (double)tableSize;
		return table
			.Select(t => Sqr(t.Count - fractionCountOfTableSize))
			.Sum()
			* tableSize / Count;
	}
#endif

	private int GetHash([DisallowNull]TKey key)
	{
		key.ThrowIfNull();
		
		return key.GetHashCode() % tableSize;
	}
}

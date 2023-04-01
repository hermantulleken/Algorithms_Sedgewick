namespace Algorithms_Sedgewick.HashTable;

using System.Diagnostics.CodeAnalysis;
using SymbolTable;

public class HashTableWithSeparateChaining2<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly SymbolTableWithKeyArray<TKey, TValue>[] table;
	private readonly int tableSize;

	public int Count => table.Select(t => t.Count).Sum();

	public IEnumerable<TKey> Keys 
		=> table.SelectMany(st => st.Keys);

	public HashTableWithSeparateChaining2(IComparer<TKey> comparer)
		: this(997, comparer)
	{
	}

	public HashTableWithSeparateChaining2(int tableSize, IComparer<TKey> comparer)
	{ 
		this.tableSize = tableSize;
		table = new SymbolTableWithKeyArray<TKey, TValue>[tableSize];
		
		for (int i = 0; i < tableSize; i++)
		{
			table[i] = new SymbolTableWithKeyArray<TKey, TValue>(comparer);
		}
	}

	public void Add(TKey key, TValue value)
	{
		key.ThrowIfNull();
		
		var table1 = GetTable1(key);
		var table2 = GetTable2(key);

		var tableToAddTo = table1.Count < table2.Count ? table2 : table1;
		
		tableToAddTo[key] = value;
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

	public void RemoveKey(TKey key)
	{
		key.ThrowIfNull();
		
		var table1 = GetTable1(key);
		var table2 = GetTable2(key);

		if (table1.ContainsKey(key))
		{
			table1.RemoveKey(key);
		}
		else
		{
			table2.RemoveKey(key);
		}
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		key.ThrowIfNull();

		var table1 = GetTable1(key);
		var table2 = GetTable2(key);

		return table1.TryGetValue(key, out value) || table2.TryGetValue(key, out value);
	}

	private int GetHash([DisallowNull]TKey key, int keyMultiplayer) => keyMultiplayer * key.GetHashCode() % tableSize;

	private int GetHash1([DisallowNull]TKey key) => GetHash(key, 11);

	private int GetHash2([DisallowNull] TKey key) => GetHash(key, 13);

	private ISymbolTable<TKey, TValue> GetTable1([DisallowNull] TKey key) => table[GetHash1(key)];

	private ISymbolTable<TKey, TValue> GetTable2([DisallowNull] TKey key) => table[GetHash2(key)];
}

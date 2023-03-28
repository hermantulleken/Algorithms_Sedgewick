namespace Algorithms_Sedgewick.HashTable;

using System.Diagnostics.CodeAnalysis;
using SymbolTable;

public class HashTableWithSeparateChaining<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly SymbolTableWithKeyArray<TKey, TValue>[] table; 
	private readonly int tableSize;

	public int Count => table.Select(t => t.Count).Sum();

	public HashTableWithSeparateChaining(IComparer<TKey> comparer)
		: this(997, comparer)
	{
	}
	
	public HashTableWithSeparateChaining(int tableSize, IComparer<TKey> comparer)
	{ 
		this.tableSize = tableSize;
		table = new SymbolTableWithKeyArray<TKey, TValue>[tableSize];
		
		for (int i = 0; i < tableSize; i++)
		{
			table[i] = new SymbolTableWithKeyArray<TKey, TValue>(comparer);
		}
	}

	private int GetHash([DisallowNull]TKey key)
	{
		key.ThrowIfNull();
		
		return key.GetHashCode() % tableSize;
	}

	public TValue this[TKey key]
	{
		get
		{
			key.ThrowIfNull();
			
			return table[GetHash(key)][key];
		}
		
		set
		{
			key.ThrowIfNull();
			table[GetHash(key)][key] = value;
		}
	}

	public IEnumerable<TKey> Keys 
		=> table.SelectMany(st => st.Keys);

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
}

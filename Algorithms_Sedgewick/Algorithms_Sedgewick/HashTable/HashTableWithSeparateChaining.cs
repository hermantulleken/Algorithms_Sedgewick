namespace Algorithms_Sedgewick.HashTable;

using System.Diagnostics.CodeAnalysis;
using SymbolTable;

public class SeparateChainingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly SymbolTableWithKeyArray<TKey, TValue>[] table;
	private readonly int tableSize;

	public int Count => table.Select(t => t.Count).Sum();

	public TValue this[TKey key]
	{
		get => AsSymbolTable[key];
		set => AsSymbolTable[key] = value;
	}

	public IEnumerable<TKey> Keys 
		=> table.SelectMany(st => st.Keys);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	public SeparateChainingHashTable(IComparer<TKey> comparer)
		: this(997, comparer)
	{
	}

	public SeparateChainingHashTable(int tableSize, IComparer<TKey> comparer)
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

	public bool TryGetValue(TKey key, out TValue value)
	{
		key.ThrowIfNull();
		
		return table[GetHash(key)].TryGetValue(key, out value);
	}

	private int GetHash([DisallowNull]TKey key)
	{
		key.ThrowIfNull();
		
		return key.GetHashCode() % tableSize;
	}
}

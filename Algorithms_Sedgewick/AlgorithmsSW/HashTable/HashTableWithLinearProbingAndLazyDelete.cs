namespace AlgorithmsSW.HashTable;

using System.Diagnostics.CodeAnalysis;
using SymbolTable;

/*
	This class represents a hash table that uses linear probing to resolve collisions and employs a lazy deletion strategy.
	Lazy deletion means that when a key is removed from the table, it is not immediately physically removed from the data structure.
	Instead, the key is marked for removal, and its slot in the table is considered as available for new insertions.
	
	The advantage of using lazy deletion is that it helps reduce the number of reinsertions of other keys in the same cluster
	when a key is removed, as the removed key still occupies its original slot, maintaining the integrity of the linear probing sequence.
	
	However, this approach may lead to increased memory usage, as the marked entries still consume space in the table.
	To address this issue, the table is resized and rehashed when the number of marked entries reaches a certain threshold,
	clearing all marked entries in the process.
	
	Ex. 3.4.26
*/
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class HashTableWithLinearProbingAndLazyDelete<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private enum Presence
	{
		NotPresent,
		Present,
		MarkedForRemoval,
	}

	private readonly IComparer<TKey> comparer;
	private Presence[] keyPresent; // Necessary if TKey is a value type
	private TKey[] keys;
	private int keysMarkedForRemovalCount;
	private int log2TableSize;
	private int tableSize;
	private TValue[] values;

	public int Count { get; private set; }

	public IEnumerable<TKey> Keys 
		=> keyPresent
			.IndexWhere(presence => presence == Presence.Present)
			.Select(index => keys[index]);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	public HashTableWithLinearProbingAndLazyDelete(IComparer<TKey> comparer)
		: this(4, comparer)
	{
	}

	public HashTableWithLinearProbingAndLazyDelete(int log2TableSize, IComparer<TKey> comparer)
	{
		tableSize = 1 << log2TableSize;
		this.log2TableSize = log2TableSize;
		this.comparer = comparer;
		keys = new TKey[tableSize];
		values = new TValue[tableSize];
		keyPresent = new Presence[tableSize];
		Array.Fill(keyPresent, Presence.NotPresent);

		keysMarkedForRemovalCount = 0;
	}

	public void Add(TKey key, TValue value)
	{
		if (Count >= tableSize / 2)
		{
			// The number of present entries is too much, so make the table bigger 
			Resize(log2TableSize + 1); // Doubles the size
		}
		else if (Count + keysMarkedForRemovalCount >= tableSize / 2)
		{
			// The number of marked entries is too much, so keep the size but re-hash and set removed keys to non-present
			Resize(log2TableSize);
		}

		int index = IndexOf(key);

		if (index >= 0)
		{
			values[index] = value;
		}
		else
		{
			SetAt(~index, key, value, Presence.Present);
			Count++;
		}
	}

	public bool ContainsKey(TKey key) => IndexOf(key) >= 0;

	public void RemoveKey(TKey key)
	{
		int index = IndexOf(key);

		if (index < 0)
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}

		RemoveKeyAt(index);

		if (Count > 0 && Count == tableSize / 8)
		{
			Resize(log2TableSize - 1);
		}
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		int index = IndexOf(key);
		bool found = index >= 0;
		value = found ? values[index] : default!;

		return found;
	}

	private int GetHash(TKey key)
	{
		key.ThrowIfNull();
		int hashCode = key.GetHashCode();
		
		if (log2TableSize < 26)
		{
			hashCode %= HashTableWithLinearProbing.Primes[log2TableSize + 5];
		}
		
		return hashCode % tableSize;
	}

	private void GetNextIndex(ref int index) => index = (index + 1) % tableSize;

	/*
		Iterate through the table, starting from the hash value of the key and moving linearly.
		The loop continues until an empty slot is found (meaning the key is not present in the table).
		
		Returns a negative value if the index is not found. The complement of this value (i.e. ~IndexOf(key)) is where a new 
		element can be inserted. This can be inside a cluster if an element in the cluster has bene marked for removal. 
	*/
	private int IndexOf(TKey key)
	{
		int indexToAdd = -1;
		int index;
		
		for (index = GetHash(key); keyPresent[index] != Presence.NotPresent; GetNextIndex(ref index))
		{
			if (indexToAdd < 0 && keyPresent[index] == Presence.MarkedForRemoval)
			{
				indexToAdd = index;
			}
			else if (comparer.Equal(keys[index], key))
			{
				return index;
			}
		}

		return (indexToAdd < 0) ? ~index : ~indexToAdd;
	}

	private void RemoveKeyAt(int index)
	{
		SetAt(index, default!, default!, Presence.MarkedForRemoval);
		Count--;
		keysMarkedForRemovalCount++;
	}

	private void Resize(int newLog2TableSize) // See page 474.
	{
		var newTable = new HashTableWithLinearProbingAndLazyDelete<TKey, TValue>(newLog2TableSize, comparer);
		
		for (int i = 0; i < tableSize; i++)
		{
			if (keyPresent[i] == Presence.Present)
			{
				newTable.AsSymbolTable[keys[i]] = values[i];
			}
		}
		
		keys = newTable.keys;
		values = newTable.values;
		keyPresent = newTable.keyPresent;
		log2TableSize = newLog2TableSize;
		tableSize = newTable.tableSize;
		keysMarkedForRemovalCount = 0;
	}

	private void SetAt(int index, TKey key, TValue value, Presence presence)
	{
		keys[index] = key;
		values[index] = value;
		keyPresent[index] = presence;
	}
}

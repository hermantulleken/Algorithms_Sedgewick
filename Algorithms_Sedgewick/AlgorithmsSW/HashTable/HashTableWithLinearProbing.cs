using System.Diagnostics.CodeAnalysis;

namespace AlgorithmsSW.HashTable;

using SymbolTable;

public static class HashTableWithLinearProbing
{
	internal static readonly int[] Primes =
	{
		31,
		61,
		127,
		251,
		509,
		1021,
		2039,
		4093,
		8191,
		16381,
		65521,
		131071,
		262139,
		524287,
		1048573,
		2097143,
		4194301,
		8388593,
		16777213,
		33554393,
		67108859,
		134217689,
		268435399,
		536870909,
		1073741789,
		2147483647,
	};

	public static (int log2TableSize, int taleSize) GetTableSize(int initialCapacity)
	{
		int log2TableSize = Math.Max(Math2.IntegerCeilLog2(initialCapacity) - 4, 0);
		int tableSize = Primes[log2TableSize];

		return (log2TableSize, tableSize);
	}
}

/*
	This class represents a hash table that uses linear probing to resolve collisions.
	Linear probing is an open-addressing strategy where we look for the next available slot
	in the array when a collision occurs.
*/
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class HashTableWithLinearProbing<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private bool[] keyPresent; // Necessary if TKey is a value type
	private TKey[] keys;
	private int log2TableSize;
	private int tableSize;
	private TValue[] values;

	public int Count { get; private set; }

	public IEnumerable<TKey> Keys 
		=> keyPresent
			.IndexWhere(Algorithms.Identity)
			.Select(index => keys[index]);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	public HashTableWithLinearProbing(IComparer<TKey> comparer)
		: this(4, comparer)
	{
	}

	public HashTableWithLinearProbing(int log2TableSize, IComparer<TKey> comparer)
	{
		tableSize = 1 << log2TableSize;
		this.log2TableSize = log2TableSize;
		this.comparer = comparer;
		keys = new TKey[tableSize];
		values = new TValue[tableSize];
		keyPresent = new bool[tableSize];
	}

	public void Add(TKey key, TValue value)
	{
		if (Count >= tableSize / 2)
		{
			Resize(log2TableSize + 1); // Doubles the size
		}

		int index = IndexOf(key);

		if (index < 0)
		{
			SetAt(~index, key, value, true);
			Count++;
		}
		else
		{
			values[index] = value;
		}
	}

	public bool ContainsKey(TKey key) => IndexOf(key) >= 0;

	public void RemoveKey(TKey key)
	{
		void ReinsertAt(int index)
		{
			var keyToRedo = keys[index];
			var valueToRedo = values[index];
			RemoveKeyAt(index);
			AsSymbolTable[keyToRedo] = valueToRedo;
		}

		int index = IndexOf(key);

		if (index < 0)
		{
			throw ThrowHelper.KeyNotFoundException(key);
		}

		RemoveKeyAt(index);

		/*
		    Reinsert all the keys in the same cluster as the removed key.
		    This is necessary because their positions might have been affected by the removal of the key.
		*/
		for (GetNextIndex(ref index); keyPresent[index]; GetNextIndex(ref index))
		{
			ReinsertAt(index);
		}
		
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
		
		if (log2TableSize + 5 < 26)
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
		element can be inserted.
	*/
	private int IndexOf(TKey key)
	{
		int i;
		for (i = GetHash(key); keyPresent[i]; GetNextIndex(ref i))
		{
			if (comparer.Equal(keys[i], key))
			{
				return i;
			}
		}

		return ~i;
	}

	private void RemoveKeyAt(int index)
	{
		SetAt(index, default!, default!, false);
		Count--;
	}

	private void Resize(int newLog2TableSize) // See page 474.
	{
		var newTable = new HashTableWithLinearProbing<TKey, TValue>(newLog2TableSize, comparer);
		
		for (int i = 0; i < tableSize; i++)
		{
			if (keyPresent[i])
			{
				newTable.AsSymbolTable[keys[i]] = values[i];
			}
		}
		
		keys = newTable.keys;
		values = newTable.values;
		keyPresent = newTable.keyPresent;
		log2TableSize = newLog2TableSize;
		tableSize = newTable.tableSize;
	}

	private void SetAt(int index, TKey key, TValue value, bool present)
	{
		keys[index] = key;
		values[index] = value;
		keyPresent[index] = present;
	}
}

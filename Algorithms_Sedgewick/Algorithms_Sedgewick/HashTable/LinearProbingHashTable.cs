using System.Diagnostics.CodeAnalysis;

namespace Algorithms_Sedgewick.HashTable;

using SymbolTable;

public static class LinearProbingHashTable
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
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class LinearProbingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private TKey[] keys;
	private int tableSize;
	private int log2TableSize;
	private TValue[] values;
	private bool[] keyPresent; // Necessary if TKey is a value type

	public int Count { get; private set; }

	public TValue this[TKey key]
	{
		get
		{
			for (int i = GetHash(key); keyPresent[i]; i = (i + 1) % tableSize)
			{
				if (comparer.Equal(keys[i], key))
				{
					return values[i];
				}
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (Count >= tableSize / 2)
			{
				Resize(log2TableSize = 1); // double M (see text)
			}

			int i;
			for (i = GetHash(key); keyPresent[i]; i = (i + 1) % tableSize)
			{
				if (comparer.Equal(keys[i], key))
				{
					values[i] = value;
					keyPresent[i] = true;
					
					return;
				}
			}

			keys[i] = key;
			values[i] = value;
			keyPresent[i] = true;
			
			Count++;
		}
	}

	public IEnumerable<TKey> Keys => throw new NotImplementedException();

	public LinearProbingHashTable(IComparer<TKey> comparer)
		: this(4, comparer)
	{
	}

	public LinearProbingHashTable(int log2TableSize, IComparer<TKey> comparer)
	{
		tableSize = 1 << log2TableSize;
		this.log2TableSize = log2TableSize;
		this.comparer = comparer;
		keys = new TKey[tableSize];
		values = new TValue[tableSize];
		keyPresent = new bool[tableSize];
	}

	public bool ContainsKey(TKey key)
	{
		for (int i = GetHash(key); keyPresent[i]; i = (i + 1) % tableSize)
		{
			if (comparer.Equal(keys[i], key))
			{
				return true;
			}
		}

		return false;
	}

	public void RemoveKey(TKey key)
	{
		if (!ContainsKey(key)) 
		{
			return; // TODO: Throw
		}
		
		int i = GetHash(key);
		
		while (!comparer.Equal(key, keys[i]))
		{
			i = (i + 1) % tableSize;
		}
		
		keys[i] = default!;
		values[i] = default!;
		keyPresent[i] = false;
		
		i = (i + 1) % tableSize;
		
		while (keyPresent[i])
		{
			var keyToRedo = keys[i];
			var valToRedo = values[i];
			
			keys[i] = default!;
			values[i] = default!;
			keyPresent[i] = false;
			
			Count--;
			this[keyToRedo] = valToRedo;
			i = (i + 1) % tableSize;
		}
		
		Count--;
		
		if (Count > 0 && Count == tableSize / 8)
		{
			Resize(log2TableSize - 1);
		}
	}

	private int GetHash(TKey key)
	{
		key.ThrowIfNull();

		int t = key.GetHashCode();
		
		if (log2TableSize < 26)
		{
			t = t % LinearProbingHashTable.Primes[log2TableSize + 5];
		}
		
		return t % tableSize;
	}

	private void Resize(int newLog2TableSize) // See page 474.
	{
		var newTable = new LinearProbingHashTable<TKey, TValue>(newLog2TableSize, comparer);
		
		for (int i = 0; i < tableSize; i++)
		{
			if (keyPresent[i])
			{
				newTable[keys[i]] = values[i];
			}
		}
		
		keys = newTable.keys;
		values = newTable.values;
		keyPresent = newTable.keyPresent;
		log2TableSize = newLog2TableSize;
		tableSize = newTable.tableSize;
	}
}

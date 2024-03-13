using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW.List;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.HashTable;

using SymbolTable;

/// <summary>
/// Represents a Cuckoo hash table implementation that stores key-value pairs.
/// </summary>
/// <remarks>
/// Cuckoo hashing allows for constant time complexity in the average case for search, insert, and delete operations.
/// It uses two hash functions and two tables to resolve collisions. This implementation provides methods for adding, 
/// removing, and checking the presence of keys.
/// </remarks>
/// <typeparam name="TKey">The type of keys in the hash table.</typeparam>
/// <typeparam name="TValue">The type of values in the hash table.</typeparam>
public class CuckooHashTable<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private const int MaxSteps = 5;
	private const int Prime1 = 31;
	private const int Prime2 = 37;
	
	private bool[] keyPresent1; // Necessary if TKey is a value type
	private bool[] keyPresent2; // TODO: Maybe presence and tables should be their own class?

	private int log2TableSize;

	private ParallelArrays<TKey?, TValue?> table1;
	private ParallelArrays<TKey?, TValue?> table2;
	private int halfTableSize;

	/// <inheritdoc />
	public IComparer<TKey> Comparer { get; }
	
	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc />
	public IEnumerable<TKey> Keys
	{
		get
		{
			return GetKeysFromTable(table1.Keys, keyPresent1)
				.Concat(GetKeysFromTable(table2.Keys, keyPresent2));

			IEnumerable<TKey> GetKeysFromTable(IReadonlyRandomAccessList<TKey?> keys, bool[] keyPresence)
			{
				return keyPresence
					.IndexWhere(Algorithms.Identity)
					.Select(GetNonNullKey);

				TKey GetNonNullKey(int index)
				{
					var key = keys[index];
					Assert(key != null);
					return key;
				}
			}
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CuckooHashTable{TKey, TValue}"/> class with the specified comparer.
	/// </summary>
	/// <param name="comparer">The <see cref="IComparer{TKey}"/> to use for comparing keys.</param>
	public CuckooHashTable(IComparer<TKey> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CuckooHashTable{TKey, TValue}"/> class with the specified table size and comparer.
	/// </summary>
	/// <param name="tableSize">The initial size of the table.</param>
	/// <param name="comparer">The <see cref="IComparer{TKey}"/> to use for comparing keys.</param>
	public CuckooHashTable(int tableSize, IComparer<TKey> comparer)
	{
		log2TableSize = MathX.IntegerCeilLog2(tableSize);
		halfTableSize = 1 << (log2TableSize - 1); // Note: Half the size since we use two tables ;)
		this.Comparer = comparer;
		
		void InitTable(out ParallelArrays<TKey?, TValue?> table)
		{
			table = new ParallelArrays<TKey?, TValue?>(halfTableSize);
			CuckooHashTable<TKey, TValue>.FillTable(table, halfTableSize);
		}
		
		InitTable(out table1);
		InitTable(out table2);
		
		keyPresent1 = new bool[halfTableSize];
		keyPresent2 = new bool[halfTableSize];
	}
	
	/// <inheritdoc />
	public void Add(TKey key, TValue value)
	{
		if (TryGetIndex(key, out var table, out int index))
		{
			table.Set(index, key, value);
			return;
		}
		
		if (Count >= halfTableSize)
		{
			Resize(halfTableSize * 4); // Doubles the size
		}

		index = GetHash1(key);

		for (int i = 0; i < MaxSteps; i++)
		{
			ValidateIndex(index, keyPresent1);
			
			if (!keyPresent1[index])
			{
				AddKeyAt(table1, keyPresent1, index, key, value);
				return;
			}

			KickKeyFromTable(table1, index, ref key, ref value);
			index = GetHash2(key);

			ValidateIndex(index, keyPresent2);
			
			if (keyPresent2[index])
			{
				KickKeyFromTable(table2, index, ref key, ref value);
				index = GetHash1(key);
			}
			else
			{
				AddKeyAt(table2, keyPresent2, index, key, value);
				return;
			}
		}

		Resize(halfTableSize * 4);
		Add(key, value);
	}

	/// <inheritdoc />
	public bool ContainsKey(TKey key) => TryGetValue(key, out _);

	/// <inheritdoc />
	public void RemoveKey(TKey key)
	{
		int index = GetHash1(key);
		ValidateIndex(index, keyPresent1);
		if (keyPresent1[index] && Comparer.Equal(table1.Keys[index]!, key))
		{
			RemoveKeyAt(table1, index);
		}
		else
		{
			index = GetHash2(key);
			ValidateIndex(index, keyPresent2);
			if (keyPresent2[index] && Comparer.Equal(table2.Keys[index]!, key))
			{
				RemoveKeyAt(table2, index);
			}
			else
			{
				throw ThrowHelper.KeyNotFoundException(key);
			}
		}
	}

	/// <inheritdoc />
	public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
	{
		bool found = TryGetIndex(key, out var table, out int index);
		value = found ? table!.Values[index] : default; // table not null when found
		return found;
	}
	
	/// <summary>
	/// Tries to get the index and corresponding table where the specified key is stored.
	/// </summary>
	/// <param name="key">The key to locate in the cuckoo hash table.</param>
	/// <param name="table">
	/// When this method returns, contains the <see cref="ParallelArrays{TKey,TValue}"/> table 
	/// in which the key is potentially stored, if the key is found; otherwise, <c>null</c>.
	/// This parameter is passed uninitialized.
	/// </param>
	/// <param name="index">
	/// When this method returns, contains the index at which the key is potentially stored in the table, 
	/// if the key is found; otherwise, the default value for the type of the index parameter.
	/// This parameter is passed uninitialized.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the cuckoo hash table contains an element with the specified key; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	/// This method is a key part of the cuckoo hashing algorithm, which involves using two hash functions 
	/// and two tables. It attempts to find the specified key by checking both tables with respective hash functions. 
	/// If the key is found, the method returns <see langword="true"/>, and outputs the table and index where the key is stored. 
	/// If the key is not found, the method returns <see langword="false"/>, and the output parameters are set to their default values.
	/// </remarks> 
	public bool TryGetIndex(TKey key, [NotNullWhen(true)] out ParallelArrays<TKey?, TValue?>? table, out int index)
	{
		index = GetHash1(key);
		ValidateIndex(index, keyPresent1);
		if (keyPresent1[index] && Comparer.Equal(table1.Keys[index]!, key))
		{
			table = table1;
			return true;
		}
		
		index = GetHash2(key);
		ValidateIndex(index, keyPresent2);
		if (keyPresent2[index] && Comparer.Equal(table2.Keys[index]!, key))
		{
			table = table2;
			return true;
		}

		table = null;
		return false;
	}
	
	private static void FillTable(ParallelArrays<TKey?, TValue?> table, int size, TKey? initialKey = default, TValue? initialValue = default)
	{
		for (int i = 0; i < size; i++)
		{
			table.Add(initialKey, initialValue);
		}
	}
	
	/*
		This method puts the given key and value in the table, and assign the key and value
		that were already there to the key value variables. Essentially, we kick the existing
		pair from the table so we can find a place for them instead.

		Note: This could be a local function but the name clashes are too messy to deal with
	*/
	private static void KickKeyFromTable(ParallelArrays<TKey?, TValue?> table, int index, ref TKey key, ref TValue value)
	{
		var tmpKey = table.Keys[index]!; // We checked that key is present at the call site. 
		var tmpValue = table.Values[index]!;

		table.Set(index, key, value);

		key = tmpKey;
		value = tmpValue;
	}	

	private int GetHash(TKey key, int prime)
	{
		key.ThrowIfNull();
		int hashCode = key.GetHashCode() * prime;
		
		if (log2TableSize + 5 < 26)
		{
			hashCode %= HashTableWithLinearProbing.Primes[log2TableSize + 5];
		}
		
		return hashCode % halfTableSize;
	}

	private int GetHash1(TKey key) => GetHash(key, Prime1);

	private int GetHash2(TKey key) => GetHash(key, Prime2);

	private void Resize(int newTableSize) // See page 474.
	{
		void Copy(ParallelArrays<TKey?, TValue?> oldTable, bool[] oldKeyPresent, CuckooHashTable<TKey, TValue> newTable)
		{
			for (int i = 0; i < halfTableSize; i++)
			{
				if (!oldKeyPresent[i])
				{
					continue;
				}

				var key = oldTable.Keys[i];
				var value = oldTable.Values[i];
					
				Assert(key != null);
				Assert(value != null);
					
				newTable.Add(key, value);
			}
		}
		
		Assert(keyPresent1.Length == halfTableSize);
		Assert(table1.Keys.Count == halfTableSize);
		Assert(table1.Values.Count == halfTableSize);
		
		Assert(keyPresent2.Length == halfTableSize);
		Assert(table2.Keys.Count == halfTableSize);
		Assert(table2.Values.Count == halfTableSize);
		
		var resizedTable 
			= new CuckooHashTable<TKey, TValue>(newTableSize, Comparer);
		
		// These two need to be before all the assignments happen
		Copy(table1, keyPresent1, resizedTable);
		Copy(table2, keyPresent2, resizedTable);
		
		table1 = resizedTable.table1;
		keyPresent1 = resizedTable.keyPresent1;
		
		table2 = resizedTable.table2;
		keyPresent2 = resizedTable.keyPresent2;
		
		log2TableSize = resizedTable.log2TableSize;
		halfTableSize = resizedTable.halfTableSize;
		
		Assert(keyPresent1.Length == halfTableSize);
		Assert(table1.Keys.Count == halfTableSize);
		Assert(table1.Values.Count == halfTableSize);
		
		Assert(keyPresent2.Length == halfTableSize);
		Assert(table2.Keys.Count == halfTableSize);
		Assert(table2.Values.Count == halfTableSize);
	}

	#region Add and Remove

	private void AddKeyAt(ParallelArrays<TKey?, TValue?> table, bool[] keyPresent, int index, TKey key, TValue value)
	{
		// There should be nothing there, otherwise Count will not be accurate
		Assert(!keyPresent[index]);
		Assert(EqualityComparer<TKey>.Default.Equals(table.Keys[index], default));
		Assert(EqualityComparer<TValue>.Default.Equals(table.Values[index], default));

		keyPresent[index] = true; 
		table.Set(index, key, value);
		Count++;
	}

	private void RemoveKeyAt(ParallelArrays<TKey?, TValue?> table, int index)
	{
		table.Set(index, default!, default!);
		Count--;
	}
	
	[Conditional(Diagnostics.DebugDefine)]
	private void ValidateIndex<T>(int index, IList<T> list)
	{
		Assert(index >= 0 && index < list.Count);
	}

	#endregion
}

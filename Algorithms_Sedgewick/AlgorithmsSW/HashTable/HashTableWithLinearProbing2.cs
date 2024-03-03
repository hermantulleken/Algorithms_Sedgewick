using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AlgorithmsSW.List;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.HashTable;

using SymbolTable;

/// <summary>
/// Represents a hash table that uses linear probing to resolve collisions.
/// </summary>
/// <typeparam name="TKey">The type of keys in the hash table.</typeparam>
/// <typeparam name="TValue">The type of values in the hash table.</typeparam>
[ExerciseReference(3, 4, 28)]
/*
	This class represents a hash table that uses linear probing to resolve collisions.
	Linear probing is an open-addressing strategy where we look for the next available slot
	in the array when a collision occurs.
	
	This version always have a prime table size, so the table hashing mechanism can work as expected. 
*/
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class HashTableWithLinearProbing2<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private bool[] keyPresent; // Necessary if TKey is a value type
	private TKey[] keys;
	private int log2TableSize;
	private int tableSize;
	private TValue[] values;

	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc />
	public IEnumerable<TKey> Keys 
		=> keyPresent
			.IndexWhere(Algorithms.Identity)
			.Select(index => keys[index]);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	/// <summary>
	/// Initializes a new instance of the <see cref="HashTableWithLinearProbing2{TKey,TValue}"/> class.
	/// </summary>
	/// <param name="comparer">The comparer used to compare keys.</param>
	public HashTableWithLinearProbing2(IComparer<TKey> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HashTableWithLinearProbing2{TKey,TValue}"/> class.
	/// </summary>
	/// <param name="initialCapacity">The initial capacity of the hash table.</param>
	/// <param name="comparer">The comparer used to compare keys.</param>
	public HashTableWithLinearProbing2(int initialCapacity, IComparer<TKey> comparer)
	{
		(log2TableSize, tableSize) = HashTableWithLinearProbing.GetTableSize(initialCapacity);
		
		this.comparer = comparer;
		keys = new TKey[tableSize];
		values = new TValue[tableSize];
		keyPresent = new bool[tableSize];
	}

	/// <inheritdoc />
	public void Add(TKey key, TValue value)
	{
		key.ThrowIfNull();
		
		if (Count >= tableSize / 2)
		{
			Resize(Count * 2); // Doubles the size
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

	/// <inheritdoc />
	public bool ContainsKey(TKey key) => TryGetValue(key, out _);

	/// <inheritdoc />
	public void RemoveKey(TKey key)
	{
		key.ThrowIfNull();
		
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
		for (GetNextIndex(key, ref index); keyPresent[index]; GetNextIndex(key, ref index))
		{
			ReinsertAt(index);
		}
		
		if (Count > 0 && Count == tableSize / 8)
		{
			Resize(Count / 2);
		}
	}

	/// <inheritdoc />
	public bool TryGetValue(TKey key, out TValue value)
	{
		key.ThrowIfNull();
		
		int index = IndexOf(key);
		bool found = index >= 0;
		value = found ? values[index] : default!;

		return found;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetHash([DisallowNull] TKey key)
	{
		int hashCode = key.GetHashCode();

		return MathX.Mod(hashCode, tableSize);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void GetNextIndex([DisallowNull] TKey key, ref int index)
	{
		int offset = GetOffset(key);
		int result = MathX.Mod(index + offset, tableSize);
		
		Assert(result != index);
		
		index = result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetOffset([DisallowNull] TKey key) => key.GetHashCode() % (tableSize - 1) + 1;

	/*
		Iterate through the table, starting from the hash value of the key and moving linearly.
		The loop continues until an empty slot is found (meaning the key is not present in the table).
		
		Returns a negative value if the index is not found. The complement of this value (i.e. ~IndexOf(key)) is where a new 
		element can be inserted.
	*/
	private int IndexOf([DisallowNull] TKey key)
	{
		int i;
		for (i = GetHash(key); keyPresent[i]; GetNextIndex(key, ref i))
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

	private void Resize(int newCapacity) // See page 474.
	{
		var newTable = new HashTableWithLinearProbing2<TKey, TValue>(newCapacity, comparer);
		
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
		log2TableSize = newTable.log2TableSize;
		tableSize = newTable.tableSize;
	}

	private void SetAt(int index, TKey key, TValue value, bool present)
	{
		keys[index] = key;
		values[index] = value;
		keyPresent[index] = present;
	}
}

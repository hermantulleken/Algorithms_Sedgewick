using System.Collections;
using Algorithms_Sedgewick.HashTable;

namespace Algorithms_Sedgewick.Set;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using List;
using static System.Diagnostics.Debug;

// Ex. 3.4.28
/*
	This class represents a hash table that uses linear probing to resolve collisions.
	Linear probing is an open-addressing strategy where we look for the next available slot
	in the array when a collision occurs.
	
	This version always have a prime table size, so the table hashing mechanism can work as expected. 
*/
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class HashSet<T> : ISet<T>
{
	private readonly IComparer<T> comparer;
	private bool[] keyPresent; // Necessary if TKey is a value type
	private T[] keys;
	private int log2TableSize;
	private int tableSize;
	
	public int Count { get; private set; }

	public HashSet(IComparer<T> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	public HashSet(int initialCapacity, IComparer<T> comparer)
	{
		log2TableSize = Math2.IntegerCeilLog2(initialCapacity);
		tableSize = HashTableWithLinearProbing.Primes[log2TableSize];
		this.comparer = comparer;
		keys = new T[tableSize];
		keyPresent = new bool[tableSize];
	}

	public void Add(T key)
	{
		key.ThrowIfNull();
		
		if (Count >= tableSize / 2)
		{
			Resize(log2TableSize + 1); // Doubles the size
		}

		int index = IndexOf(key);

		if (index < 0)
		{
			Count++;
		}
		
		SetAt(~index, key, true);
	}

	public bool Contains(T key)
	{
		key.ThrowIfNull();
		
		int index = IndexOf(key);
		bool found = index >= 0;

		return found;
	}

	public void Remove(T key)
	{
		key.ThrowIfNull();
		
		void ReinsertAt(int index)
		{
			var keyToRedo = keys[index];
			RemoveKeyAt(index);
			Add(keyToRedo);
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
			Resize(log2TableSize - 1);
		}
	}

	public IEnumerator<T> GetEnumerator() => keyPresent
		.IndexWhere(Algorithms.Identity)
		.Select(index => keys[index]).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetHash([DisallowNull] T key)
	{
		int hashCode = key.GetHashCode();

		return hashCode % tableSize;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void GetNextIndex([DisallowNull] T key, ref int index)
	{
		int offset = GetOffset(key);
		int result = (index + offset) % tableSize;
		
		Assert(result != index);
		
		index = result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetOffset([DisallowNull] T key) => key.GetHashCode() % (tableSize - 1) + 1;

	/*
		Iterate through the table, starting from the hash value of the key and moving linearly.
		The loop continues until an empty slot is found (meaning the key is not present in the table).
		
		Returns a negative value if the index is not found. The complement of this value (i.e. ~IndexOf(key)) is where a new 
		element can be inserted.
	*/
	private int IndexOf([DisallowNull] T key)
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
		SetAt(index, default!, false);
		Count--;
	}

	private void Resize(int newLog2TableSize) // See page 474.
	{
		var newTable = new HashSet<T>(newLog2TableSize, comparer);
		
		for (int i = 0; i < tableSize; i++)
		{
			if (keyPresent[i])
			{
				newTable.Add(keys[i]);
			}
		}
		
		keys = newTable.keys;
		keyPresent = newTable.keyPresent;
		log2TableSize = newLog2TableSize;
		tableSize = newTable.tableSize;
	}

	private void SetAt(int index, T key, bool present)
	{
		keys[index] = key;
		keyPresent[index] = present;
	}
}

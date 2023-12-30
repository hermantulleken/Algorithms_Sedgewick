using System.Collections;
using AlgorithmsSW.HashTable;

namespace AlgorithmsSW.Set;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using List;
using Support;
using static System.Diagnostics.Debug;

[ExerciseReference(3, 4, 28)]
/*
	This class represents a hash table that uses linear probing to resolve collisions.
	Linear probing is an open-addressing strategy where we look for the next available slot
	in the array when a collision occurs.
	
	This version always have a prime table size, so the table hashing mechanism can work as expected. 
*/
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic and non-generic versions.")]
public class HashSet<T> : ISet<T>
{
	private bool[] keyPresent; // Necessary if TKey is a value type
	private T[] keys;
	private int log2TableSize;
	private int tableSize;
	
	public int Count { get; private set; }

	public IComparer<T> Comparer { get; }

	public HashSet(IComparer<T> comparer)
		: this(ResizeableArray.DefaultCapacity, comparer)
	{
	}

	public HashSet(int initialCapacity, IComparer<T> comparer)
	{
		(log2TableSize, tableSize) = HashTableWithLinearProbing.GetTableSize(initialCapacity);
		
		Comparer = comparer;
		keys = new T[tableSize];
		keyPresent = new bool[tableSize];
	}

	public void Add(T key)
	{
		key.ThrowIfNull();
		
		if (Count >= tableSize / 2)
		{
			Resize(Count * 2); // Doubles the size
		}

		int index = IndexOf(key);

		if (index < 0)
		{
			Count++;
			SetAt(~index, key, true);
		}
		
		// else already there
	}

	public bool Contains(T key)
	{
		key.ThrowIfNull();
		
		int index = IndexOf(key);
		bool found = index >= 0;

		return found;
	}

	public bool Remove(T key)
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
			return false;
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
		
		/*if (Count > 0 && Count <= tableSize / 8)
		{
			Resize(log2TableSize - 1);
		}*/

		return true;
	}

	public IEnumerator<T> GetEnumerator() => keyPresent
		.IndexWhere(Algorithms.Identity)
		.Select(index => keys[index]).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetHash([DisallowNull] T key)
	{
		int hashCode = key.GetHashCode();

		return MathX.Mod(hashCode, tableSize);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void GetNextIndex([DisallowNull] T key, ref int index)
	{
		int offset = GetOffset(key);
		int result = MathX.Mod(index + offset, tableSize);
		
		Assert(result != index);
		
		index = result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetOffset([DisallowNull] T key) => MathX.Mod(key.GetHashCode(), tableSize - 1) + 1;

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
			if (Comparer.Equal(keys[i], key))
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

	private void Resize(int newCapacity) // See page 474.
	{
		var newTable = new HashSet<T>(newCapacity, Comparer);
		
		for (int i = 0; i < tableSize; i++)
		{
			if (keyPresent[i])
			{
				newTable.Add(keys[i]);
			}
		}
		
		keys = newTable.keys;
		keyPresent = newTable.keyPresent;
		log2TableSize = newTable.log2TableSize;
		tableSize = newTable.tableSize;
	}

	private void SetAt(int index, T key, bool present)
	{
		keys[index] = key;
		keyPresent[index] = present;
	}
}

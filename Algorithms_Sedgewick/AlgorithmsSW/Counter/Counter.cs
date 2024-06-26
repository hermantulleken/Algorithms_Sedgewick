﻿namespace AlgorithmsSW.Counter;

using System.Collections;
using System.Runtime.CompilerServices;
using HashTable;
using SymbolTable;

/// <inheritdoc />
[CollectionBuilder(typeof(CounterBuilder), nameof(CounterBuilder.Create))]
public class Counter<T>(IComparer<T> comparer) : ICounter<T>
{
	private readonly ISymbolTable<T, int> counts = new HashTableWithLinearProbing2<T, int>(comparer);

	/// <inheritdoc/>
	public IEnumerable<T> Keys => counts.Keys;

	/// <inheritdoc/>
	public int this[T item] => counts.ContainsKey(item) ? counts[item] : 0;

	/// <inheritdoc/>
	public void Add(T item)
	{
		if (!counts.ContainsKey(item))
		{
			counts[item] = 1;
		}
		else
		{
			counts[item]++;
		}
	}

	/// <inheritdoc/>
	public void Remove(T item)
	{
		if (!counts.ContainsKey(item))
		{
			counts[item] = -1;
		}
		else
		{
			counts[item]--;
			
			if (counts[item] == 0)
			{
				counts.RemoveKey(item);
			}
		}
	}

	public int GetCount(T item) => counts.ContainsKey(item) ? counts[item] : 0;

	/// <inheritdoc/>
	public void Clear() => counts.Clear();

	public void RemoveAll(T item)
	{
		if (counts.ContainsKey(item))
		{
			counts.RemoveKey(item);
		} // else nothing left to do
	}

	public IEnumerator<KeyValuePair<T, int>> GetEnumerator() 
		=> Keys.Select(key => new KeyValuePair<T, int>(key, counts[key])).GetEnumerator();
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

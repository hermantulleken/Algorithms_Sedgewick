﻿namespace Algorithms_Sedgewick.SymbolTable;

using static System.Diagnostics.Debug;

public sealed class OrderedSymbolTableWithOrderedLinkedList<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private readonly List.LinkedList<KeyValuePair<TKey, TValue>> list;
	private readonly IComparer<KeyValuePair<TKey, TValue>> pairComparer;

	public int Count => list.Count;

	public TValue this[TKey key]
	{
		get => AsSymbolTable[key];
		set => AsSymbolTable[key] = value;
	}

	public IEnumerable<TKey> Keys => list.Select(pair => pair.Key);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	public OrderedSymbolTableWithOrderedLinkedList(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		list = new List.LinkedList<KeyValuePair<TKey, TValue>>();
		pairComparer = comparer.Convert<TKey, KeyValuePair<TKey, TValue>>(OrderedSymbolTableWithOrderedArray<TKey, TValue>.PairToKey);
	}

	public void Add(TKey key, TValue value)
	{
		if (list.IsEmpty || comparer.Less(key, list.First.Item.Key))
		{
			list.InsertAtFront(new KeyValuePair<TKey, TValue>(key, value));
		}
		else
		{
			var insertionNode = list.FindInsertionNode(new KeyValuePair<TKey, TValue>(key, default), pairComparer);
				
			Assert(comparer.LessOrEqual(insertionNode.Item.Key, key));
				
			if (insertionNode.NextNode != null)
			{
				Assert(comparer.Less(key, insertionNode.NextNode.Item.Key));
			}

			var newItem = new KeyValuePair<TKey, TValue>(key, value);
				
			if (comparer.Equal(key, insertionNode.Item.Key))
			{
				insertionNode.Item = newItem;
			}
			else
			{
				list.InsertAfter(insertionNode, newItem);
			}
		}
	}

	public bool ContainsKey(TKey key) => TryFirst(key, out _);

	public int CountRange(TKey start, TKey end)
		=> KeysRange(start, end).Count();

	public IEnumerable<TKey> KeysRange(TKey start, TKey end)
	{
		if (list.IsEmpty || comparer.Equal(start, end))
		{
			yield break;
		}

		var startNode = list.FindInsertionNode(KeyToPair(start), pairComparer);
		var node = comparer.Equal(startNode.Item.Key, start) ? startNode : startNode.NextNode;

		while (node != null && comparer.Less(node.Item.Key, end))
		{
			yield return node.Item.Key;
			node = node.NextNode;
		}
	}

	// TODO Check for special cases.
	public TKey KeyWithRank(int rank)
		=> list.ElementAt(rank).Key;

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		if (list.IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		if (comparer.Less(key, list.First.Item.Key))
		{
			ThrowHelper.ThrowException("All keys are larger than the given key.");
		}
		
		var insertionNode = list.FindInsertionNode(new KeyValuePair<TKey, TValue>(key, default), pairComparer);
				
		Assert(comparer.LessOrEqual(insertionNode.Item.Key, key));
				
		if (insertionNode.NextNode != null)
		{
			Assert(comparer.Less(key, insertionNode.NextNode.Item.Key));
		}

		return insertionNode.Item.Key;
	}

	public TKey MaxKey() => list.Last.Item.Key;

	public TKey MinKey() => list.First.Item.Key;

	public int RankOf(TKey key)
	{
		if (comparer.Less(list.Last.Item.Key, key))
		{
			return Count;
		}

		int i = 0;
		foreach (var pair in list)
		{
			if (comparer.LessOrEqual(key, pair.Key))
			{
				return i;
			}

			i++;
		}
		
		return Count;
	}

	public void RemoveKey(TKey key)
	{
		if (comparer.Equal(key, list.First.Item.Key))
		{
			list.RemoveFromFront();
			return;
		}
		
		foreach (var node in list.Nodes)
		{
			var nextNode = node.NextNode;

			if (nextNode != null)
			{
				switch (comparer.Compare(key, nextNode.Item.Key))
				{
					case 0:
						list.RemoveAfter(node);
						return; // We need not iterate further. If we did we would trigger the version validation in the next iteration.
					case < 0:
						// All the reaming items are bigger.
						throw ThrowHelper.KeyNotFoundException(key);
				}
			} // else we reached the end
		}
		
		throw ThrowHelper.KeyNotFoundException(key);
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		var insertionNode = list.FindInsertionNode(KeyToPair(key), pairComparer);

		while (comparer.Less(insertionNode.Item.Key, key))
		{
			if (insertionNode.NextNode != null)
			{
				insertionNode = insertionNode.NextNode;
			}
			else
			{
				ThrowHelper.ThrowException("All keys are smaller than the given key.");
			}
		}

		return insertionNode.Item.Key;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool result = TryFirst(key, out var pair);
		value = result ? pair.Value : default!;

		return result;
	}

	private static KeyValuePair<TKey, TValue> KeyToPair(TKey key) => new(key, default);

	private bool TryFirst(TKey key, out KeyValuePair<TKey, TValue> pair)
	{
		foreach (var listPair in list)
		{
			if (comparer.Equal(key, listPair.Key))
			{
				pair = listPair;
				return true;
			}
		}

		pair = default;
		return false;
	}

	private bool TryFirstNode(TKey key, out List.LinkedList<KeyValuePair<TKey, TValue>>.Node node)
	{
		foreach (var listNode in list.Nodes)
		{
			if (comparer.Equal(key, listNode.Item.Key))
			{
				node = listNode;
				return true;
			}
		}

		node = default;
		return false;
	}
}

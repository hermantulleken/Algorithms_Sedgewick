namespace Algorithms_Sedgewick.SymbolTable;

using static System.Diagnostics.Debug;

public sealed class OrderedSymbolTableWithOrderedLinkedList<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private readonly List.LinkedList<KeyValuePair<TKey, TValue>> list;
	private readonly IComparer<KeyValuePair<TKey, TValue>> pairComparer;

	public int Count => list.Count;

	public IEnumerable<TKey> Keys => list.Select(pair => pair.Key);

	public OrderedSymbolTableWithOrderedLinkedList(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		list = new List.LinkedList<KeyValuePair<TKey, TValue>>();
		pairComparer = new PairComparer<TKey, TValue>(comparer);
	}

	public void Add(TKey key, TValue value)
	{
		var newItem = new KeyValuePair<TKey, TValue>(key, value);
		
		if (list.IsEmpty || comparer.Less(key, list.First.Item.Key))
		{
			list.InsertAtFront(newItem);
		}
		else
		{
			// Question: Do we really need to create this class here?
			
			var insertionNode = list.FindInsertionNodeUnsafe(newItem, pairComparer);
				
			Assert(comparer.LessOrEqual(insertionNode.Item.Key, key));
				
			if (insertionNode.NextNode != null)
			{
				Assert(comparer.Less(key, insertionNode.NextNode.Item.Key));
			}
			
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

	public bool ContainsKey(TKey key) => TryGetValue(key, out _);

	public int CountRange(TKey start, TKey end)
		=> KeysRange(start, end).Count();

	public IEnumerable<TKey> KeysRange(TKey start, TKey end)
	{
		if (list.IsEmpty || comparer.Equal(start, end))
		{
			yield break;
		}

		var startNode = list.FindInsertionNodeUnsafe(KeyToPair(start), pairComparer);
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
		
		var insertionNode = list.FindInsertionNodeUnsafe(KeyToPair(key), pairComparer);
				
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
		var insertionNode = list.FindInsertionNodeUnsafe(KeyToPair(key), pairComparer);

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
		KeyValuePair<TKey, TValue> pair = default;
		var node = list.First;
		bool found = false;

		while (node != null)
		{
			if (comparer.Equal(key, node.Item.Key))
			{
				pair = node.Item;
				found = true;
				break;
			}

			node = node.NextNode;
		}
		
		value = pair.Value; // Will bde default(TValue) if pair is still default;

		return found;
	}

	private static KeyValuePair<TKey, TValue> KeyToPair(TKey key) => new(key, default!);
}

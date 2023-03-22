namespace Algorithms_Sedgewick.SymbolTable;

using System.Diagnostics;

public class OrderedSymbolTableWithOrderedLinkedList<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;
	private readonly IComparer<KeyValuePair<TKey, TValue>> pairComparer;
	private readonly List.LinkedList<KeyValuePair<TKey, TValue>> list;

	public OrderedSymbolTableWithOrderedLinkedList(IComparer<TKey> comparer)
	{
		this.comparer = comparer;
		list = new List.LinkedList<KeyValuePair<TKey, TValue>>();
		pairComparer = comparer.Convert<TKey, KeyValuePair<TKey, TValue>>(OrderedSymbolTableWithOrderedArray<TKey, TValue>.PairToKey);
	}

	public int Count => list.Count;

	public IEnumerable<TKey> Keys => list.Select(pair => pair.Key);

	public TValue this[TKey key]
	{
		get
		{
			if (TryFirst(key, out var pair))
			{
				return pair.Value;
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set
		{
			if (list.IsEmpty || comparer.Less(key, list.First.Item.Key))
			{
				list.InsertAtFront(new KeyValuePair<TKey, TValue>(key, value));
			}
			else
			{
				var insertionNode = list.FindInsertionNode(new KeyValuePair<TKey, TValue>(key, default), pairComparer);
				
				Debug.Assert(comparer.LessOrEqual(insertionNode.Item.Key, key));
				
				if (insertionNode.NextNode != null)
				{
					Debug.Assert(comparer.Less(key, insertionNode.NextNode.Item.Key));
				}

				var newItem = new KeyValuePair<TKey, TValue>(key, value);
				
				if(comparer.Equal(key, insertionNode.Item.Key))
				{
					insertionNode.Item = newItem;
				}
				else
				{
					list.InsertAfter(insertionNode, newItem);
				}
			}
		}
	}

	public void RemoveKey(TKey key)
	{
		throw new NotImplementedException();
	}

	public bool ContainsKey(TKey key) => TryFirst(key, out _);

	public TKey MinKey() => list.First.Item.Key;

	public TKey MaxKey() => list.Last.Item.Key;

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
				
		Debug.Assert(comparer.LessOrEqual(insertionNode.Item.Key, key));
				
		if (insertionNode.NextNode != null)
		{
			Debug.Assert(comparer.Less(key, insertionNode.NextNode.Item.Key));
		}

		return insertionNode.Item.Key;
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		var insertionNode = list.FindInsertionNode(new KeyValuePair<TKey, TValue>(key, default), pairComparer);

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

		Debug.Assert(false, "Already handled before the loop.");
		return Count;
		
	}

	//TODO Check for special cases.
	public TKey KeyWithRank(int rank)
		=> list.ElementAt(rank).Key;

	public int CountRange(TKey start, TKey end)
	{
		if (TryFirstNode(start, out var node))
		{
			int count = 1;

			//TODO - handle start == end
			foreach (var restNode in node.Rest)
			{
				count++;
				if (comparer.Equal(end, restNode.Item.Key))
				{
					return count;
				}
			}
		}
		
		//TODO
		ThrowHelper.ThrowException("");
		return default;
	}

	public IEnumerable<TKey> KeysRange(TKey start, TKey end) => throw new NotImplementedException();

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

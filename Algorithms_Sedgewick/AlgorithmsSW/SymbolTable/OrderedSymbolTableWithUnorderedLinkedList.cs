﻿namespace AlgorithmsSW.SymbolTable;

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using PriorityQueue;

public class OrderedSymbolTableWithUnorderedLinkedList<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly List.LinkedList<KeyValuePair<TKey, TValue>> list;

	public IComparer<TKey> Comparer { get; }
	
	public int Count => list.Count;

	public IEnumerable<TKey> Keys => list.Select(pair => pair.Key);

	private IEnumerable<
			(List.LinkedList<KeyValuePair<TKey, TValue>>.Node? previousNode, 
			List.LinkedList<KeyValuePair<TKey, TValue>>.Node node)> NodeAndPrevious
	{
		get
		{
			yield return (null, list.First);

			foreach (var node in list.Nodes)
			{
				if (node.NextNode != null)
				{
					yield return (node, node.NextNode);
				}
			}
		}
	}

	public OrderedSymbolTableWithUnorderedLinkedList(IComparer<TKey> comparer)
	{
		this.Comparer = comparer;
		list = new();
	}

	public void Add(TKey key, TValue value)
	{
		if (TryFindNodeWithKey(key, out var node))
		{
			node.Item = new(key, value);
		}
		else
		{
			list.InsertAtBack(new(key, value));
		}	
	}

	public bool ContainsKey(TKey key) => TryFindNodeWithKey(key, out _);
	
	public int CountRange(TKey start, TKey end) 
		=> Keys.Count(key => LessOrEqual(start, key) && Less(key, end));

	public IEnumerable<TKey> KeysRange(TKey start, TKey end) 
		=> Keys.Where(key => LessOrEqual(start, key) && Less(key, end));

	public TKey KeyWithRank(int rank)
	{
		if (rank >= Count)
		{
			ThrowHelper.ThrowNotEnoughElements(rank + 1);
		}
		
		if (rank == 0)
		{
			return MinKey();
		}

		if (rank == Count - 1)
		{
			return MaxKey();
		}

		// This can be made into a field, initialized lazily, and resized as needed.
		// Also, this is a wrapper class that is slower then the minimum version
		// We could use two, depending on whether the rank is smaller than Count / 2
		var queue = new FixedCapacityMaxBinaryHeap<TKey>(rank + 1, Comparer);

		void PushToQueue(TKey key1)
		{
			queue.Push(key1);
		}

		foreach (var key in Keys)
		{
			if (queue.Count < rank + 1)
			{
				PushToQueue(key);
			}
			else if (Less(key, queue.PeekMax))
			{
				queue.PopMax();
				PushToQueue(key);
			}
		}
		
		Debug.Assert(queue.Count == rank + 1);

		return queue.PeekMax;
	}

	// This can throw an exception if the given key is larger than all the keys
	public TKey LargestKeyLessThanOrEqualTo(TKey key) 
		=> Keys
			.Where(leftKey => LessOrEqual(leftKey, key))
			.Max(Comparer);

	public TKey MaxKey() => MaxNodeAndPrevious().node.Item.Key;

	public TKey MinKey() => MinNodeAndPrevious().node.Item.Key;

	public int RankOf(TKey key) 
		=> Keys
			.Count(listKey => Less(listKey, key));

	public void RemoveKey(TKey key)
	{
		if (Equals(list.First.Item.Key, key))
		{
			list.RemoveFromFront();
		}
		else
		{
			if (TryFindPredecessor(key, out var node))
			{
				list.RemoveAfter(node);
			}
			else
			{
				ThrowHelper.ThrowKeyNotFound(key);
			}
		}
	}

	public KeyValuePair<TKey, TValue> RemoveMaxKey()
	{
		var (previous, node) = MaxNodeAndPrevious();
		if (previous == null)
		{
			list.RemoveFromFront();
		}
		else
		{
			list.RemoveAfter(previous);
		}

		return node.Item;
	}

	public KeyValuePair<TKey, TValue> RemoveMinKey()
	{
		var (previous, node) = MinNodeAndPrevious();
		if (previous == null)
		{
			list.RemoveFromFront();
		}
		else
		{
			list.RemoveAfter(previous);
		}

		return node.Item;
	}

	// This can throw an exception if the given key is smaller than all the keys
	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		var tmp = Keys
			.Where(leftKey => LessOrEqual(key, leftKey))
			.ToArray();

		return tmp.Min(Comparer);
	}

	public override string ToString() => list.ToString();

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFindNodeWithKey(key, out var node);
		value = found ? node.Item.Value : default!;

		return found;
	}

	private bool Equals(TKey left, TKey right) 
		=> Comparer.Compare(left, right) == 0;

	private bool Less(TKey left, TKey right)
		=> Comparer.Compare(left, right) < 0;

	private bool LessOrEqual(TKey left, TKey right)
		=> Comparer.Compare(left, right) <= 0;

	private (List.LinkedList<KeyValuePair<TKey, TValue>>.Node? previousNode, List.LinkedList<KeyValuePair<TKey, TValue>>.Node node) 
		MaxNodeAndPrevious() 
		=> NodeAndPrevious.MaxBy(pair => pair.node.Item.Key, Comparer);

	private (List.LinkedList<KeyValuePair<TKey, TValue>>.Node? previousNode, List.LinkedList<KeyValuePair<TKey, TValue>>.Node node) 
		MinNodeAndPrevious() 
		=> NodeAndPrevious.MinBy(pair => pair.node.Item.Key, Comparer);
	
	private bool TryFindNodeWithKey(TKey key, [NotNullWhen(true)] out List.LinkedList<KeyValuePair<TKey, TValue>>.Node? node)
	{
		if (list.IsEmpty)
		{
			node = null;
			return false;
		}
		
		var current = list.First;
			
		while (current != null)
		{
			if (Equals(current.Item.Key, key))
			{
				node = current;
				return true;
			}
			
			current = current.NextNode;
		}

		node = null;
		return false;
	}

	private bool TryFindPredecessor(TKey key, [MaybeNullWhen(false)] out List.LinkedList<KeyValuePair<TKey, TValue>>.Node node)
	{
		foreach (var listNode in list.Nodes)
		{
			if (listNode.NextNode != null && Equals(key, listNode.NextNode.Item.Key))
			{
				node = listNode;
				return true;
			}
		}

		node = null;
		return false;
	}
}

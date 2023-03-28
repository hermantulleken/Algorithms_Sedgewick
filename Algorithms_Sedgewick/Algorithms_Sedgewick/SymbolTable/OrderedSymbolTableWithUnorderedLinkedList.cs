namespace Algorithms_Sedgewick.SymbolTable;

using System.Collections.Generic;
using System.Diagnostics;
using PriorityQueue;

public class OrderedSymbolTableWithUnorderedLinkedList<TKey, TValue>: IOrderedSymbolTable<TKey, TValue>
{
	private readonly IComparer<TKey> comparer;

	private readonly List.LinkedList<KeyValuePair<TKey, TValue>> list;

	public int Count => list.Count;

	public TValue this[TKey key]
	{
		get => AsSymbolTable[key];
		set => AsSymbolTable[key] = value;
	}

	public IEnumerable<TKey> Keys => list.Select(pair => pair.Key);

	private ISymbolTable<TKey, TValue> AsSymbolTable => this;

	private IEnumerable<
			(List.LinkedList<KeyValuePair<TKey, TValue>>.Node previousNode, 
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
		this.comparer = comparer;
		list = new List.LinkedList<KeyValuePair<TKey, TValue>>();
	}

	public void Add(TKey key, TValue value)
	{
		if (TryFindNodeWithKey(key, out var node))
		{
			node.Item = new KeyValuePair<TKey, TValue>(key, value);
		}
		else
		{
			list.InsertAtBack(new KeyValuePair<TKey, TValue>(key, value));
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
		var queue = new FixedCapacityMaxBinaryHeap<Comparable<TKey>>(rank + 1);

		void PushToQueue(TKey key1)
		{
			queue.Push(key1.ToComparable(comparer));
		}

		foreach (var key in Keys)
		{
			if (queue.Count < rank + 1)
			{
				PushToQueue(key);
			}
			else if (Less(key, queue.PeekMax.Item))
			{
				queue.PopMax();
				PushToQueue(key);
			}
		}
		
		Debug.Assert(queue.Count == rank + 1);

		return queue.PeekMax.Item;
	}

	// This can throw an exception if the given key is larger than all the keys
	public TKey LargestKeyLessThanOrEqualTo(TKey key) 
		=> Keys
			.Where(leftKey => LessOrEqual(leftKey, key))
			.Max(comparer);

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

		return tmp.Min(comparer);
	}

	public override string ToString() => list.ToString();

	public bool TryGetValue(TKey key, out TValue value)
	{
		bool found = TryFindNodeWithKey(key, out var node);
		value = found ? node.Item.Value : default!;

		return found;
	}

	private bool Equals(TKey left, TKey right) 
		=> comparer.Compare(left, right) == 0;

	private bool Less(TKey left, TKey right)
		=> comparer.Compare(left, right) < 0;

	private bool LessOrEqual(TKey left, TKey right)
		=> comparer.Compare(left, right) <= 0;

	private (List.LinkedList<KeyValuePair<TKey, TValue>>.Node previousNode, List.LinkedList<KeyValuePair<TKey, TValue>>.Node node) 
		MaxNodeAndPrevious() 
		=> NodeAndPrevious.MaxBy(pair => pair.node.Item.Key, comparer);

	private (List.LinkedList<KeyValuePair<TKey, TValue>>.Node previousNode, List.LinkedList<KeyValuePair<TKey, TValue>>.Node node) 
		MinNodeAndPrevious() 
		=> NodeAndPrevious.MinBy(pair => pair.node.Item.Key, comparer);

	private bool TryFindNodeWithKey(TKey key, out List.LinkedList<KeyValuePair<TKey, TValue>>.Node node)
	{
		foreach (var listNode in list.Nodes)
		{
			if (Equals(listNode.Item.Key, key))
			{
				node = listNode;
				return true;
			}
		}

		node = null;
		return false;
	}

	private bool TryFindPredecessor(TKey key, out List.LinkedList<KeyValuePair<TKey, TValue>>.Node node)
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

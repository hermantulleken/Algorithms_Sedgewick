﻿namespace Algorithms_Sedgewick.SymbolTable;

using SearchTrees;

public sealed class SymbolTableWithBinarySearchTree<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly BinarySearchTree<KeyValuePair<TKey, TValue>> tree;

	public int Count => tree.Count;

	public TValue this[TKey key]
	{
		get
		{
			bool found = tree.TryFindNode(KeyToPair(key), out var node);
			if (found)
			{
				return node.Item.Value;
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}
		
		set => tree.Add(new KeyValuePair<TKey, TValue>(key, value));
	}

	public IEnumerable<TKey> Keys 
		=> tree.NodesInOrder.Select(NodeToKey);

	public SymbolTableWithBinarySearchTree(IComparer<TKey> comparer)
	{
		var pairComparer = comparer.Convert<TKey, KeyValuePair<TKey, TValue>>(PairToKey);
		
		tree = new BinarySearchTree<KeyValuePair<TKey, TValue>>(pairComparer);
	}

	public bool ContainsKey(TKey key) => tree.TryFindNode(KeyToPair(key), out _);

	public int CountRange(TKey start, TKey end) => KeysRange(start, end).Count();

	public IEnumerable<TKey> KeysRange(TKey start, TKey end)
		=> tree
			.Range(KeyToPair(start), KeyToPair(end))
			.Select(NodeToKey);

	public TKey KeyWithRank(int rank) => tree.NodesInOrder.ElementAt(rank).Item.Key;

	public TKey LargestKeyLessThanOrEqualTo(TKey key)
	{
		var largestNode = tree.LargestKeyLessThanOrEqualTo(KeyToPair(key));
		
		return largestNode != null 
			? largestNode.Item.Key 
			: throw new Exception("No keys less than given key.");
	}

	public TKey MaxKey() => NodeToKey(tree.GetMaxNode());

	public TKey MinKey() => NodeToKey(tree.GetMinNode());

	public int RankOf(TKey key) => tree.CountNodesSmallerThan(KeyToPair(key));

	public void RemoveKey(TKey key)
	{
		throw new NotImplementedException();
	}

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key)
	{
		var smallestNode = tree.SmallestKeyGreaterThanOrEqualTo(KeyToPair(key));
		
		return smallestNode != null 
			? smallestNode.Item.Key 
			: throw new Exception("No keys greater than given key.");
	}

	private static KeyValuePair<TKey, TValue> KeyToPair(TKey key) => new(key, default);

	private static TKey NodeToKey(BinarySearchTree<KeyValuePair<TKey, TValue>>.Node node) => node.Item.Key;

	private static TKey PairToKey(KeyValuePair<TKey, TValue> pair) => pair.Key;
}

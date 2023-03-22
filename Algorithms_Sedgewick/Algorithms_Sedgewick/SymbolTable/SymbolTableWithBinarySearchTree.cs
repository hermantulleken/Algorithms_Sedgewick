namespace Algorithms_Sedgewick.SymbolTable;

using SearchTrees;

public class SymbolTableWithBinarySearchTree<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
{
	private readonly BinarySearchTree<KeyValuePair<TKey, TValue>> tree;

	public int Count => tree.Count;

	public IEnumerable<TKey> Keys 
		=> tree.NodesInOrder.Select(NodeToKey);

	public TValue this[TKey key]
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public SymbolTableWithBinarySearchTree(IComparer<TKey> comparer)
	{
		var pairComparer = comparer.Convert<TKey, KeyValuePair<TKey, TValue>>(PairToKey);
		
		tree = new BinarySearchTree<KeyValuePair<TKey, TValue>>(pairComparer);
	}
	
	public void RemoveKey(TKey key)
	{
		throw new NotImplementedException();
	}

	public bool ContainsKey(TKey key) => tree.TryFindNode(KeyToPair(key), out _);

	public TKey MinKey() => NodeToKey(tree.GetMinNode());

	public TKey MaxKey() => NodeToKey(tree.GetMaxNode());

	public TKey LargestKeyLessThanOrEqualTo(TKey key) => throw new NotImplementedException();

	public TKey SmallestKeyGreaterThanOrEqualTo(TKey key) => throw new NotImplementedException();

	public int RankOf(TKey key) => tree.CountNodesSmallerThan(KeyToPair(key));

	public TKey KeyWithRank(int rank) => throw new NotImplementedException();

	public int CountRange(TKey start, TKey end) => throw new NotImplementedException();

	public IEnumerable<TKey> KeysRange(TKey start, TKey end) => throw new NotImplementedException();

	private static KeyValuePair<TKey, TValue> KeyToPair(TKey key) => new(key, default);

	private static TKey PairToKey(KeyValuePair<TKey, TValue> pair) => pair.Key;

	private static TKey NodeToKey(BinarySearchTree<KeyValuePair<TKey, TValue>>.Node node) => node.Item.Key;
}

using AlgorithmsSW.Digraph;
using AlgorithmsSW.EdgeWeightedGraph;
using AlgorithmsSW.Graph;
using AlgorithmsSW.HashTable;
using AlgorithmsSW.List;
using AlgorithmsSW.PriorityQueue;
using AlgorithmsSW.Queue;
using AlgorithmsSW.Stack;
using AlgorithmsSW.SymbolTable;

namespace AlgorithmsSW;

using EdgeWeightedDigraph;

/// <summary>
/// Sensible default implementations of data structures.
/// </summary>
/// <remarks>
/// This is useful when you need a container with a certain interface but you do not want to make decisions about what
/// implementation to use. 
/// </remarks>
public static class DataStructures
{
	public static IStack<T> Stack<T>() => new StackWithResizeableArray<T>();
	
	public static IStack<T> Stack<T>(int capacity) => new StackWithResizeableArray<T>(capacity);
	
	public static IQueue<T> Queue<T>() => new QueueWithResizeableArray<T>();
	
	public static IQueue<T> Queue<T>(int capacity) => new QueueWithResizeableArray<T>(capacity);
	
	public static ResizeableArray<T> List<T>() => new ResizeableArray<T>();
	
	public static IRandomAccessList<T> List<T>(int capacity) => new ResizeableArray<T>(capacity);
	
	public static Set.ISet<T> Set<T>(IComparer<T> comparer) => new Set.HashSet<T>(comparer);
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(comparer);
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(int capacity, IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(capacity, comparer);
	
	public static IGraph Graph(int vertexCount) => new GraphWithAdjacentsLists(vertexCount);
	
	public static IDigraph Digraph(int vertexCount) => new DigraphWithAdjacentsLists(vertexCount);
	
	// This is actually not a good default, we need a dynamic container. 
	public static IPriorityQueue<T> PriorityQueue<T>(int capacity, IComparer<T> comparer)
		=> new FixedCapacityMinBinaryHeap<T>(capacity, comparer);
	
	public static IndexPriorityQueue<T> IndexedPriorityQueue<T>(int capacity, IComparer<T> comparer) => new(capacity, comparer);

	public static IEdgeWeightedGraph<T> EdgeWeightedGraph<T>(int vertexCount, IComparer<T> comparer) 
		=> new EdgeWeightedGraphWithAdjacencyLists<T>(vertexCount, comparer);
	
	
	public static IEdgeWeightedDigraph<T> EdgeWeightedDigraph<T>(int vertexCount, IComparer<T> comparer) 
		=> new EdgeWeightedDigraphWithAdjacencyLists<T>(vertexCount, comparer);
}

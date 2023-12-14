using Algorithms_Sedgewick.Digraphs;
using Algorithms_Sedgewick.Graphs;
using Algorithms_Sedgewick.HashTable;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.PriorityQueue;
using Algorithms_Sedgewick.Queue;
using Algorithms_Sedgewick.Stack;
using Algorithms_Sedgewick.SymbolTable;

namespace Algorithms_Sedgewick;

/// <summary>
/// Sensible default implementations of data structures.
/// </summary>
public static class DataStructures
{
	public static IStack<T> Stack<T>() => new StackWithResizeableArray<T>();
	
	public static IStack<T> Stack<T>(int capacity) => new StackWithResizeableArray<T>(capacity);
	
	public static IQueue<T> Queue<T>() => new QueueWithResizeableArray<T>();
	
	public static IQueue<T> Queue<T>(int capacity) => new QueueWithResizeableArray<T>(capacity);
	
	public static IRandomAccessList<T> List<T>() => new ResizeableArray<T>();
	
	public static IRandomAccessList<T> List<T>(int capacity) => new ResizeableArray<T>(capacity);
	
	public static ISet<T> Set<T>() => new HashSet<T>();
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(comparer);
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(int capacity, IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(capacity, comparer);
	
	public static IGraph Graph(int vertexCount) => new GraphWithAdjacentsLists(vertexCount);
	
	public static IDigraph Digraph(int vertexCount) => new DigraphWithAdjacentsLists(vertexCount);
	
	// This is actually not a good default, we need a dynamic container. 
	public static IPriorityQueue<T> PriorityQueue<T>(int capacity) 
		where T : IComparable<T> 
		=> new FixedCapacityMinBinaryHeap<T>(capacity);
}

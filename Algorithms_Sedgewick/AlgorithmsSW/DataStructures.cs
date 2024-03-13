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
	/// <summary>
	/// Creates a new array of a specified type and size.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array.</typeparam>
	/// <param name="count">The number of elements in the array.</param>
	/// <returns>A new array of the specified type and size.</returns>
	public static T[] Array<T>(int count) => new T[count];

	/// <summary>
	/// Creates a new array of a specified type and size, and fills it with a specified initial element. 
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array.</typeparam>
	/// <param name="count">The number of elements in the array.</param>
	/// <param name="initialElement">The initial element to fill the array with. Be careful when the supplied value is a
	/// reference type; the same reference will be added to every cell. 
	/// <code>
	/// <![CDATA[
	/// var grid = DataStructures.Array<int[]>(10,  DataStructures.Array<int>(20));
	/// grid[0][0] = 1;
	/// Console.WriteLine($"Value at grid[0][1] is {grid[0][1]}"); // Prints 1, not 0, since the same array is in each cell. 
	/// ]]>
	/// </code>
	///
	/// </param>
	/// <returns>A new array of the specified type and size.</returns>
	public static T[] Array<T>(int count, T initialElement)
	{
		var array = new T[count];
		array.Fill(initialElement);
		return array;
	}

	/// <summary>
	/// Creates a new <see cref="IStack{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the stack.</typeparam>
	public static IStack<T> Stack<T>() => new StackWithResizeableArray<T>();

	/// <summary>
	/// Creates a new <see cref="IStack{T}"/> with the given capacity. 
	/// </summary>
	/// <typeparam name="T">The type of elements in the stack.</typeparam>
	public static IStack<T> Stack<T>(int capacity) => new StackWithResizeableArray<T>(capacity);
	
	public static IQueue<T> Queue<T>() => new QueueWithResizeableArray<T>();
	
	public static IQueue<T> Queue<T>(int capacity) => new QueueWithResizeableArray<T>(capacity);
	
	public static IRandomAccessList<T> ResizeableList<T>() => new ResizeableArray<T>();

	/// <summary>
	/// Creates a new <see cref="IRandomAccessList{T}"/> list with the specified capacity. 
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <returns>A new instance of an empty random access list.</returns>
	public static IRandomAccessList<T> ResizeableList<T>(int capacity) => new ResizeableArray<T>(capacity);
	
	/// <summary>
	/// Creates a new <see cref="IRandomAccessList{T}"/> list with the specified count and initial element.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="count">The initial count of the list.</param>
	/// <param name="initialElement">The initial element to fill the list with. Be careful when the supplied value is a
	/// reference type; the same reference will be added to every cell. 
	/// <code>
	/// <![CDATA[
	/// var grid = DataStructures.List<int[]>(10,  DataStructures.List<int>(20));
	/// grid[0][0] = 1;
	/// Console.WriteLine($"Value at grid[0][1] is {grid[0][1]}"); // Prints 1, not 0, since the same array is in each cell. 
	/// ]]>
	/// </code>
	/// </param>
	/// <returns>A new instance of the <see cref="IRandomAccessList{T}"/> interface.</returns>
	public static IRandomAccessList<T> FixedSizeList<T>(int count, T initialElement)
	{
		var list = FixedSizeList<T>(count);
		list.Fill(initialElement);
		return list; 
	}

	/// <summary>
	/// Creates a new <see cref="IRandomAccessList{T}"/> list with the specified count filed with <see langword="default"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="count">The initial count of the list.</param>
	/// <returns>A new instance of the <see cref="IRandomAccessList{T}"/> interface.</returns>
	public static IRandomAccessList<T> FixedSizeList<T>(int count)
	{
		var list = new ResizeableArray<T>(count);
		list.SetCount(count);
		return list; 
	}
	
	public static Set.ISet<T> Set<T>(IComparer<T> comparer) => new Set.HashSet<T>(comparer);
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(comparer);
	
	public static ISymbolTable<TKey, TValue> HashTable<TKey, TValue>(int capacity, IComparer<TKey> comparer) 
		=> new HashTableWithLinearProbing<TKey, TValue>(capacity, comparer);
	
	public static IGraph Graph(int vertexCount) => new GraphWithAdjacentsLists(vertexCount);
	
	public static IDigraph Digraph(int vertexCount) => new DigraphWithAdjacentsLists(vertexCount);
	
	public static IPriorityQueue<T> PriorityQueue<T>(int capacity, IComparer<T> comparer)
		=> new ResizeableMinBinaryHeap<T>(capacity, comparer);
	
	public static IPriorityQueue<T> PriorityQueue<T>(IComparer<T> comparer)
		=> new ResizeableMinBinaryHeap<T>(comparer);
	
	public static IndexPriorityQueue<T> IndexedPriorityQueue<T>(int capacity, IComparer<T> comparer) => new(capacity, comparer);

	public static IEdgeWeightedGraph<T> EdgeWeightedGraph<T>(int vertexCount) 
		=> new EdgeWeightedGraphWithAdjacencyLists<T>(vertexCount);
	
	public static IEdgeWeightedDigraph<T> EdgeWeightedDigraph<T>(int vertexCount) 
		=> new EdgeWeightedDigraphWithAdjacencyLists<T>(vertexCount);

	public static IEdgeWeightedDigraph<T> ToDigraph<T>(this IEnumerable<(int source, int target, T weight)> edges)
	{
		int maxVertexIndex = edges.Max(edge => Math.Max(edge.source, edge.target));
		var graph = EdgeWeightedDigraph<T>(maxVertexIndex + 1);

		foreach (var edge in edges)
		{
			graph.AddEdge(edge.source, edge.target, edge.weight);
		}

		return graph;
	}
	
	public static IDigraph ToDigraph(this IEnumerable<(int source, int target)> edges)
	{
		int maxVertexIndex = edges.Max(edge => Math.Max(edge.source, edge.target));
		var graph = Digraph(maxVertexIndex + 1);

		foreach (var edge in edges)
		{
			graph.AddEdge(edge.source, edge.target);
		}

		return graph;
	}
	
	public static IEdgeWeightedGraph<T> ToGraph<T>(this IEnumerable<(int source, int target, T weight)> edges)
	{
		int maxVertexIndex = edges.Max(edge => Math.Max(edge.source, edge.target));
		var graph = EdgeWeightedGraph<T>(maxVertexIndex + 1);

		foreach (var edge in edges)
		{
			graph.AddEdge(edge.source, edge.target, edge.weight);
		}

		return graph;
	}
	
	public static IGraph ToGraph(this IEnumerable<(int source, int target)> edges)
	{
		int maxVertexIndex = edges.Max(edge => Math.Max(edge.source, edge.target));
		var graph = Graph(maxVertexIndex + 1);

		foreach (var edge in edges)
		{
			graph.AddEdge(edge.source, edge.target);
		}

		return graph;
	}

	public static IEdgeWeightedDigraph<T> ToDigraph<T>(this string edges)
		where T : IParsable<T> 
		=> edges.ParseEdges<T>().ToDigraph();
	
	public static IEdgeWeightedGraph<T> ToGraph<T>(this string edges)
		where T : IParsable<T> 
		=> edges.ParseEdges<T>().ToGraph();
	
	public static IDigraph ToDigraph(this string edges)
		=> edges.ParseEdges().ToDigraph();
	
	public static IGraph ToGraph(this string edges)
		=> edges.ParseEdges().ToGraph();

	public static (int source, int target, T weight)[] ParseEdges<T>(this string edgeString, IFormatProvider? provider = null)
		where T : IParsable<T>
	{ 
		var tuples = 
			from edge in edgeString.Split(';')
			let parts = edge.Split(',')
			select (int.Parse(parts[0]), int.Parse(parts[1]), T.Parse(parts[2], provider));

		return tuples.ToArray();
	}
	
	public static (int source, int target)[] ParseEdges(this string edgeString)
	{ 
		var tuples = 
			from edge in edgeString.Split(';')
			let parts = edge.Split(',')
			select (int.Parse(parts[0]), int.Parse(parts[1]));

		return tuples.ToArray();
	}
}

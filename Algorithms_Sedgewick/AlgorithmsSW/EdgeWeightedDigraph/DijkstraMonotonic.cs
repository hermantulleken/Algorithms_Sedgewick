namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;
using PriorityQueue;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/*	This is adapted from
	https://github.com/reneargento/algorithms-sedgewick-wayne/blob/master/src/chapter4/section4/Exercise34_MonotonicShortestPath.java

	An example of an algorithm to obscure or difficult for Chat GPT (Dec 2023).
*/
public sealed class DijkstraMonotonic<TWeight>
	where TWeight : INumber<TWeight>
{
	private class Path(TWeight weight, DirectedEdge<TWeight> lastEdge)
	{
		private readonly Path? previousPath;

		public Path(TWeight weight, DirectedEdge<TWeight> directedEdge, Path previousPath)
			: this(weight, directedEdge)
		{
			this.previousPath = previousPath;
		}

		public TWeight Weight => weight;

		public DirectedEdge<TWeight> LastEdge { get; } = lastEdge;

		public IEnumerable<DirectedEdge<TWeight>> GetPath() 
		{
			Stack<DirectedEdge<TWeight>> path = new();
			path.Push(LastEdge);
			var iterator = previousPath;

			while (iterator != null)
			{
				path.Push(iterator.LastEdge);
				iterator = iterator.previousPath;
			}
		
			return path;
		}
	}
	
	private readonly IRandomAccessList<ExtendedComparable<TWeight>> distTo;
	private readonly IRandomAccessList<Path?> pathTo;
	private readonly int source;

	// O(E lg E)
	// If negative edge weights are present, still works but becomes O(2^V)
	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraMonotonic{TWeight}"/> class.
	/// </summary>
	/// <param name="edgeWeightedDigraph">The edge-weighted directed graph on which the Dijkstra's algorithm will be executed.</param>
	/// <param name="source">The source vertex from which distances to all other vertices in the graph will be calculated.</param>
	public DijkstraMonotonic(IReadOnlyEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, int source)
	{
		this.source = source;
		int vertexCount = edgeWeightedDigraph.VertexCount;

		var distToMonotonicAscending = DataStructures.List(vertexCount, ExtendedComparable<TWeight>.PositiveInfinity);
		var distToMonotonicDescending = DataStructures.List(vertexCount, ExtendedComparable<TWeight>.PositiveInfinity);
		distTo = DataStructures.List(vertexCount, ExtendedComparable<TWeight>.PositiveInfinity);
		
		var pathMonotonicAscending = DataStructures.List<Path?>(vertexCount, null);
		var pathMonotonicDescending = DataStructures.List<Path?>(vertexCount, null);
		pathTo = DataStructures.List<Path?>(vertexCount, null);

		// 1- Relax edges in ascending order to get a monotonic increasing shortest path
		var edgesComparatorAscending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => -edge1.Weight.CompareTo(edge2.Weight));

		RelaxAllEdgesInSpecificOrder(
			edgeWeightedDigraph, 
			edgesComparatorAscending, 
			distToMonotonicAscending,
			pathMonotonicAscending, 
			true);

		// 2- Relax edges in descending order to get a monotonic decreasing shortest path
		var edgesComparatorDescending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => edge1.Weight.CompareTo(edge2.Weight));

		RelaxAllEdgesInSpecificOrder(
			edgeWeightedDigraph, 
			edgesComparatorDescending,
			distToMonotonicDescending,
			pathMonotonicDescending, 
			false);

		// 3- Compare distances to get the shortest monotonic path
		CompareMonotonicPathsAndComputeShortest(
			distToMonotonicAscending, 
			pathMonotonicAscending, 
			distToMonotonicDescending, 
			pathMonotonicDescending);
	}
	
	/// <summary>
	/// Gets the shortest path distance from the source vertex to the specified vertex.
	/// </summary>
	/// <param name="vertex">The vertex to which the shortest path distance should be retrieved.</param>
	/// <returns>The shortest path distance from the source vertex to the specified vertex, if the path exists.</returns>
	/// <exception cref="InvalidOperationException">The path does not exist.</exception>
	public TWeight DistTo(int vertex) => distTo[vertex].FiniteValue;

	/// <summary>
	/// Determines whether a path exists from the source vertex to the specified vertex.
	/// </summary>
	/// <param name="vertex">The vertex to which the presence of a path should be checked.</param>
	/// <returns><see langword="true"/> if there is a path from the source vertex to the specified vertex; otherwise, <see langword="false"/>.</returns>
	public bool HasPathTo(int vertex) => distTo[vertex] != ExtendedComparable<TWeight>.PositiveInfinity;

	/// <summary>
	/// Gets the shortest path from the source vertex to the specified vertex.
	/// </summary>
	/// <param name="vertex">The vertex to which the shortest path should be retrieved.</param>
	/// <returns>The shortest path from the source vertex to the specified vertex.</returns>
	public IEnumerable<DirectedEdge<TWeight>>? PathTo(int vertex) 
		=> !HasPathTo(vertex) ? null : source == vertex ? Array.Empty<DirectedEdge<TWeight>>() : pathTo[vertex]!.GetPath();

	private static void AddEdgesOnMonotonicPaths(
		IReadOnlyEdgeWeightedDigraph<TWeight> edgeWeightedDigraph,
		DirectedEdge<TWeight> currentEdge,
		int nextVertexInPath,
		Path currentShortestPath,
		IComparer<DirectedEdge<TWeight>> edgesComparator,
		bool isAscendingOrder,
		IPriorityQueue<Path> priorityQueue)
	{
		var weightInPreviousEdge = currentEdge.Weight;
		var edges = edgeWeightedDigraph.GetIncidentEdges(nextVertexInPath).ToArray();
		Array.Sort(edges, edgesComparator);

		foreach (var edge in edges)
		{
			if ((isAscendingOrder && edge.Weight <= weightInPreviousEdge)
				|| (!isAscendingOrder && edge.Weight >= weightInPreviousEdge))
			{
				break;
			}

			var path = new Path(currentShortestPath.Weight + edge.Weight, edge, currentShortestPath);
			priorityQueue.Push(path);
		}
	}
	
	private void RelaxAllEdgesInSpecificOrder(
		IReadOnlyEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, 
		IComparer<DirectedEdge<TWeight>> edgesComparator, 
		IRandomAccessList<ExtendedComparable<TWeight>> distToVertex,
		IRandomAccessList<Path?> pathToVertex, 
		bool isAscendingOrder)
	{
		pathToVertex.ThrowIfNull();
		var pathComparer = Comparer<Path>.Create((x, y) => x.Weight.CompareTo(y.Weight));
		var priorityQueue = DataStructures.PriorityQueue(pathComparer);
		distToVertex[source] = new(TWeight.Zero);

		AddInitialEdges(edgeWeightedDigraph, edgesComparator, priorityQueue);

		while (!priorityQueue.IsEmpty()) {
			var currentShortestPath = priorityQueue.PopMin();
			var currentEdge = currentShortestPath.LastEdge;
			int nextVertexInPath = currentEdge.Target;
			
			if (pathToVertex[nextVertexInPath] == null
				|| (ExtendedComparable<TWeight>)currentShortestPath.Weight < distToVertex[nextVertexInPath])
			{
				distToVertex[nextVertexInPath] = new(currentShortestPath.Weight);
				pathToVertex[nextVertexInPath] = currentShortestPath;
			}

			AddEdgesOnMonotonicPaths(
				edgeWeightedDigraph, 
				currentEdge,
				nextVertexInPath, 
				currentShortestPath, 
				edgesComparator,
				isAscendingOrder, 
				priorityQueue);
		}
	}

	private void AddInitialEdges(IReadOnlyEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, IComparer<DirectedEdge<TWeight>> edgesComparator, IPriorityQueue<Path> priorityQueue)
	{
		var edges = edgeWeightedDigraph.GetIncidentEdges(source).ToArray();
		Array.Sort(edges, edgesComparator);
		
		foreach (var edge in edges)
		{
			var path = new Path(edge.Weight, edge);
			priorityQueue.Push(path);
		}
	}

	private void CompareMonotonicPathsAndComputeShortest(
		IRandomAccessList<ExtendedComparable<TWeight>> distToMonotonicAscending, 
		IRandomAccessList<Path?> pathMonotonicAscending,
		IRandomAccessList<ExtendedComparable<TWeight>> distToMonotonicDescending,
		IRandomAccessList<Path?> pathMonotonicDescending)
	{
		for (int vertex = 0; vertex < distTo.Count; vertex++) 
		{
			if (distToMonotonicAscending[vertex] <= distToMonotonicDescending[vertex]) 
			{
				distTo[vertex] = distToMonotonicAscending[vertex];
				pathTo[vertex] = pathMonotonicAscending[vertex];
			}
			else
			{
				distTo[vertex] = distToMonotonicDescending[vertex];
				pathTo[vertex] = pathMonotonicDescending[vertex];
			}
		}
	}
}

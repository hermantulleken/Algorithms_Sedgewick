namespace AlgorithmsSW.EdgeWeightedDigraph;

using PriorityQueue;
using Support;
using static System.Diagnostics.Debug;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/*	This is adapted from
	https://github.com/reneargento/algorithms-sedgewick-wayne/blob/master/src/chapter4/section4/Exercise34_MonotonicShortestPath.java
	
	An example of an algorithm to obscure or difficult for Chat GPT (Dec 2023).
*/
public class DijkstraMonotonic<TWeight> 
	: IShortestPath<TWeight>
{
	public enum PathType
	{
		Ascending,
		Descending
	}
	
	private class Path(TWeight weight, DirectedEdge<TWeight> lastEdge)
	{
		public TWeight Weight => weight;

		public DirectedEdge<TWeight> LastEdge => lastEdge;

		public override string ToString()
		{
			return $"{weight}; {lastEdge}";
		}
	}

	private class VertexInformation(DirectedEdge<TWeight>[] edges)
	{
		public DirectedEdge<TWeight>[] Edges { get; } = edges;

		public int CurrentEdgeIteratorPosition { get; private set; } = 0;

		public void IncrementEdgeIteratorPosition() => CurrentEdgeIteratorPosition++;

		public override string ToString()
		{
			return $"{CurrentEdgeIteratorPosition}; {Edges.Pretty()}";
		}
	}
	
	private class PathComparer(IComparer<TWeight> comparer)
		: IComparer<Path>
	{
		public int Compare(Path? x, Path? y) => 
			x == null && y == null 
				? 0 
				: x == null 
					? -1 
					: y == null 
						? 1 
						: comparer.Compare(x.Weight, y.Weight);
	}

	private readonly IEdgeWeightedDigraph<TWeight> graph;
	private readonly IComparer<TWeight> comparer;
	private readonly TWeight maxValue;
	private readonly Func<TWeight, TWeight, TWeight> add;
	private readonly TWeight zero;
	
	private readonly TWeight[] distanceToAscending;
	private readonly DirectedEdge<TWeight>?[] edgeToAscending;

	private readonly TWeight[] distanceToDescending;
	private readonly DirectedEdge<TWeight>?[] edgeToDescending;

	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraMonotonic{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest path in.</param>
	/// <param name="source">The source vertex.</param>
	/// <param name="maxValue">The maximum value of the weights.</param>
	/// <param name="add">The function to add two weights.</param>
	/// <param name="zero">The zero value of the weights.</param>
	public DijkstraMonotonic(
		IEdgeWeightedDigraph<TWeight> graph,
		int source,
		TWeight maxValue,
		Func<TWeight, TWeight, TWeight> add,
		TWeight zero)
	{
		comparer = graph.Comparer;
		this.graph = graph;
		this.add = add;
		this.zero = zero;
		this.maxValue = maxValue;
		
		distanceToAscending = new TWeight[graph.VertexCount];
		distanceToDescending = new TWeight[graph.VertexCount];
		edgeToAscending = new DirectedEdge<TWeight>[graph.VertexCount];
		edgeToDescending = new DirectedEdge<TWeight>[graph.VertexCount];

		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			distanceToAscending[vertex] = maxValue;
			distanceToDescending[vertex] = maxValue;
		}

		// Relax all edges in specific order (ascending and descending)
		RelaxAllEdgesInSpecificOrder(source, PathType.Ascending);
		//RelaxAllEdgesInSpecificOrder(source, PathType.Descending);
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex)
	{
		if (!HasPathTo(vertex))
		{
			throw new InvalidOperationException("No path exists to the given vertex.");
		}
		
		var pathType = GetBestPathType(vertex);
		var distanceTo = GetDistanceTo(pathType);
		
		return distanceTo[vertex];
	}

	/// <inheritdoc />
	public bool HasPathTo(int vertex) => HasAscendingPathTo(vertex) || HasDescendingPathTo(vertex);

	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int vertex)
	{
		if (!HasPathTo(vertex))
		{
			throw new InvalidOperationException("There is no path to the given vertex.");
		}
		
		var pathType = GetBestPathType(vertex);
		var edgeTo = GetEdgeTo(pathType);
		var path = new Stack<DirectedEdge<TWeight>>();
		
		for (var edge = edgeTo[vertex]; edge != null; edge = edgeTo[edge.Source])
		{
			path.Push(edge);
		}
		
		return path;
	}
	
	private bool HasAscendingPathTo(int vertex) => edgeToAscending[vertex] != null;
	
	private bool HasDescendingPathTo(int vertex) => edgeToDescending[vertex] != null;
	
	private PathType GetBestPathType(int vertex)
	{
		Assert(HasPathTo(vertex)); // This method is only called when this has already checked
		
		if (!HasAscendingPathTo(vertex))
		{
			Assert(HasDescendingPathTo(vertex));
			return PathType.Descending;
		}
		
		if (!HasDescendingPathTo(vertex))
		{
			Assert(HasAscendingPathTo(vertex));
			return PathType.Ascending;
		}
		
		Assert(HasAscendingPathTo(vertex));
		Assert(HasDescendingPathTo(vertex));
		return comparer.LessOrEqual(distanceToAscending[vertex], distanceToDescending[vertex])
			? PathType.Ascending 
			: PathType.Descending;
	}
	
	private TWeight[] GetDistanceTo(PathType pathType) 
		=> pathType == PathType.Ascending ? distanceToAscending : distanceToDescending;
	
	private DirectedEdge<TWeight>?[] GetEdgeTo(PathType pathType)
		=> pathType == PathType.Ascending ? edgeToAscending : edgeToDescending;
	
	private Comparison<DirectedEdge<TWeight>> GetCComparison(PathType pathType) 
		=> pathType == PathType.Ascending
			? (edge1, edge2) => comparer.Compare(edge1.Weight, edge2.Weight)
			: (edge1, edge2) => comparer.Compare(edge2.Weight, edge1.Weight);

	private void RelaxAllEdgesInSpecificOrder(int source, PathType pathType)
	{
		var distanceTo = GetDistanceTo(pathType);
		var edgeTo = GetEdgeTo(pathType);
		var verticesInformation = new Dictionary<int, VertexInformation>();

		foreach (int vertex in Enumerable.Range(0, graph.VertexCount))
		{
			var edges = graph.GetIncidentEdges(vertex).ToArray();
			Array.Sort(edges, GetCComparison(pathType));
			verticesInformation[vertex] = new(edges);
		}

		var priorityQueue = DataStructures.PriorityQueue(10_000, new PathComparer(comparer));
		SetInitialDistancesToSource(source, distanceTo, edgeTo, verticesInformation, priorityQueue);
		RelaxEdges(priorityQueue, distanceTo, edgeTo, verticesInformation, pathType);
	}

	private void SetInitialDistancesToSource(
		int source,
		TWeight[] distanceTo,
		DirectedEdge<TWeight>?[] edgeTo,
		Dictionary<int, VertexInformation> verticesInformation,
		IPriorityQueue<Path> priorityQueue)
	{
		foreach (int vertex in Enumerable.Range(0, graph.VertexCount))
		{
			distanceTo[vertex] = maxValue;
		}
		distanceTo[source] = zero;

		var sourceVertexInformation = verticesInformation[source];
		
		while (sourceVertexInformation.CurrentEdgeIteratorPosition < sourceVertexInformation.Edges.Length)
		{
			var edge = sourceVertexInformation.Edges[sourceVertexInformation.CurrentEdgeIteratorPosition];
			sourceVertexInformation.IncrementEdgeIteratorPosition();
			var newDistance = add(zero, edge.Weight);
			
			if (comparer.Compare(distanceTo[edge.Target], newDistance) > 0)
			{
				distanceTo[edge.Target] = newDistance;
				edgeTo[edge.Target] = edge;

				var newPath = new Path(newDistance, edge);
				priorityQueue.Push(newPath);
			}
		}
	}

	private void RelaxEdges(
		IPriorityQueue<Path> priorityQueue, 
		TWeight[] distanceTo, 
		DirectedEdge<TWeight>?[] edgeTo, 
		Dictionary<int, VertexInformation> verticesInformation,
		PathType pathType)
	{
		Tracer.Init();
		int i = 0;
		while (priorityQueue.Count > 0)
		{
			var currentShortestPath = priorityQueue.PopMin();
			Tracer.TraceIteration(i, currentShortestPath);
			Tracer.Trace("Q", priorityQueue.Pretty());
			Tracer.TraceIteration(i, edgeTo.Pretty());
			i++;
			
			var currentEdge = currentShortestPath.LastEdge;
			int nextVertexInPath = currentEdge.Target;
			var nextVertexInformation = verticesInformation[nextVertexInPath];
			var weightInPreviousEdge = currentEdge.Weight;

			// Iterate through all the edges connected to the next vertex in the path
			while (nextVertexInformation.CurrentEdgeIteratorPosition < nextVertexInformation.Edges.Length)
			{
				var edge = nextVertexInformation.Edges[nextVertexInformation.CurrentEdgeIteratorPosition];
				int comparisonResult = comparer.Compare(edge.Weight, weightInPreviousEdge);
				bool shouldBreak = pathType == PathType.Ascending ? comparisonResult <= 0 : comparisonResult >= 0;
				string arrow = pathType == PathType.Ascending ? "\u2191" : "\u2193";
				Tracer.Trace("Next edge", $"{arrow} {weightInPreviousEdge}->{edge} {shouldBreak}");
				
				if (shouldBreak)
				{
					break;
				}

				nextVertexInformation.IncrementEdgeIteratorPosition();
				var newDistance = add(currentShortestPath.Weight, edge.Weight);
				Tracer.Trace("New distance", $"{newDistance} < {distanceTo[edge.Target]}");
				
				// If a shorter path is found, update the distance and enqueue the new path
				if (comparer.Compare(newDistance, distanceTo[edge.Target]) < 0)
				{
					distanceTo[edge.Target] = newDistance;
					edgeTo[edge.Target] = edge;
				
					var newPath = new Path(newDistance, edge);
					priorityQueue.Push(newPath);
				}
			}
		}
	}
}

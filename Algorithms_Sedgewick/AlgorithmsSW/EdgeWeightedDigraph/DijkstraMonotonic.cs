using System.Numerics;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;
using AlgorithmsSW.PriorityQueue;
#if compile
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
#endif

public class Path<TWeight>
{
	private readonly TWeight weight;
	private readonly DirectedEdge<TWeight> lastEdge;

	public Path(TWeight weight, DirectedEdge<TWeight> lastEdge)
	{
		this.weight = weight;
		this.lastEdge = lastEdge;
	}

	public TWeight Weight()
	{
		return weight;
	}

	public DirectedEdge<TWeight> LastEdge()
	{
		return lastEdge;
	}
}

public class PathComparer<TWeight> : IComparer<Path<TWeight>>
	where TWeight : IComparable<TWeight>
{
	public int Compare(Path<TWeight> x, Path<TWeight> y)
	{
		return x.Weight().CompareTo(y.Weight());
	}
}


public class VertexInformation<TWeight>
{
	private readonly DirectedEdge<TWeight>[] edges;
	private int currentEdgeIteratorPosition;

	public VertexInformation(DirectedEdge<TWeight>[] edges)
	{
		this.edges = edges;
		currentEdgeIteratorPosition = 0;
	}

	public void IncrementEdgeIteratorPosition()
	{
		currentEdgeIteratorPosition++;
	}

	public DirectedEdge<TWeight>[] Edges => edges;

	public int CurrentEdgeIteratorPosition => currentEdgeIteratorPosition;
}

public class DijkstraMonotonic<TWeight>
	where TWeight : IComparable<TWeight>, IFloatingPointIeee754<TWeight>
{
	private enum PathType
	{
		Ascending, 
		Descending,
	}
	/*
		private TWeight[] distTo; // length of path to vertex
		private DirectedEdge<TWeight>[] edgeTo; // last edge on path to vertex
	*/

	private readonly TWeight[] distToMonotonicAscending; // length of monotonic ascending path to vertex
	private readonly DirectedEdge<TWeight>[] edgeToMonotonicAscending; // last edge on monotonic ascending path to vertex

	private readonly TWeight[] distToMonotonicDescending; // length of monotonic descending path to vertex
	private readonly DirectedEdge<TWeight>[] edgeToMonotonicDescending; // last edge on monotonic descending path to vertex
	private readonly TWeight[] distTo;
	private readonly DirectedEdge<TWeight>[] edgeTo;

	public DijkstraMonotonic(IEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, int source)
	{
		distToMonotonicAscending = new TWeight[edgeWeightedDigraph.VertexCount];
		distToMonotonicDescending = new TWeight[edgeWeightedDigraph.VertexCount];
		distTo = new TWeight[edgeWeightedDigraph.VertexCount];

		edgeToMonotonicAscending = new DirectedEdge<TWeight>[edgeWeightedDigraph.VertexCount];
		edgeToMonotonicDescending = new DirectedEdge<TWeight>[edgeWeightedDigraph.VertexCount];
		edgeTo = new DirectedEdge<TWeight>[edgeWeightedDigraph.VertexCount];

		for (int vertex = 0; vertex < edgeWeightedDigraph.VertexCount; vertex++)
		{
			distTo[vertex] = TWeight.PositiveInfinity;
			distToMonotonicAscending[vertex] = TWeight.PositiveInfinity;
			distToMonotonicDescending[vertex] = TWeight.PositiveInfinity;
		}

		// 1- Relax edges in ascending order to get a monotonic increasing shortest path
		var edgesComparatorAscending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => -edge1.Weight.CompareTo(edge2.Weight));
		RelaxAllEdgesInSpecificOrder(edgeWeightedDigraph, source, edgesComparatorAscending, distToMonotonicAscending, edgeToMonotonicAscending, true);

		// 2- Relax edges in descending order to get a monotonic decreasing shortest path
		var edgesComparatorDescending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => -edge2.Weight.CompareTo(edge1.Weight));
		RelaxAllEdgesInSpecificOrder(edgeWeightedDigraph, source, edgesComparatorDescending, distToMonotonicDescending, edgeToMonotonicDescending, false);

		// 3- Compare distances to get the shortest monotonic path
		CompareMonotonicPathsAndComputeShortest();
	}

	/*private void RelaxAllEdgesInSpecificOrder(IEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, int source,
											  IComparer<DirectedEdge<TWeight>> edgesComparator, TWeight[] distToVertex,
											  DirectedEdge<TWeight>[] edgeToVertex, bool isAscendingOrder)
	{
		var verticesInformation = new Dictionary<int, VertexInformation>();
		for (int vertex = 0; vertex < edgeWeightedDigraph.VertexCount; vertex++)
		{
			var edges = new List<DirectedEdge<TWeight>>(edgeWeightedDigraph.GetIncidentEdges(vertex));
			edges.Sort(edgesComparator);

			verticesInformation[vertex] = new VertexInformation(edges.ToArray());
		}

		var priorityQueue = new PriorityQueue<Path, TWeight>(new PathComparer());
		distToVertex[source] = 0;

		var sourceVertexInformation = verticesInformation[source];
		foreach (var edge in sourceVertexInformation.Edges)
		{
			Path path = new Path(edge.Weight, edge);
			priorityQueue.Enqueue(path, path.Weight());
		}
		
		//priorityQueue.Enqueue(new Path(0, new DirectedEdge<TWeight>(0,0,0)), 0);

		while (priorityQueue.Count > 0)
		{
			Path currentShortestPath = priorityQueue.Dequeue();
			DirectedEdge<TWeight> currentEdge = currentShortestPath.LastEdge();

			int nextVertexInPath = currentEdge.Target; // Assuming DirectedEdge has Target property for destination vertex
			VertexInformation nextVertexInformation = verticesInformation[nextVertexInPath];

			TWeight weightInPreviousEdge = currentEdge.Weight;

			foreach (var edge in nextVertexInformation.Edges)
			{
				if ((isAscendingOrder && edge.Weight <= weightInPreviousEdge) || (!isAscendingOrder && edge.Weight >= weightInPreviousEdge))
				{
					break;
				}

				if (edgeToVertex[nextVertexInPath] == null || distToVertex[nextVertexInPath] > currentShortestPath.Weight() + edge.Weight)
				{
					edgeToVertex[nextVertexInPath] = currentEdge;
					distToVertex[nextVertexInPath] = currentShortestPath.Weight() + edge.Weight;

					Path newPath = new Path(distToVertex[nextVertexInPath], edge);
					priorityQueue.Enqueue(newPath, newPath.Weight());
				}
			}
		}
	}*/

		private void RelaxAllEdgesInSpecificOrder(
			IEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, int source,
			Comparer<DirectedEdge<TWeight>> edgesComparator, 
			TWeight[] distToVertex,
			DirectedEdge<TWeight>[] edgeToVertex, bool isAscendingOrder) 
		{
			// Create a map with vertices as keys and sorted outgoing edges as values
			var verticesInformation = new VertexInformation<TWeight>[edgeWeightedDigraph.VertexCount];
			for (int vertex = 0; vertex < edgeWeightedDigraph.VertexCount; vertex++)
			{
				var edges = edgeWeightedDigraph.GetIncidentEdges(vertex).ToArray();
				Array.Sort(edges, edgesComparator);
				verticesInformation[vertex] = new(edges);
			}

			var priorityQueue = DataStructures.PriorityQueue(10_000, new PathComparer<TWeight>());

			distToVertex[source] = TWeight.Zero;

			var sourceVertexInformation = verticesInformation[source];
			
			while (sourceVertexInformation.CurrentEdgeIteratorPosition < sourceVertexInformation.Edges.Length)
			{
				var edge = sourceVertexInformation.Edges[sourceVertexInformation.CurrentEdgeIteratorPosition];
				sourceVertexInformation.IncrementEdgeIteratorPosition();

				var path = new Path<TWeight>(edge.Weight, edge);
				priorityQueue.Push(path);
			}

			while (!priorityQueue.IsEmpty())
			{
				var currentShortestPath = priorityQueue.PopMin();
				var currentEdge = currentShortestPath.LastEdge();
				int nextVertexInPath = currentEdge.Target;
				var nextVertexInformation = verticesInformation[nextVertexInPath];
				TWeight weightInPreviousEdge = currentEdge.Weight;

				while (nextVertexInformation.CurrentEdgeIteratorPosition < nextVertexInformation.Edges.Length) 
				{
					var edge =
							verticesInformation[nextVertexInPath].Edges[nextVertexInformation.CurrentEdgeIteratorPosition];

					if ((isAscendingOrder && edge.Weight <= weightInPreviousEdge)
							|| (!isAscendingOrder && edge.Weight >= weightInPreviousEdge)) {
						break;
					}

					nextVertexInformation.IncrementEdgeIteratorPosition();

					bool update = edgeToVertex[nextVertexInPath] == null
								|| currentShortestPath.Weight() < distToVertex[nextVertexInPath];

					//if (update)
					{
						edgeToVertex[nextVertexInPath] = currentShortestPath.LastEdge();
						distToVertex[nextVertexInPath] = currentShortestPath.Weight();

						var path = new Path<TWeight>(currentShortestPath.Weight() + edge.Weight, edge);
						priorityQueue.Push(path);
					}
					/*else
					{
						Console.WriteLine("We sometimes reach this");
					}*/
				}

				if (edgeToVertex[nextVertexInPath] == null) {
					edgeToVertex[nextVertexInPath] = currentEdge;
					distToVertex[nextVertexInPath] = currentShortestPath.Weight();
				}
			}
		}
	
	private void CompareMonotonicPathsAndComputeShortest()
	{
		for (int vertex = 0; vertex < edgeTo.Length; vertex++)
		{
			if (distToMonotonicAscending[vertex] <= distToMonotonicDescending[vertex])
			{
				distTo[vertex] = distToMonotonicAscending[vertex];
				edgeTo[vertex] = edgeToMonotonicAscending[vertex];
			}
			else
			{
				distTo[vertex] = distToMonotonicDescending[vertex];
				edgeTo[vertex] = edgeToMonotonicDescending[vertex];
			}
		}
	}

	private PathType GetBestPath(int vertex)
	{
		if (distToMonotonicAscending[vertex] <= distToMonotonicDescending[vertex])
		{
			return PathType.Ascending;
		}
		else
		{
			return PathType.Descending;
		}
	}

	private TWeight[] GetDistanceTo(PathType pathType) => distTo;/*pathType switch
	{
		PathType.Ascending => distToMonotonicAscending,
		PathType.Descending => distToMonotonicDescending,
		_ => throw new InvalidEnumArgumentException(nameof(pathType), (int)pathType, typeof(PathType)),
	};*/

	private DirectedEdge<TWeight>[] GetEdgeTo(PathType pathType) => edgeTo; /*pathType switch
	{
		PathType.Ascending => edgeToMonotonicAscending,
		PathType.Descending => edgeToMonotonicDescending,
		_ => throw new InvalidEnumArgumentException(nameof(pathType), (int)pathType, typeof(PathType)),
	};*/
		
	public TWeight DistTo(int vertex)
	{
		var pathType = GetBestPath(vertex);
		var distTo = GetDistanceTo(pathType);
		return distTo[vertex];
	}

	public bool HasPathTo(int vertex)
	{
		/*var edgeTo = GetEdgeTo(GetBestPath(vertex));

		return edgeTo[vertex] != null;*/
		var disTo = GetDistanceTo(GetBestPath(vertex));
		
		return distTo[vertex] < TWeight.PositiveInfinity;
	}

	public IEnumerable<DirectedEdge<TWeight>> PathTo(int vertex)
	{
		if (!HasPathTo(vertex))
		{
			return null;
		}
		var edgeTo = GetEdgeTo(GetBestPath(vertex));
		Stack<DirectedEdge<TWeight>> path = new Stack<DirectedEdge<TWeight>>();
		for (DirectedEdge<TWeight> edge = edgeTo[vertex]; edge != null; edge = edgeTo[edge.Source])
		{
			path.Push(edge);
		}
		return path;
	}
}

/*
	Changes made:
	1.	Removed call to CompareMonotonicPathsAndComputeShortest, and instead check for best path when it is requested. 
		The distTo and edgeTo arrays are not used anymore, and have been removed. 
*/

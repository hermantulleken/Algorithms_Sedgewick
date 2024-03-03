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
public class DijkstraMonotonic<TWeight>
	where TWeight : IFloatingPointIeee754<TWeight>
{
	private class VertexInformation(DirectedEdge<TWeight>[] edges)
	{
		private int currentEdgeIteratorPosition = 0;

		public DirectedEdge<TWeight>[] Edges { get; } = edges;

		public int CurrentEdgeIteratorPosition => currentEdgeIteratorPosition;

		public void IncEdgeIteratorPosition() => currentEdgeIteratorPosition++;
	}
	
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
			path.Push(lastEdge);
			var iterator = previousPath;

			while (iterator != null)
			{
				path.Push(iterator.LastEdge);
				iterator = iterator.previousPath;
			}
		
			return path;
		}
	}

	private readonly TWeight[] distTo;
	private readonly Path[] pathTo;


	private readonly int source;

	// O(E lg E)
	// If negative edge weights are present, still works but becomes O(2^V)
	public DijkstraMonotonic(IEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, int source)
	{
		this.source = source;

		var distToMonotonicAscending = new TWeight[edgeWeightedDigraph.VertexCount];
		var distToMonotonicDescending = new TWeight[edgeWeightedDigraph.VertexCount];
		distTo = new TWeight[edgeWeightedDigraph.VertexCount];
		
		distToMonotonicAscending.Fill(TWeight.PositiveInfinity);
		distToMonotonicDescending.Fill(TWeight.PositiveInfinity);
		distTo.Fill(TWeight.PositiveInfinity);

		var pathMonotonicAscending = new Path[edgeWeightedDigraph.VertexCount];
		var pathMonotonicDescending = new Path[edgeWeightedDigraph.VertexCount];
		pathTo = new Path[edgeWeightedDigraph.VertexCount];

		// 1- Relax edges in ascending order to get a monotonic increasing shortest path
		var edgesComparatorAscending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => -edge1.Weight.CompareTo(edge2.Weight));

		relaxAllEdgesInSpecificOrder(
			edgeWeightedDigraph, 
			edgesComparatorAscending, 
			distToMonotonicAscending,
			pathMonotonicAscending, 
			true);

		// 2- Relax edges in descending order to get a monotonic decreasing shortest path
		var edgesComparatorDescending = Comparer<DirectedEdge<TWeight>>.Create((edge1, edge2) => edge1.Weight.CompareTo(edge2.Weight));

		relaxAllEdgesInSpecificOrder(
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

	private void relaxAllEdgesInSpecificOrder(
		IEdgeWeightedDigraph<TWeight> edgeWeightedDigraph, 
		Comparer<DirectedEdge<TWeight>> edgesComparator, 
		TWeight[] distToVertex,
		Path[] pathToVertex, bool isAscendingOrder)
	{
		// Create a map with vertices as keys and sorted outgoing edges as values
		var verticesInformation = DataStructures.HashTable<int, VertexInformation>(Comparer<int>.Default);
		
		for (int vertex = 0; vertex < edgeWeightedDigraph.VertexCount; vertex++) 
		{
			var edges = edgeWeightedDigraph.GetIncidentEdges(vertex).ToArray();
			Array.Sort(edges, edgesComparator);
			verticesInformation[vertex] = new(edges);
		}

		var pathComparer = Comparer<Path>.Create((x, y) => x.Weight.CompareTo(y.Weight));
		var priorityQueue = DataStructures.PriorityQueue(pathComparer);
		distToVertex[source] = TWeight.Zero;

		var sourceVertexInformation = verticesInformation[source];
		while (sourceVertexInformation.CurrentEdgeIteratorPosition < sourceVertexInformation.Edges.Length)
		{
			var edge = sourceVertexInformation.Edges[sourceVertexInformation.CurrentEdgeIteratorPosition];
			sourceVertexInformation.IncEdgeIteratorPosition();

			var path = new Path(edge.Weight, edge);
			priorityQueue.Push(path);
		}

		while (!priorityQueue.IsEmpty()) {
			var currentShortestPath = priorityQueue.PopMin();
			var currentEdge = currentShortestPath.LastEdge;

			int nextVertexInPath = currentEdge.Target;
			var nextVertexInformation = verticesInformation[nextVertexInPath];

			if (pathToVertex[nextVertexInPath] == null
				|| currentShortestPath.Weight < distToVertex[nextVertexInPath])
			{
				distToVertex[nextVertexInPath] = currentShortestPath.Weight;
				pathToVertex[nextVertexInPath] = currentShortestPath;
			}

			TWeight weightInPreviousEdge = currentEdge.Weight;

			while (nextVertexInformation.CurrentEdgeIteratorPosition < nextVertexInformation.Edges.Length)
			{
				var edge =
					verticesInformation[nextVertexInPath].Edges[nextVertexInformation.CurrentEdgeIteratorPosition];

				if ((isAscendingOrder && edge.Weight <= weightInPreviousEdge)
					|| (!isAscendingOrder && edge.Weight >= weightInPreviousEdge)) {
					break;
				}

				nextVertexInformation.IncEdgeIteratorPosition();
				var path = new Path(currentShortestPath.Weight + edge.Weight, edge, currentShortestPath);
				priorityQueue.Push(path);
			}
		}
	}

	private void CompareMonotonicPathsAndComputeShortest(
		TWeight[] distToMonotonicAscending, 
		Path[] pathMonotonicAscending,
		TWeight[] distToMonotonicDescending,
		Path[] pathMonotonicDescending) 
	{
		for (int vertex = 0; vertex < distTo.Length; vertex++) {
			if (distToMonotonicAscending[vertex] <= distToMonotonicDescending[vertex]) {
				distTo[vertex] = distToMonotonicAscending[vertex];
				pathTo[vertex] = pathMonotonicAscending[vertex];
			} else {
				distTo[vertex] = distToMonotonicDescending[vertex];
				pathTo[vertex] = pathMonotonicDescending[vertex];
			}
		}
	}

	public TWeight DistTo(int vertex) {
		return distTo[vertex];
	}

	public bool HasPathTo(int vertex) {
		return distTo[vertex] != TWeight.PositiveInfinity;
	}

	public IEnumerable<DirectedEdge<TWeight>>? PathTo(int vertex)
	{
		return !HasPathTo(vertex) ? null : source == vertex ? Array.Empty<DirectedEdge<TWeight>>() : pathTo[vertex].GetPath();
	}
}

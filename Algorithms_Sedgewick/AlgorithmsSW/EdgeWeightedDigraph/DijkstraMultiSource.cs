namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using Support;
using static System.Diagnostics.Debug;

/// <summary>
/// A modified version of Dijkstra's algorithm that finds the shortest path from any source to any vertex.
/// </summary>
// Note: only implemented for weights of type double.
[ExerciseReference(4, 4, 24)]
public class DijkstraMultiSource
{
	private readonly DirectedEdge<double>?[] edgeTo;
	private readonly Set.ISet<int> sourcesSet;
	private readonly double[] distanceTo;

	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraMultiSource"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="sources">The source vertexes to find the shortest paths from.</param>
	/// <exception cref="ArgumentException"><paramref name="graph"/> contains negative weights.</exception>
	public DijkstraMultiSource(IEdgeWeightedDigraph<double> graph, IEnumerable<int> sources)
	{
		sourcesSet = DataStructures.Set(Comparer<int>.Default);
		distanceTo = new double[graph.VertexCount];
		edgeTo = new DirectedEdge<double>?[graph.VertexCount];
		int pathsFound = 0;
		
		int[] sourcesArray = sources.ToArray();
		
		distanceTo.Fill(double.MaxValue);
		var queue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<double>.Default);
		
		foreach (int source in sourcesArray)
		{
			sourcesSet.Add(source);
			distanceTo[source] = 0;
			edgeTo[source] = null;
			queue.Insert(source, 0);
		}
		
		while (!queue.IsEmpty && pathsFound < sourcesArray.Length)
		{
			(int nextNode, double distanceToSource) = queue.PopMin();
			
			foreach (var edge in graph.GetIncidentEdges(nextNode))
			{
				Assert(!sourcesArray.Contains(edge.Target)); // Because of the priority queue, this should never happen.
				
				if (edge.Weight < 0)
				{
					throw new ArgumentException("Negative weights are not allowed.", nameof(graph));
				}
				
				// Not visited.
				if (edgeTo[edge.Target] == null) 
				{
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = distanceToSource + edge.Weight;
					queue.Insert(edge.Target, distanceTo[edge.Target]);
				}
				// Found a shorter path.
				else if (distanceTo[edge.Target] > distanceToSource + edge.Weight) 
				{
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = distanceToSource + edge.Weight;
					queue.UpdateValue(edge.Target, distanceTo[edge.Target]);
				}
			}
		}
	}
	
	public bool PathExists(int vertex) => sourcesSet.Contains(vertex) || edgeTo[vertex] != null;

	/// <summary>
	/// Gets the path from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public DirectedPath<double> GetPath(int vertex)
		=> PathExists(vertex)
			? GetPathImpl(vertex)
			: throw new InvalidOperationException("No path exists.");

	/// <summary>
	/// Gets the distance from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public double Distance(int sourceVertex)
		=> sourcesSet.Contains(sourceVertex)
			? 0.0
			: PathExists(sourceVertex)
				? distanceTo[sourceVertex]
				: throw new InvalidOperationException("No path exists.");

	private DirectedPath<double> GetPathImpl(int sink)
	{
		var stack = new Stack<DirectedEdge<double>>();
		
		IterationGuard.Reset();
		for (var edge = edgeTo[sink]; edge != null; edge = edgeTo[edge.Source])
		{
			IterationGuard.Inc();
			stack.Push(edge);
		}

		return new(stack.ToResizableArray());
	}
}

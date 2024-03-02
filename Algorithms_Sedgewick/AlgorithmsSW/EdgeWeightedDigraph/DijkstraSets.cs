namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using Support;
using static System.Diagnostics.Debug;

/// <summary>
/// A modified version of Dijkstra's algorithm that finds the shortest path from any of a set of sources to a set of
/// sinks.
/// </summary>
/// <remarks>
/// This implementation can be faster than vanilla Dijkstra since we can stop early once we found a sink.
/// </remarks>
// Note: only implemented for weights of type double.
[ExerciseReference(4, 4, 25)]
public class DijkstraSets
{
	private readonly double distance;
	private readonly DirectedPath<double> path;
	
	public bool PathExists { get; }
	
	public double Distance
		=> PathExists ? distance : throw new InvalidOperationException("No path exists.");

	public DirectedPath<double> Path
		=> PathExists ? path : throw new InvalidOperationException("No path exists.");

	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraSets"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="sources">The source vertexes to find the shortest paths from.</param>
	/// <param name="sinks">The sink vertexes to find the shortest paths to.</param>
	/// <exception cref="ArgumentException"><paramref name="graph"/> contains negative weights.</exception>
	public DijkstraSets(IEdgeWeightedDigraph<double> graph, IEnumerable<int> sources, IEnumerable<int> sinks)
	{
		var distanceTo = new double[graph.VertexCount];
		var edgeTo = new DirectedEdge<double>?[graph.VertexCount];
		
		int[] sourcesArray = sources.ToArray();
		var sinksSet = sinks.ToSet(Comparer<int>.Default);
		
		distanceTo.Fill(double.MaxValue);
		var queue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<double>.Default);
		
		foreach (int source in sourcesArray)
		{
			distanceTo[source] = 0;
			edgeTo[source] = null;
			queue.Insert(source, 0);
		}
		
		while (!queue.IsEmpty && !PathExists)
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

				if (!sinksSet.Contains(edge.Target))
				{
					continue;
				}

				PathExists = true;
				path = GetPathImpl(edgeTo, edge.Target);
				distance = distanceTo[edge.Target];
				break;
			}
		}
	}

	private DirectedPath<double> GetPathImpl(DirectedEdge<double>?[] edgeTo, int sink)
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

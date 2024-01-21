namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using List;
using static System.Diagnostics.Debug;

/// <summary>
/// A modified version of Dijkstra's algorithm that finds the shortest path from a source to a sink.
/// </summary>
/// <remarks>
/// This implementation can be faster than vanilla Dijkstra since we can stop early once we processed the sink.
/// </remarks>
// Note: only implemented for weights of type double.
[ExerciseReference(4, 4, 23)]
public class DijkstraSourceSink<TWeight>
{
	private readonly TWeight distance;

	private readonly IReadonlyRandomAccessList<DirectedEdge<TWeight>>? path;
	
	/// <summary>
	/// Gets a value indicating whether a path exists from the source to the sink.
	/// </summary>
	[MemberNotNullWhen(true, nameof(path))]
	[MemberNotNullWhen(true, nameof(Path))]
	public bool PathExists { get; }

	/// <summary>
	/// Gets the path from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public IReadonlyRandomAccessList<DirectedEdge<TWeight>>? Path
		=> PathExists ? path : throw new InvalidOperationException("No path exists.");

	/// <summary>
	/// Gets the distance from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public TWeight Distance => PathExists ? distance : throw new InvalidOperationException("No path exists.");
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraSourceSink{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest path in.</param>
	/// <param name="source">The source vertex to find the shortest path from.</param>
	/// <param name="sink">The sink vertex to find the shortest path to.</param>
	/// <exception cref="ArgumentException"><paramref name="graph"/> contains negative weights.</exception>
	public DijkstraSourceSink(
		IEdgeWeightedDigraph<TWeight> graph,
		int source, 
		int sink, 
		Func<TWeight, TWeight, TWeight> add, 
		TWeight zero, 
		TWeight maxValue)
	{
		var distanceTo = new TWeight[graph.VertexCount];
		var edgeTo = new DirectedEdge<TWeight>?[graph.VertexCount];
		
		distanceTo.Fill(maxValue);
		distanceTo[source] = zero;
		edgeTo[source] = null;
		
		var queue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<TWeight>.Default);
		queue.Insert(source, zero);
		
		while (!queue.IsEmpty && !PathExists)
		{
			(int nextNode, TWeight distanceToSource) = queue.PopMin();
			
			foreach (var edge in graph.GetIncidentEdges(nextNode))
			{
				Assert(edge.Target != source); // Because of the priority queue, this should never happen.
				
				if (graph.Comparer.Less(edge.Weight, zero))
				{
					throw new ArgumentException("Negative weights are not allowed.", nameof(graph));
				}
				
				// Not visited.
				if (edgeTo[edge.Target] == null) 
				{
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = add(distanceToSource, edge.Weight);
					queue.Insert(edge.Target, distanceTo[edge.Target]);
				}
				// Found a shorter path.
				else if (graph.Comparer.Less(add(distanceToSource, edge.Weight), distanceTo[edge.Target]))
				{
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = add(distanceToSource, edge.Weight);
					queue.UpdateValue(edge.Target, distanceTo[edge.Target]);
				}

				if (edge.Target == sink)
				{
					distance = distanceTo[edge.Target];
					PathExists = true;
					path = GetPath(edgeTo, source, sink);
					break;
				}
			}
		}
	}

	private IReadonlyRandomAccessList<DirectedEdge<TWeight>>? GetPath(DirectedEdge<TWeight>?[] edgeTo, int source, int sink)
	{
		Assert(PathExists);
		
		var stack = new Stack<DirectedEdge<TWeight>>();
		
		for (var edge = edgeTo[sink]; edge != null; edge = edgeTo[edge.Source])
		{
			stack.Push(edge);
		}

		return stack.ToResizableArray();
	}
}

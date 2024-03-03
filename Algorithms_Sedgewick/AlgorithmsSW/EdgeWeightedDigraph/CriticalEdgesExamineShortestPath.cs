namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

/// <summary>
/// Given two nodes, the edge that when removed will increases the shortest path between them the most. 
/// </summary>
[ExerciseReference(4, 4, 37)]
public class CriticalEdgesExamineShortestPath<TWeight>
	where TWeight : IFloatingPoint<TWeight>, IMinMaxValue<TWeight>
{
	/// <summary>
	/// Gets a value indicating whether whether the graph has a critical edge.
	/// </summary>
	[MemberNotNullWhen(true, nameof(CriticalEdge))]
	public bool HasCriticalEdge => CriticalEdge != null;
	
	/// <summary>
	/// Gets the critical edge, if any.
	/// </summary>
	/// <remarks>The critical edge is the edge that when removed will increases the shortest path between the source
	/// and the destination the most.</remarks>
	public DirectedEdge<TWeight>? CriticalEdge { get; }

	/// <summary>
	/// Gets the distance of the shortest path oif the critical edge would be removed.  
	/// </summary>
	public TWeight DistanceWithoutCriticalEdge { get; }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="CriticalEdgesExamineShortestPath{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the critical edge in.</param>
	/// <param name="source">The source vertex.</param>
	/// <param name="destination">The destination vertex.</param>
	public CriticalEdgesExamineShortestPath(
		IEdgeWeightedDigraph<TWeight> graph,
		int source, 
		int destination)
	{
		var maxDistance = TWeight.Zero;
		DirectedEdge<TWeight>? maxEdge = null;
		
		// We only need to check the edges on the path
		DijkstraSourceSink<TWeight> dijkstra = new(graph, source, destination);

		if (dijkstra.PathExists)
		{
			// Or should we throw an exception?
			return;
		}
		
		var shortestPathEdges = dijkstra.Path!;
		
		foreach (var edge in shortestPathEdges)
		{
			graph.RemoveEdge(edge);
			dijkstra = new(graph, source, destination);

			if (maxDistance < dijkstra.Distance)
			{
				maxDistance = dijkstra.Distance;
				maxEdge = edge;
			}
			
			graph.AddEdge(edge);
		}

		CriticalEdge = maxEdge;
		DistanceWithoutCriticalEdge = maxDistance;
	}
}

namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

/// <inheritdoc />
[ExerciseReference(4, 4, 37)]
public class CriticalEdgesExamineShortestPath<TWeight> : ICriticalEdge<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	/// <inheritdoc />
	[MemberNotNullWhen(true, nameof(CriticalEdge))]
	public bool HasCriticalEdge => CriticalEdge != null;
	
	/// <inheritdoc />
	public DirectedEdge<TWeight>? CriticalEdge { get; }

	/// <inheritdoc />
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

		if (!dijkstra.PathExists)
		{
			// Or should we throw an exception?
			return;
		}
		
		var shortestPathEdges = dijkstra.Path!;
		
		foreach (var edge in shortestPathEdges)
		{
			graph.RemoveEdge(edge);
			dijkstra = new(graph, source, destination);

			if (dijkstra.PathExists && dijkstra.Distance > maxDistance)
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

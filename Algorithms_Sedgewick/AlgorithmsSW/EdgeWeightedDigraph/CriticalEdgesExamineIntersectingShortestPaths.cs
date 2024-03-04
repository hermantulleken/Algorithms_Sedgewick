namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;

/// <inheritdoc />
[ExerciseReference(4, 4, 37)]
public class CriticalEdgesExamineIntersectingShortestPaths<TWeight> : ICriticalEdge<TWeight>
	where TWeight : IFloatingPoint<TWeight>, IMinMaxValue<TWeight>
{
	/// <inheritdoc />
	public bool HasCriticalEdge => CriticalEdge != null;
	
	/// <inheritdoc />
	public DirectedEdge<TWeight>? CriticalEdge { get; }

	/// <inheritdoc />
	public TWeight DistanceWithoutCriticalEdge { get; }
	
	/// <summary>
	/// Initializes a new instance of the
	/// <see cref="CriticalEdgesExamineIntersectingShortestPaths{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the critical edge in.</param>
	/// <param name="source">The source vertex.</param>
	/// <param name="destination">The destination vertex.</param>
	public CriticalEdgesExamineIntersectingShortestPaths(
		IEdgeWeightedDigraph<TWeight> graph,
		int source, 
		int destination)
	{
		var maxDistance = TWeight.Zero;
		DirectedEdge<TWeight>? maxEdge = null;
		
		// We only need to check the edges on the path
		OverlappingYensAlgorithm<TWeight> algorithm = new OverlappingYensAlgorithm<TWeight>(
			graph, 
			source, 
			destination);

		var edges = algorithm.Intersection;

		foreach (var weightlessEdge in edges)
		{
			var edge = graph.GetUniqueEdge(weightlessEdge.Item1, weightlessEdge.Item2);
			graph.RemoveEdge(edge);
			var dijkstra = new DijkstraSourceSink<TWeight>(graph, source, destination);

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

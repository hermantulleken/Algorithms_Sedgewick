namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics;
using System.Numerics;
using List;

/// <inheritdoc />
[ExerciseReference(4, 4, 37)]
public class CriticalEdgesExamineIntersectingShortestPaths<TWeight> : ICriticalEdge<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	public IEnumerable<DirectedEdge<TWeight>> CriticalEdges { get; }

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
		DirectedEdge<TWeight>? maxEdge = null;
		
		// We only need to check the edges on the path
		OverlappingYensAlgorithm<TWeight> algorithm = new OverlappingYensAlgorithm<TWeight>(
			graph, 
			source, 
			destination);
		
		var edges = algorithm.CriticalEdges; // TODO: can be null, what do we do then?

		if (edges != null && edges.Any())
		{
			CriticalEdges = edges.Select(edge => graph.GetUniqueEdge(edge.Item1, edge.Item2));
			Debug.Assert(algorithm.PathRank >= 1);
			DistanceWithoutCriticalEdge = algorithm.GetPath(algorithm.PathRank - 1).Distance;
		}
		else
		{
			CriticalEdges = Array.Empty<DirectedEdge<TWeight>>();
		}
	}
}

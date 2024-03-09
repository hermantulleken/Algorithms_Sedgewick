namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics;
using System.Numerics;

/// <inheritdoc />
[ExerciseReference(4, 4, 37)]
public class CriticalEdgesExamineIntersectingShortestPaths<TWeight> : ICriticalEdge<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> CriticalEdges { get; }

	/// <inheritdoc />
	public TWeight? DistanceWithoutCriticalEdge { get; } = default;
	
	/// <inheritdoc />
	public bool HasCriticalEdges { get; private set; }
	
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
			HasCriticalEdges = true;
		}
		else
		{
			CriticalEdges = Array.Empty<DirectedEdge<TWeight>>();
			HasCriticalEdges = false;
		}
	}
}

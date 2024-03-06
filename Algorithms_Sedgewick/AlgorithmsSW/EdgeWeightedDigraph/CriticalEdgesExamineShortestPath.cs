namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using static System.Diagnostics.Debug;

/// <inheritdoc />
public class CriticalEdgesExamineShortestPath<TWeight> : ICriticalEdge<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> CriticalEdges { get; } = [];

	/// <inheritdoc />
	public TWeight DistanceWithoutCriticalEdge { get; } = TWeight.Zero;
	
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
		var maxAggregator = new MaxAggregator<DirectedEdge<TWeight>, TWeight>(Comparer<TWeight>.Default);
		
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

			if (dijkstra.PathExists)
			{
				maxAggregator.AddIfBigger(edge, dijkstra.Distance);
			}

			graph.AddEdge(edge);
		}

		Assert(maxAggregator.HasMaxComparisonValue);
		
		CriticalEdges = maxAggregator.MaxValues;
		DistanceWithoutCriticalEdge = maxAggregator.MaxComparisonComparisonValue;
	}
}

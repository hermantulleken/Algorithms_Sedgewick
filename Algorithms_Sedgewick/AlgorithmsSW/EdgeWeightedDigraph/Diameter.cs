namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using PathTerminals = (int source, int target);

/// <summary>
/// A class that finds the diameter of a directed graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
[ExerciseReference(4, 4, 8)]
public class Diameter<TWeight>
	where TWeight : IFloatingPoint<TWeight>, IMinMaxValue<TWeight>
{
	/// <summary>
	///  This class allow us to find the maximum of an item by some metric, while keeping both together.
	/// </summary>
	private class ItemMetricComparer<TItem, TMetric>(IComparer<TMetric> metricComparer)
		: IComparer<(TItem itme, TMetric metric)>
	{
		public int Compare((TItem itme, TMetric metric) x, (TItem itme, TMetric metric) y)
			=> metricComparer.Compare(x.metric, y.metric);
	}
	
	/// <summary>
	/// The terminal vertices of the diameter.
	/// </summary>
	public PathTerminals TerminalVertices { get; }
	
	/// <summary>
	/// The path of the diameter.
	/// </summary>
	/// <remarks>This is the shortest path from the source to the target of <see cref="TerminalVertices"/>.</remarks>
	public IEnumerable<DirectedEdge<TWeight>> Path { get; }
	
	/// <summary>
	/// The total distance of the diameter.
	/// </summary>
	public TWeight Distance { get; }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Diameter{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the diameter of.</param>
	public Diameter(IReadOnlyEdgeWeightedDigraph<TWeight> graph)
	{
		var allPairs = new DijkstraAllPairs<TWeight>(graph);
		
		(PathTerminals edge, TWeight distance) AugmentWithDistance(PathTerminals pair)
			=> (pair, allPairs.GetDistance(pair.source, pair.target));

		(TerminalVertices, Distance) = graph
			.Vertexes
			.GenerateDistinctPairs()
			.Select(AugmentWithDistance)
			.Max(new ItemMetricComparer<PathTerminals, TWeight>(Comparer<TWeight>.Default));

		Path = allPairs.GetPath(TerminalVertices.source, TerminalVertices.target);
	}
}

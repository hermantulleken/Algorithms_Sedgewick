namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// An algorithm for calculating the minimum spanning tree of a edge weighted graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IMst<TWeight>
{
	/// <summary>
	/// Gets the edges that is part of the minimum spanning tree.
	/// </summary>
	public IEnumerable<Edge<TWeight>> Edges { get; }

	/// <summary>
	/// Gets the total weight of the minimum spanning tree.
	/// </summary>
	/// <param name="add">The function to add two weights.</param>
	/// <returns>The total weight of the minimum spanning tree.</returns>
	public TWeight GetTotalWeight(Func<TWeight, TWeight, TWeight> add) 
		=> Edges
			.Select(e => e.Weight)
			.Aggregate(add);
}

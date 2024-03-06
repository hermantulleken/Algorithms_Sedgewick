namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

/// <summary>
/// An algorithm for calculating the minimum spanning tree of a edge weighted graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IMst<TWeight>
	where TWeight : INumber<TWeight>
{
	/// <summary>
	/// Gets the edges that is part of the minimum spanning tree.
	/// </summary>
	public IEnumerable<Edge<TWeight>> Edges { get; }

	/// <summary>
	/// Gets the total weight of the minimum spanning tree.
	/// </summary>
	/// <returns>The total weight of the minimum spanning tree.</returns>
	public TWeight GetTotalWeight() 
		=> Edges
			.Select(e => e.Weight)
			.Aggregate((x, y) => x + y);
}

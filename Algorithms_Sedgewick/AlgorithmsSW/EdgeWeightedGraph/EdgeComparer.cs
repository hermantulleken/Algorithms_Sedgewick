namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

/// <summary>
/// A comparer for weighted edges.
/// </summary>
/// <typeparam name="TWeight">The type of the weights.</typeparam>
public class EdgeComparer<TWeight> : IComparer<Edge<TWeight>>
	where TWeight : IComparisonOperators<TWeight, TWeight, bool>
{
	/// <summary>
	/// Compares two edges by their weights.
	/// </summary>
	public int Compare(Edge<TWeight>? x, Edge<TWeight>? y)
		=> x == null && y == null
			? 0
			: x == null
				? -1
				: y == null
					? 1
					: x.Weight < y.Weight
						? -1
							: x.Weight > y.Weight
							? 1
							: 0;
}

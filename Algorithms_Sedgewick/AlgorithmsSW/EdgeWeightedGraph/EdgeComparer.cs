namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// A comparer for weighted edges.
/// </summary>
/// <param name="comparer">The comparer to use for the weights.</param>
/// <typeparam name="TWeights">The type of the weights.</typeparam>
public class EdgeComparer<TWeights>(IComparer<TWeights> comparer) : IComparer<Edge<TWeights>>
{
	/// <summary>
	/// Compares two edges by their weight.
	/// </summary>
	public int Compare(Edge<TWeights>? x, Edge<TWeights>? y) 
		=> x == null && y == null
			? 0 
			: x == null
				? -1 
				: y == null 
					? 1 
					: comparer.Compare(x.Weight, y.Weight);
}

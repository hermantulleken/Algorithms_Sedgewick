namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// Represents a directed edge with a weight.
/// </summary>
/// <param name="source">The vertex the edge is directed from.</param>
/// <param name="target">The vertex the edge is directed to.</param>
/// <param name="weight">The weight of the edge.</param>
/// <typeparam name="TWeight">The type of the weight. See [Weights](../content/Weights.md).</typeparam>
public class DirectedEdge<TWeight>(int source, int target, TWeight weight)
{
	/// <summary>
	/// Gets the vertex the edge is directed from.
	/// </summary>
	public int Source { get; } = source;
	
	/// <summary>
	/// Gets the vertex the edge is directed to.
	/// </summary>
	public int Target { get; } = target;
	
	/// <summary>
	/// Gets the weight of the edge.
	/// </summary>
	public TWeight Weight { get; } = weight;

	/// <inheritdoc/>
	public override string ToString() => $"[{Source}, {Target}: {Weight}]";
}

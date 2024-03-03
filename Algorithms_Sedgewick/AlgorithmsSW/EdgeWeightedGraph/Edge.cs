namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// Represents an edge in an edge-weighted graph.
/// </summary>
/// <param name="Vertex0">One vertex of the edge.</param>
/// <param name="Vertex1">The other vertex of the edge.</param>
/// <param name="Weight">The weight of the edge.</param>
/// <typeparam name="TWeight">The type of weight. See [Weights](../content/Weights.md).</typeparam>
public record Edge<TWeight>(int Vertex0, int Vertex1, TWeight Weight)
{
	/// <summary>
	/// Given one vertex of the edge, returns the other vertex.
	/// </summary>
	public int OtherVertex(int vertex) 
		=> vertex == Vertex1 
			? Vertex0 
			: vertex == Vertex0 
				? Vertex1 
				: throw new ArgumentException("Invalid vertex.");

	/// <inheritdoc />
	public override string ToString() => $"[{Vertex0}, {Vertex1}: {Weight}]";
}

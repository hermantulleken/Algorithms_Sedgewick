namespace AlgorithmsSW.EdgeWeightedGraph;

public record Edge<T>(int Vertex0, int Vertex1, T Weight)
{
	public int OtherVertex(int vertex) 
		=> vertex == Vertex1 
			? Vertex0 
			: vertex == Vertex0 
				? Vertex1 
				: throw new ArgumentException("Invalid vertex.");

	public override string ToString() => $"[{Vertex0}, {Vertex1}: {Weight}]";
}

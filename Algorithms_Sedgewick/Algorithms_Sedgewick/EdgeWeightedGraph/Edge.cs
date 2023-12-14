namespace Algorithms_Sedgewick.EdgeWeightedGraph;

public class Edge<T> : IComparable<Edge<T>>
	where T : IComparable<T>
{
	private readonly int vertx0;
	private readonly int vertex1;
	
	public T Weight { get; }

	public Edge(int vertx0, int vertex1, T weight)
	{
		this.vertx0 = vertx0;
		this.vertex1 = vertex1;
		Weight = weight;
	}

	public int Vertex1 => vertex1;
	
	public int Vertex0 => vertx0;
	
	public int OtherVertex(int vertex) 
		=> vertex == vertex1 
			? vertx0 
			: vertex == vertx0 
				? vertex1 
				: throw new ArgumentException("Invalid vertex.");

	public int CompareTo(Edge<T> other) => Weight.CompareTo(other.Weight);

	public override string ToString() => $"[{vertx0}, {vertex1}: {Weight}]";
}

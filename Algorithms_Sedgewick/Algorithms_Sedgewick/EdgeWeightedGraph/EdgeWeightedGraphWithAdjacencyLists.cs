using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.EdgeWeightedGraph;

public class EdgeWeightedGraphWithAdjacencyLists<T>
	where T : IComparable<T>
{
	private ResizeableArray<Edge<T>>[] adjacents;
	public IComparer<T> Comparer { get;  }
	
	IEnumerable<int> Vertexes => Enumerable.Range(0, VertexCount);

	IEnumerable<Edge<T>> Edges => 
		from vertex in Vertexes 
		from edge in GetIncidentEdges(vertex) 
		where edge.Vertex0 <= edge.Vertex1 
		select edge;

	public int VertexCount { get; }

	public int EdgeCount { get; private set; }
	
	public EdgeWeightedGraphWithAdjacencyLists(int vertexCount, IComparer<T> comparer)
	{
		adjacents = new ResizeableArray<Edge<T>>[VertexCount];
		VertexCount = vertexCount;
		this.Comparer = comparer;
	}
	
	public void AddEdge(Edge<T> edge)
	{
		adjacents[edge.Vertex0].Add(edge);

		if (edge.Vertex0 != edge.Vertex1)
		{
			adjacents[edge.Vertex1].Add(edge);
		}
	}
	
	public void AddEdge(int vertex0, int vertex1, T weight)
	{
		ValidateVertex(vertex0);
		ValidateVertex(vertex1);
		
		AddEdge(new Edge<T>(vertex0, vertex1, weight));
	}
	
	public IEnumerable<Edge<T>> GetIncidentEdges(int vertex)
	{
		ValidateVertex(vertex);

		return adjacents[vertex];
	}
	
	private void ValidateVertex(int vertex)
	{
		if (vertex < 0 || vertex >= VertexCount)
		{
			throw new ArgumentException("Invalid vertex.");
		}
	}
}

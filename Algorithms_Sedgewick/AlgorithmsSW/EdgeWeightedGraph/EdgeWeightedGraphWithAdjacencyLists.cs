using AlgorithmsSW.List;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class EdgeWeightedGraphWithAdjacencyLists<T>
{
	private readonly ResizeableArray<Edge<T>>[] adjacents;
	
	public IComparer<T> Comparer { get;  }

	public IEnumerable<int> Vertexes => Enumerable.Range(0, VertexCount);

	public IEnumerable<Edge<T>> Edges => 
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
		Comparer = comparer;
	}
	
	public EdgeWeightedGraphWithAdjacencyLists(int vertexCount, IEnumerable<Edge<T>> edges, IComparer<T> comparer)
		: this(vertexCount, comparer)
	{
		foreach (var edge in edges)
		{
			AddEdge(edge);
		}
	}

	public EdgeWeightedGraphWithAdjacencyLists(EdgeWeightedGraphWithAdjacencyLists<T> graph)
		: this(graph.VertexCount, graph.Edges, graph.Comparer)
	{
	}

	public void AddEdge(Edge<T> edge)
	{
		adjacents[edge.Vertex0].Add(edge);

		if (edge.Vertex0 != edge.Vertex1)
		{
			adjacents[edge.Vertex1].Add(edge);
		}

		EdgeCount++;
	}
	
	public void AddEdge(int vertex0, int vertex1, T weight)
	{
		ValidateVertex(vertex0);
		ValidateVertex(vertex1);
		
		AddEdge(new Edge<T>(vertex0, vertex1, weight));
	}
	
	public void RemoveEdge(Edge<T> edge)
	{
		adjacents[edge.Vertex0].Remove(edge);

		if (edge.Vertex0 != edge.Vertex1)
		{
			adjacents[edge.Vertex1].Remove(edge);
		}

		EdgeCount--;
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

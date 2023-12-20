using static System.Diagnostics.Debug;

namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Collections;
using List;
using Support;

/// <summary>
/// Represents a weighted graph data structure.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class EdgeWeightedGraphWithAdjacencyLists<TWeight> 
	: IEdgeWeightedGraph<TWeight>
{
	private readonly ResizeableArray<Edge<TWeight>>[] adjacents;
	
	/// <inheritdoc />
	public IComparer<TWeight> Comparer { get;  }

	/// <inheritdoc />
	public IEnumerable<int> Vertexes => Enumerable.Range(0, VertexCount);

	/// <inheritdoc />
	public IEnumerable<Edge<TWeight>> Edges => 
		from vertex in Vertexes 
		from edge in GetIncidentEdges(vertex) 
		where vertex <= edge.OtherVertex(vertex)
		select edge;

	/// <inheritdoc />
	public int VertexCount { get; }

	/// <inheritdoc />
	public int EdgeCount { get; private set; }

	/// <inheritdoc />
	public bool SupportsParallelEdges => true;

	/// <inheritdoc />
	public bool SupportsSelfLoops => true;

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex)
		=> adjacents[vertex].Select(edge => edge.OtherVertex(vertex));

	public EdgeWeightedGraphWithAdjacencyLists(int vertexCount, IComparer<TWeight> comparer)
	{
		VertexCount = vertexCount;
		Comparer = comparer;
		adjacents = new ResizeableArray<Edge<TWeight>>[vertexCount];
		adjacents.Fill(() => new ResizeableArray<Edge<TWeight>>());
	}
	
	public EdgeWeightedGraphWithAdjacencyLists(int vertexCount, IEnumerable<Edge<TWeight>> edges, IComparer<TWeight> comparer)
		: this(vertexCount, comparer)
	{
		foreach (var edge in edges)
		{
			AddEdge(edge);
		}
	}

	public EdgeWeightedGraphWithAdjacencyLists(EdgeWeightedGraphWithAdjacencyLists<TWeight> graph)
		: this(graph.VertexCount, graph.Edges, graph.Comparer)
	{
	}

	/// <inheritdoc />
	public void AddEdge(Edge<TWeight> edge)
	{
		edge.ThrowIfNull();
		ValidateVertex(edge.Vertex0);
		ValidateVertex(edge.Vertex1);
		
		adjacents[edge.Vertex0].Add(edge);

		if (edge.Vertex0 != edge.Vertex1)
		{
			adjacents[edge.Vertex1].Add(edge);
		}

		EdgeCount++;
	}
	
	/// <inheritdoc />
	public bool RemoveEdge(Edge<TWeight> edge)
	{
		if (!ContainsEdge(edge.Vertex0, edge.Vertex1))
		{
			return false;
		}

		bool wasRemoved = adjacents[edge.Vertex0].Remove(edge);

		if (!wasRemoved)
		{
			return false;
		}
		
		if (edge.Vertex0 != edge.Vertex1)
		{
			bool wasAlsoRemoved = adjacents[edge.Vertex1].Remove(edge);
			Assert(wasAlsoRemoved);
		}

		EdgeCount--;

		return true;
	}
	
	/// <inheritdoc />
	public IEnumerable<Edge<TWeight>> GetIncidentEdges(int vertex)
	{
		ValidateVertex(vertex);

		return adjacents[vertex];
	}

	/// <inheritdoc />
	public bool ContainsEdge(int vertex0, int vertex1)
	{
		ValidateVertex(vertex0);
		ValidateVertex(vertex1);

		return adjacents[vertex0].Any(edge => edge.OtherVertex(vertex0) == vertex1);
	}
	
	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() 
		=> Edges.Select(edge => (edge.Vertex0, edge.Vertex1)).GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc />
	// 4.3.17
	public override string ToString() => Edges.AsText().Bracket();
	
	private void ValidateVertex(int vertex)
	{
		if (vertex < 0 || vertex >= VertexCount)
		{
			throw new ArgumentException("Invalid vertex.");
		}
	}
}

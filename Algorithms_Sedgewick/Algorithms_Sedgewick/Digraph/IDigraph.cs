using System.Runtime.CompilerServices;
using Support;

namespace Algorithms_Sedgewick.Digraphs;

public interface IDigraph
{
	/// <summary>
	/// Gets the number of vertices in the graph.
	/// </summary>
	public int VertexCount { get; }
	
	/// <summary>
	/// Gets the number of edges in the graph.
	/// </summary>
	public int EdgeCount { get; }
	
	/// <summary>
	/// Gets the vertices in the graph.
	/// </summary>
	public IEnumerable<int> Vertexes => Enumerable.Range(0, VertexCount); 
	
	/// <summary>
	/// Adds an edge between two vertices in the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to add.</param>
	/// <param name="vertex1">The second vertex of the edge to add.</param>
	void AddEdge(int vertex0, int vertex1);
	
	/// <summary>
	/// Removes an edge between two vertices in the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to remove.</param>
	/// <param name="vertex1">The second vertex of the edge to remove.</param>
	/// <returns><see langword="true"/> if the edge was removed, <see langword="false"/> otherwise.</returns>
	bool RemoveEdge(int vertex0, int vertex1);
	
	/// <summary>
	/// Gets the adjacent vertices of a given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to find adjacents for.</param>
	/// <returns>An enumerable of adjacent vertices.</returns>
	public IEnumerable<int> GetAdjacents(int vertex);
	
	/// <summary>
	/// Creates a human-readable string representation of the graph.
	/// </summary>
	/// <returns>A string representing the graph's structure.</returns>
	public string AsText()
	{
		string NeighborsAsText(int vertex) => GetAdjacents(vertex).AsText(Formatter.Space);
		string VertexAsText(int vertex) => vertex.Describe(NeighborsAsText(vertex));
		
		return nameof(VertexCount).Describe(VertexCount.AsText())
				+ nameof(EdgeCount).Describe(EdgeCount.AsText())
				+ Vertexes.Select(VertexAsText).AsText(Formatter.NewLine);
	}

	bool AreAdjacent(int item1, int item2) => GetAdjacents(item1).Contains(item2);
}

public static class DigraphValiditor
{
	public static void ValidateInRange(
		this IDigraph digraph,
		int vertex0,
		int vertex1,
		[CallerArgumentExpression(nameof(vertex0))]
		string? vertex0ArgName = null,
		[CallerArgumentExpression(nameof(vertex1))]
		string? vertex1ArgName = null)
	{
		digraph.ValidateInRange(vertex0, vertex0ArgName);
		digraph.ValidateInRange(vertex1, vertex1ArgName);
	}

	public static void ValidateInRange(this IDigraph digraph, int vertex, [CallerArgumentExpression(nameof(vertex))] string? vertexArgName = null)
	{
		if (vertex < 0 || vertex >= digraph.VertexCount)
		{
			throw new IndexOutOfRangeException($"Vertex argument {vertexArgName}={vertex} is not between 0 and {digraph.VertexCount - 1}");
		}
	}
}

using System.Runtime.CompilerServices;

namespace AlgorithmsSW.Digraph;

public interface IDigraph : IReadOnlyDigraph
{
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

using System.Runtime.CompilerServices;

namespace Algorithms_Sedgewick.Graphs;

public static class GraphExtensions
{
	// 4.1.4
	public static bool ContainsEdge(this IGraph graph, int vertex1, int vertex2)
		=> graph.GetAdjacents(vertex1).Contains(vertex2);

	public static int GetSelfLoopCount(this IGraph graph)
		=> graph.Vertices.Count(v => graph.ContainsEdge(v, v));

	public static int GetDegree(this IGraph graph, int vertex) 
		=> graph.GetAdjacents(vertex).Count();

	public static int MaxDegree(this IGraph graph)
		=> graph
			.Vertices
			.Select(graph.GetDegree)
			.Max();

	public static float AverageDegree(this IGraph graph)
		=> 2 * graph.EdgeCount / (float)graph.VertexCount;
	
	public static void Add(this IGraph graph, int vertex0, int vertex1) => graph.AddEdge(vertex0, vertex1);
}

public static class GraphValiditor
{
	public static void ValidateInRange(
		this IGraph graph,
		int vertex0,
		int vertex1,
		[CallerArgumentExpression(nameof(vertex0))] string? vertex0ArgName = null,
		[CallerArgumentExpression(nameof(vertex1))] string? vertex1ArgName = null)
	{
		graph.ValidateInRange(vertex0, vertex0ArgName);
		graph.ValidateInRange(vertex1, vertex1ArgName);
	}
	
	public static void ValidateInRange(this IGraph graph, int vertex, [CallerArgumentExpression(nameof(vertex))] string? vertexArgName = null)
	{
		if (vertex < 0 || vertex >= graph.VertexCount)
		{
			throw new IndexOutOfRangeException($"Vertex argument {vertexArgName}={vertex} is not between 0 and {graph.VertexCount - 1}");
		}
	}
	
	public static void ValidateNotSelfLoop(
		this IGraph graph,
		int vertex0, 
		int vertex1,
		[CallerArgumentExpression(nameof(vertex0))] string? vertex0ArgName = null,
		[CallerArgumentExpression(nameof(vertex1))] string? vertex1ArgName = null)
	{
		if (vertex0 == vertex1)
		{
			throw new ArgumentException($"Vertex arguments {vertex0ArgName}={vertex0} and {vertex1ArgName}={vertex1} are the same.");
		}
	}
	
	public static void ValidateNotParallelEdge(
		this IGraph graph, 
		int vertex0,
		int vertex1,
		[CallerArgumentExpression(nameof(vertex0))] string? vertex0ArgName = null,
		[CallerArgumentExpression(nameof(vertex1))] string? vertex1ArgName = null)
	{
		if (graph.ContainsEdge(vertex0, vertex1))
		{
			throw new ArgumentException($"Edge ({vertex0ArgName} = {vertex0}, {vertex1ArgName} = {vertex1}) already exists.");
		}
	}
	
	public static void ValidateContainsEdge(
		this IGraph graph, 
		int vertex0,
		int vertex1,
		[CallerArgumentExpression(nameof(vertex0))] string? vertex0ArgName = null,
		[CallerArgumentExpression(nameof(vertex1))] string? vertex1ArgName = null)
	{
		if (!graph.ContainsEdge(vertex0, vertex1))
		{
			throw new ArgumentException($"Edge ({vertex0ArgName} = {vertex0}, {vertex1ArgName} = {vertex1}) not found.");
		}
	}
}

using System.Runtime.CompilerServices;

namespace AlgorithmsSW.Graph;

using Digraph;

public static class GraphExtensions
{
	[ExerciseReference(4, 1, 4)]
	public static bool ContainsEdge(this IReadOnlyGraph graph, int vertex1, int vertex2)
		=> graph.GetAdjacents(vertex1).Contains(vertex2);

	public static int GetSelfLoopCount(this IReadOnlyGraph graph)
		=> graph.Vertexes.Count(v => graph.ContainsEdge(v, v));

	public static bool HasSelfLoops(this IReadOnlyGraph graph)
		=> graph.Vertexes.Any(v => graph.ContainsEdge(v, v));
	
	public static bool HasParallelEdges(this IReadOnlyGraph graph) 
		=> graph.SupportsParallelEdges 
			&& graph.GroupBy(e => e).Any(g => g.Count() > 1);

	public static int GetDegree(this IReadOnlyGraph graph, int vertex) 
		=> graph.GetAdjacents(vertex).Count();

	public static int MaxDegree(this IReadOnlyGraph graph)
		=> graph
			.Vertexes
			.Select(graph.GetDegree)
			.Max();

	public static float AverageDegree(this IReadOnlyGraph graph)
		=> 2 * graph.EdgeCount / (float)graph.VertexCount;
	
	public static void Add(this IGraph graph, int vertex0, int vertex1) => graph.AddEdge(vertex0, vertex1);
	
	/// <summary>
	/// Converts a graph to a digraph by two edges (in opposite directions) for each edge in the original graph.
	/// </summary>
	/// <param name="graph">The graph to convert.</param>
	public static IReadOnlyDigraph ToDigraph(this IReadOnlyGraph graph)
	{
		var digraph = DataStructures.Digraph(graph.VertexCount);

		foreach (var edge in graph)
		{
			digraph.AddEdge(edge.vertex0, edge.vertex1);
			digraph.AddEdge(edge.vertex1, edge.vertex0);
		}

		return digraph;
	}
}

public static class GraphValidator
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

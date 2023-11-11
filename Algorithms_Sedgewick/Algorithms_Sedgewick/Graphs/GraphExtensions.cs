namespace Algorithms_Sedgewick.Graphs;

public static class GraphExtensions
{
	// 4.1.4
	public static bool AreAdjacent(this IGraph graph, int vertex1, int vertex2)
		=> graph.GetAdjacents(vertex1).Contains(vertex2);

	public static int GetSelfLoopCount(this IGraph graph)
		=> graph.Vertices.Count(v => graph.AreAdjacent(v, v));

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

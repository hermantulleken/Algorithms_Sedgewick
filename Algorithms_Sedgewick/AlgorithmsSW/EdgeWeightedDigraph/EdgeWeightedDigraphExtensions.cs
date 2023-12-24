namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Formats.Tar;

public static class EdgeWeightedDigraphExtensions
{
	public static void AddEdge<TWeight>(this IEdgeWeightedDigraph<TWeight> graph, int sourceVertex, int targetVertex, TWeight weight)
	{
		graph.AddEdge(new(sourceVertex, targetVertex, weight));
	}
	
	public static DirectedEdge<TWeight> GetUniqueEdge<TWeight>(
		this IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex, 
		int targetVertex)
	{
		var edges = graph.GetIncidentEdges(sourceVertex).Where(edge => edge.Target == targetVertex);
		int count = edges.Count();
		
		switch (count)
		{
			case 0:
				throw new ArgumentException("Vertices not connected.");
			case > 1:
				throw new ArgumentException("Graph has parallel edges.");
		}
		
		return edges.First();
	}
	
	public static DirectedEdge<TWeight> RemoveUniqueEdge<TWeight>(this IEdgeWeightedDigraph<TWeight> digraph, int sourceVertex, int targetVertex)
	{
		var edge = digraph.GetUniqueEdge(sourceVertex, targetVertex);
		digraph.RemoveEdge(edge);
		return edge;
	}
}

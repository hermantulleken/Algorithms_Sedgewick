namespace AlgorithmsSW.EdgeWeightedGraph;

using EdgeWeightedDigraph;

/// <summary>
/// Provides extension methods for <see cref="IEdgeWeightedGraph{T}"/>.
/// </summary>
public static class EdgeWeightedGraphExtensions
{
	/// <summary>
	/// Makes a new <see cref="Edge{T}"/> and adds it to the graph. 
	/// </summary>
	/// <param name="graph">The graph to add the edge to.</param>
	/// <param name="vertex0">The first vertex of the edge to add.</param>
	/// <param name="vertex1">The second vertex of the edge to add.</param>
	/// <param name="weight">The weight of the edge to add.</param>
	public static Edge<TWeight> AddEdge<TWeight>(this IEdgeWeightedGraph<TWeight> graph, int vertex0, int vertex1, TWeight weight)
	{
		var edge = new Edge<TWeight>(vertex0, vertex1, weight);
		graph.AddEdge(edge);
		
		return edge;
	}
	
	public static Edge<T> GetUniqueEdge<T>(this IEdgeWeightedGraph<T> graph, int vertex0, int vertex1)
	{
		var edges = graph.GetIncidentEdges(vertex0).Where(edge => edge.OtherVertex(vertex0) == vertex1);
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
	
	/// <summary>
	/// Converts this <see cref="IEdgeWeightedGraph{T}"/> to an <see cref="IEdgeWeightedDigraph{T}"/> by adding two
	/// edges (in opposite directions) for each edge in the original graph.
	/// </summary>
	/// <param name="graph">The graph to convert.</param>
	/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
	public static IEdgeWeightedDigraph<TWeight> ToEdgeWeightedDigraph<TWeight>(this IEdgeWeightedGraph<TWeight> graph)
	{
		var digraph = DataStructures.EdgeWeightedDigraph(graph.VertexCount, graph.Comparer);
		
		foreach (var edge in graph.WeightedEdges)
		{
			var directedEdge = new DirectedEdge<TWeight>(edge.Vertex0, edge.Vertex1, edge.Weight);
			var oppositeDirectedEdge = new DirectedEdge<TWeight>(edge.Vertex1, edge.Vertex0, edge.Weight);
			
			digraph.AddEdge(directedEdge);
			digraph.AddEdge(oppositeDirectedEdge);
		}
		
		return digraph;
	}
}

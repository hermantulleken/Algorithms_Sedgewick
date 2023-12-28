namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;

public enum EdgeExistance
{
	DoesNotExist,
	Unique,
	NotUnqiue
}

public static class EdgeWeightedDigraphExtensions
{
	public static void AddEdge<TWeight>(this IEdgeWeightedDigraph<TWeight> graph, int sourceVertex, int targetVertex, TWeight weight)
	{
		graph.AddEdge(new(sourceVertex, targetVertex, weight));
	}

	public static EdgeExistance TryGetUniqueEdge<TWeight>(
		this IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex, 
		int targetVertex,
		out DirectedEdge<TWeight>? edge)
	{
		var edges = graph.GetIncidentEdges(sourceVertex).Where(edge => edge.Target == targetVertex);
		int count = edges.Count();
		
		switch (count)
		{
			case 0:
				edge = null;
				return EdgeExistance.DoesNotExist;
			case > 1:
				edge = null;
				return EdgeExistance.NotUnqiue;
		}
		
		edge = edges.First();
		
		return EdgeExistance.Unique;
	}
	
	public static bool HasUniqueEdge<TWeight>(this IReadOnlyEdgeWeightedDigraph<TWeight> graph, int pairFirst, int pairLast) 
		=> graph.TryGetUniqueEdge(pairFirst, pairLast, out _) == EdgeExistance.Unique;
	
	public static DirectedEdge<TWeight> GetUniqueEdge<TWeight>(
		this IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex,
		int targetVertex)
	{
		if (graph.TryGetUniqueEdge(sourceVertex, targetVertex, out var edge) != EdgeExistance.Unique)
		{
			throw new InvalidOperationException("The graph does not contain a unique edge between the two vertices.");
		}

		return edge;
	}
	
	public static DirectedEdge<TWeight> RemoveUniqueEdge<TWeight>(this IEdgeWeightedDigraph<TWeight> digraph, int sourceVertex, int targetVertex)
	{
		var edge = digraph.GetUniqueEdge(sourceVertex, targetVertex);
		digraph.RemoveEdge(edge);
		return edge;
	}


	public static IEnumerable<DirectedEdge<TWeight>> RemoveVertex<TWeight>(this IEdgeWeightedDigraph<TWeight> graph, int vertex)
	{
		var edgesToRemove = graph
			.GetIncidentEdges(vertex)
			.Concat(graph.WeightedEdges.Where(edge => edge.Target == vertex))
			.ToResizableArray();

		foreach (var edge in edgesToRemove)
		{
			graph.RemoveEdge(edge);
		}

		return edgesToRemove;
	}

	// 4.4.22
	public static IEdgeWeightedDigraph<TWeight> ToEdgeWeightedDigraph<TWeight>(
		this IReadOnlyDigraph graph, 
		TWeight[] vertexWeights, 
		IComparer<TWeight> comparer,
		TWeight zero)
	{
		/*	Makes an edge for each vertex with the vertex weight as its weight
			Makes an edge for each edge in the original going from end of the corresponding source vertex edge to the
			begging of the corresponding target vertex edge
		*/

		var newGraph = DataStructures.EdgeWeightedDigraph(graph.VertexCount * 2, comparer);
		foreach (int vertex in graph.Vertexes)
		{
			newGraph.AddEdge(new(StartVertex(vertex), EndVertex(vertex), vertexWeights[vertex]));

			foreach (int otherVertex in graph.Vertexes)
			{
				if (graph.AreAdjacent(vertex, otherVertex))
				{
					newGraph.AddEdge(EndVertex(vertex), StartVertex(otherVertex), zero);
				}
			}
		}

		return newGraph;
		
		int StartVertex(int vertex) => vertex * 2;
		int EndVertex(int vertex) => vertex * 2 + 1;
	}
	
	public static IEdgeWeightedDigraph<TWeight> Reverse<TWeight>(this IEdgeWeightedDigraph<TWeight> graph)
	{
		graph.ThrowIfNull();

		var reversedGraph = DataStructures.EdgeWeightedDigraph(graph.VertexCount, graph.Comparer);

		foreach (var edge in graph.WeightedEdges)
		{
			reversedGraph.AddEdge(edge.Target, edge.Source, edge.Weight);
		}
		
		return reversedGraph;
	}
}

namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Linq;
using Digraph;

public static class RandomGraph
{
	public static IEdgeWeightedDigraph<TWeight> AddRandomWeights<TWeight>(
		this IDigraph digraph, 
		IEnumerable<TWeight> generator) // TODO: introduce proper generators
	{
		var newGraph = DataStructures.EdgeWeightedDigraph<TWeight>(digraph.VertexCount);

		foreach (var (edge, weight) in digraph.Edges.Zip(generator))
		{
			newGraph.AddEdge(edge.source, edge.target, weight);
		}

		return newGraph;
	}
	
	public static IEdgeWeightedDigraph<TWeight> AddRandomWeights<TWeight>(
		Func<int, int, IDigraph> graphFactory, 
		int vertexCount,
		int edgeCount,
		IEnumerable<TWeight> generator)
		=> graphFactory(vertexCount, edgeCount).AddRandomWeights(generator);
	
	public static IEdgeWeightedDigraph<TWeight> ErdosRenyiGraph<TWeight>(
		int vertexCount, 
		int edgeCount,
		IEnumerable<TWeight> generator) 
		=> AddRandomWeights(Digraph.RandomGraph.ErdosRenyiGraph, vertexCount, edgeCount, generator);
	
	public static IEdgeWeightedDigraph<TWeight> SimpleGraph<TWeight>(
		int vertexCount, 
		int edgeCount,
		IEnumerable<TWeight> generator) 
		=> AddRandomWeights(Digraph.RandomGraph.RandomSimple, vertexCount, edgeCount, generator);
	
	public static IEdgeWeightedDigraph<TWeight> AssignWeights<TWeight>(
		IReadOnlyDigraph graph, 
		IEnumerable<TWeight> weights)
	{
		var newGraph = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(graph.VertexCount);
		
		foreach (((int vertex0, int vertex1), var weight) in graph.Edges.Zip(weights))
		{
			newGraph.AddEdge(vertex0, vertex1, weight);
		}

		return newGraph;
	}
}

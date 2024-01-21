namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;
using System.Linq;

public static class RandomGraph
{
	public static IEdgeWeightedDigraph<TWeight> AddRandomWeights<TWeight>(
		this IDigraph digraph, 
		IComparer<TWeight> comparer,
		IEnumerable<TWeight> generator) // TODO: introduce proper generators
	{
		var newGraph = DataStructures.EdgeWeightedDigraph(digraph.VertexCount, comparer);

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
		IComparer<TWeight> comparer,
		IEnumerable<TWeight> generator)
		=> graphFactory(vertexCount, edgeCount).AddRandomWeights(comparer, generator);
	
	public static IEdgeWeightedDigraph<TWeight> ErdosRenyiGraph<TWeight>(
		int vertexCount, 
		int edgeCount,
		IComparer<TWeight> comparer,
		IEnumerable<TWeight> generator) 
		=> AddRandomWeights(Digraph.RandomGraph.ErdosRenyiGraph, vertexCount, edgeCount, comparer, generator);
	
	public static IEdgeWeightedDigraph<TWeight> SimpleGraph<TWeight>(
		int vertexCount, 
		int edgeCount,
		IComparer<TWeight> comparer,
		IEnumerable<TWeight> generator) 
		=> AddRandomWeights(Digraph.RandomGraph.RandomSimple, vertexCount, edgeCount, comparer, generator);
	
	public static IEdgeWeightedDigraph<TWeight> AssignWeights<TWeight>(
		IReadOnlyDigraph graph, 
		IEnumerable<TWeight> weights, 
		IComparer<TWeight> comparer)
	{
		var newGraph = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(graph.VertexCount, comparer);
		
		foreach (((int vertex0, int vertex1), var weight) in graph.Edges.Zip(weights))
		{
			newGraph.AddEdge(vertex0, vertex1, weight);
		}

		return newGraph;
	}
}

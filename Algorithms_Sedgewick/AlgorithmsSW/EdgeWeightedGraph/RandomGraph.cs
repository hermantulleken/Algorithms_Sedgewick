using AlgorithmsSW.Graph;

namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// Provides methods for generating random graphs.
/// </summary>
public static class RandomGraph
{
	/// <summary>
	/// Assigns weights from a list to edges of a graph.
	/// </summary>
	/// <param name="graph">The graph to assign weights to.</param>
	/// <param name="weights">The weights to assign to the edges of the graph.</param>
	/// <typeparam name="TWeight">The type of the weights.</typeparam>
	/// <example>
	/// You can use a <see cref="IEnumerable{T}"/> such as the following to assign random weights:
	/// [!code-csharp[](../../AlgorithmsSW/Generator.cs#GeneratorExample)]
	/// </example>
	/// <see cref="AlgorithmsSW.Graph.RandomGraph"/>
	public static IEdgeWeightedGraph<TWeight> AssignWeights<TWeight>(
		IReadOnlyGraph graph, 
		IEnumerable<TWeight> weights)
	{
		var newGraph = new EdgeWeightedGraphWithAdjacencyLists<TWeight>(graph.VertexCount);
		
		foreach (((int vertex0, int vertex1), var weight) in graph.Zip(weights))
		{
			newGraph.AddEdge(vertex0, vertex1, weight);
		}

		return newGraph;
	}
}

using System.Collections;

namespace AlgorithmsSW.Graphs;

public class GraphWtihNoSelfLoops(Func<IGraph> graphFactory) : IGraph
{
	private readonly IGraph graph = graphFactory();

	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() => graph.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)graph).GetEnumerator();

	/// <inheritdoc />
	public int VertexCount => graph.VertexCount;

	/// <inheritdoc />
	public int EdgeCount => graph.EdgeCount;

	bool IGraph.SupportsParallelEdges => graph.SupportsParallelEdges;

	bool IGraph.SupportsSelfLoops => false;

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		graph.ValidateNotSelfLoop(vertex0, vertex1);
		graph.AddEdge(vertex0, vertex1);
	}
	
	/// <inheritdoc />
	public bool RemoveEdge(int vertex0, int vertex1) => graph.RemoveEdge(vertex0, vertex1);

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex) => graph.GetAdjacents(vertex);
}

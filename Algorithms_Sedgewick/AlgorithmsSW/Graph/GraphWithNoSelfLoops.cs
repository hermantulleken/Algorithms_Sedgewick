using System.Collections;

namespace AlgorithmsSW.Graph;

public class GraphWithNoSelfLoops(Func<IGraph> graphFactory) : IGraph
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

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsParallelEdges => graph.SupportsParallelEdges;

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsSelfLoops => false;

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

	public bool ContainsEdge(int vertex0, int vertex1) => graph.ContainsEdge(vertex0, vertex1);
}

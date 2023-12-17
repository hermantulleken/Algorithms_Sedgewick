using System.Collections;

namespace AlgorithmsSW.Graph;

/// <summary>
/// Represents a graph with a fixed number of vertices and edges, where adjacency is represented by a boolean array.
/// </summary>
public class GraphWithAdjacentsBoolArray(int vertexCount) : IGraph
{
	private readonly bool[,] adjacents = new bool[vertexCount, vertexCount];

	/// <inheritdoc />
	public int VertexCount { get; } = vertexCount;

	/// <inheritdoc />
	public int EdgeCount { get; private set; } = 0;

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		this.ValidateNotParallelEdge(vertex0, vertex1);
		
		adjacents[vertex0, vertex1] = true;
		adjacents[vertex1, vertex0] = true; // No harm setting twice if the vertices are equal
		
		EdgeCount++;
	}
	
	public bool RemoveEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);

		if (!ContainsEdge(vertex0, vertex1))
		{
			return false;
		}
		
		adjacents[vertex0, vertex1] = false;
		adjacents[vertex1, vertex0] = false; // No harm setting twice if the vertices are equal
		EdgeCount--;

		return true;
	}

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			if (adjacents[vertex, i])
			{
				yield return i;
			}
		}
	}

	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator()
	{
		for (int vertex1 = 0; vertex1 < VertexCount; vertex1++)
		{
			for (int vertex0 = 0; vertex0 <= vertex1; vertex0++)
			{
				if (adjacents[vertex1, vertex0])
				{
					yield return (vertex0, vertex1);
				}
			}
		}
	}
	
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0, vertex1];

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsParallelEdges => false;

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsSelfLoops => true;
}

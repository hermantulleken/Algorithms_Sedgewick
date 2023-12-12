using System.Collections;

namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// Represents a graph with a fixed number of vertices and edges, where adjacency is represented by a boolean array.
/// </summary>
public class GraphWithAdjacentsIntArray(int vertexCount) : IGraph
{
	private readonly int[,] adjacents = new int[vertexCount, vertexCount];

	/// <inheritdoc />
	public int VertexCount { get; } = vertexCount;

	/// <inheritdoc />
	public int EdgeCount { get; private set; } = 0;

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		adjacents[vertex0, vertex1]++;

		if (vertex0 != vertex1)
		{
			adjacents[vertex1, vertex0]++;
		}
		
		EdgeCount++;
	}

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			for (int j = 0; j < adjacents[vertex, i]; j++)
			{
				yield return i;
			}
		}
	}

	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex2)> GetEnumerator()
	{
		for (int vertex1 = 0; vertex1 < VertexCount; vertex1++)
		{
			for (int vertex0 = 0; vertex0 <= vertex1; vertex0++)
			{
				var edge = (vertex0, vertex1);
				
				for (int k = 0; k < adjacents[vertex1, vertex0]; k++)
				{
					yield return edge;
				}
			}
		}
	}
	
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0, vertex1] > 0;

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
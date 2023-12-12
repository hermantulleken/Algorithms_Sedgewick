using System.Collections;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Graphs;

public class GraphWithAdjacentsArrays : IGraph
{
	private readonly ResizeableArray<int>[] adjacents;
	
	public int VertexCount { get; }
	
	public int EdgeCount { get; private set; }

	// 4.1.3
	public GraphWithAdjacentsArrays(GraphWithAdjacentsArrays graph)
		: this(graph.VertexCount)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			foreach (int vertex in graph.adjacents[i])
			{
				adjacents[i].Add(vertex);
			}
		}
	}

	public GraphWithAdjacentsArrays(int vertexCount)
	{
		VertexCount = vertexCount;

		adjacents = new ResizeableArray<int>[vertexCount];
		
		for (int i = 0; i < adjacents.Length; i++)
		{
			// This initial capacity a good first guess, but since we allow duplicates the lists could expand.
			adjacents[i] = new ResizeableArray<int>(vertexCount);
		}

		EdgeCount = 0;
	}

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		adjacents[vertex0].Add(vertex1);
		adjacents[vertex1].Add(vertex0);
		EdgeCount++;
	}

	public IEnumerable<int> GetAdjacents(int vertex) => adjacents[vertex];

	public IEnumerator<(int vertex0, int vertex2)> GetEnumerator()
	{
		for (int vertex0 = 0; vertex0 < VertexCount; vertex0++)
		{
			foreach (int vertex1 in adjacents[vertex0])
			{
				if (vertex0 <= vertex1)
				{
					yield return (vertex0, vertex1);
				}
			}
		}
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

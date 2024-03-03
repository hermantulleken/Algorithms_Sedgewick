using System.Collections;
using AlgorithmsSW.List;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Graph;

public class GraphWithAdjacentsLists : IGraph
{
	private readonly ResizeableArray<int>[] adjacents;
	
	public int VertexCount { get; }
	
	public int EdgeCount { get; private set; }

	[ExerciseReference(4, 1, 3)]
	public GraphWithAdjacentsLists(GraphWithAdjacentsLists graph)
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

	public GraphWithAdjacentsLists(int vertexCount)
	{
		VertexCount = vertexCount;
		EdgeCount = 0;
		adjacents = new ResizeableArray<int>[vertexCount];
		
		for (int i = 0; i < adjacents.Length; i++)
		{
			// This initial capacity a good first guess, but since we allow duplicates the lists could expand.
			adjacents[i] = new ResizeableArray<int>(vertexCount);
		}
	}

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		adjacents[vertex0].Add(vertex1);

		if (vertex0 != vertex1)
		{
			adjacents[vertex1].Add(vertex0);
		}
		
		EdgeCount++;
	}
	
	public bool RemoveEdge(int vertex0, int vertex1)
	{
		void Remove(int v0, int v1)
		{
			int indexToRemove = adjacents[v0].IndexWhere(vertex => vertex == v1).First();
			adjacents[v0].RemoveAt(indexToRemove);
		}
		
		this.ValidateInRange(vertex0, vertex1);

		if (!ContainsEdge(vertex0, vertex1))
		{
			return false;
		}

		bool removed = adjacents[vertex0].Remove(vertex1);
		Assert(removed);
		
		if (vertex0 != vertex1)
		{
			removed = adjacents[vertex1].Remove(vertex0);
			Assert(removed);
		}
		
		EdgeCount--;

		return true;
	}

	public IEnumerable<int> GetAdjacents(int vertex) => adjacents[vertex];

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator()
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
	
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0].Contains(vertex1);
	
	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsParallelEdges => true;

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsSelfLoops => true;
}

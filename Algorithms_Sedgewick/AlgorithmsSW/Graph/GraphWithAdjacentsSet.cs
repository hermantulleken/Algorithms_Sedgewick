using System.Collections;

namespace AlgorithmsSW.Graph;

[ExerciseReference(4, 1, 5)]
public class GraphWithAdjacentsSet : IGraph
{
	private readonly Set.ISet<int>[] adjacents;

	/// <inheritdoc />
	public int VertexCount { get; }

	/// <inheritdoc />
	public int EdgeCount { get; private set; }

	[ExerciseReference(4, 1, 3)]
	public GraphWithAdjacentsSet(GraphWithAdjacentsSet graph)
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

	public GraphWithAdjacentsSet(int vertexCount)	
		: this(vertexCount, () => new Set.HashSet<int>(vertexCount, Comparer<int>.Default))
	{
	}
	
	public GraphWithAdjacentsSet(int vertexCount, Func<Set.ISet<int>> setFactory)
	{
		VertexCount = vertexCount;

		adjacents = new Set.ISet<int>[vertexCount];
		
		for (int i = 0; i < adjacents.Length; i++)
		{
			adjacents[i] = setFactory();
		}

		EdgeCount = 0;
	}

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsParallelEdges => false;

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsSelfLoops => true;

	/// <inheritdoc />
	/// <remarks>Does not support parallel edges.</remarks>
	/// <exception cref="ArgumentException">the edge you are trying to add is parallel to an existing edge.</exception>
	public void AddEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		this.ValidateNotParallelEdge(vertex0, vertex1);
		
		adjacents[vertex0].Add(vertex1);

		if (vertex0 != vertex1)
		{
			adjacents[vertex1].Add(vertex0);
		}
		
		EdgeCount++;
	}
	
	public bool RemoveEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		
		bool removed = adjacents[vertex0].Remove(vertex1);

		if (removed)
		{
			EdgeCount--;
		}

		return removed;
	}
	
	/// <inheritdoc />
	// TODO: Verify that when we do lookups on this we do constant time lookups if we use a proper se
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

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0].Contains(vertex1);
}

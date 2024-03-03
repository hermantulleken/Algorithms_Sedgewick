using System.Collections;
using AlgorithmsSW.Counter;

namespace AlgorithmsSW.Graph;

public class GraphWithAdjacentsCounters : IGraph
{
	private readonly Counter<int>[] adjacents;

	/// <inheritdoc />
	public int VertexCount { get; }

	/// <inheritdoc />
	public int EdgeCount { get; private set; }

	public GraphWithAdjacentsCounters(int vertexCount)	
	{
		VertexCount = vertexCount;

		adjacents = new Counter<int>[vertexCount];
		
		for (int i = 0; i < adjacents.Length; i++)
		{
			adjacents[i] = new Counter<int>(Comparer<int>.Default);
		}

		EdgeCount = 0;
	}

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsParallelEdges => true;

	/// <inheritdoc/>
	bool IReadOnlyGraph.SupportsSelfLoops => true;

	/// <inheritdoc />
	/// <remarks>Does not support parallel edges.</remarks>
	/// <exception cref="ArgumentException">the edge you are trying to add is parallel to an existing edge.</exception>
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
		this.ValidateInRange(vertex0, vertex1);

		if (!ContainsEdge(vertex0, vertex1))
		{
			return false;
		}
		
		adjacents[vertex0].Remove(vertex1);
		EdgeCount--;

		return true;
	}
	
	/// <inheritdoc />
	// TODO: Verify that when we do lookups on this we do constant time lookups if we use a proper se
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		var counter = adjacents[vertex];

		foreach (int adjacent in counter.Keys)
		{
			for (int i = 0; i < counter[adjacent]; i++)
			{
				yield return adjacent;
			}
		}
	}

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator()
	{
		for (int vertex0 = 0; vertex0 < VertexCount; vertex0++)
		{
			var counter = adjacents[vertex0];
			
			foreach (int vertex1 in counter.Keys)
			{
				if (vertex0 > vertex1)
				{
					continue;
				}
				
				for (int i = 0; i < counter[vertex1]; i++)
				{
					yield return (vertex0, vertex1);
				}
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0][vertex1] > 0;
}

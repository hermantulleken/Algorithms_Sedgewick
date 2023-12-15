using System.Collections;
using AlgorithmsSW.Graphs;
using AlgorithmsSW.List;

namespace AlgorithmsSW.Digraphs;

/// <summary>
/// Represents a directed graph using an array of adjacency lists.
/// </summary>
public sealed class DigraphWithAdjacentsLists : IDigraph, IEnumerable<(int vertex0, int vertex1)>
{
	private readonly ResizeableArray<int>[] adjacents;

	/// <inheritdoc />
	public int VertexCount { get; }
	
	/// <inheritdoc />
	public int EdgeCount { get; private set; }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DigraphWithAdjacentsLists"/> class.
	/// </summary>
	/// <param name="vertexCount">The number of vertices in the graph.</param>
	public DigraphWithAdjacentsLists(int vertexCount)
	{
		VertexCount = vertexCount;
		EdgeCount = 0;
		adjacents = new ResizeableArray<int>[vertexCount];
		
		for (int i = 0; i < vertexCount; i++)
		{
			// This initial capacity a good first guess, but since we allow duplicates the lists could expand.
			adjacents[i] = new ResizeableArray<int>(vertexCount);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DigraphWithAdjacentsLists"/> class.
	/// </summary>
	/// <param name="vertexCount">The number of vertices in the graph.</param>
	/// <param name="edges">The edges to add to the graph.</param>
	public DigraphWithAdjacentsLists(int vertexCount, EdgeList edges) 
		: this(vertexCount)
	{
		foreach (var edge in edges)
		{
			Add(edge);
		}
	}

	// 4.2.3
	public DigraphWithAdjacentsLists(IDigraph digraph)
	{
		VertexCount = digraph.VertexCount;
		EdgeCount = digraph.EdgeCount;
		adjacents = new ResizeableArray<int>[digraph.VertexCount];

		foreach (int vertex in digraph.Vertexes)
		{
			var vertexAdjacents = digraph.GetAdjacents(vertex);

			foreach (int adjacent in vertexAdjacents)
			{
				digraph.AddEdge(vertex, adjacent);
			}
		}
	}	
	
	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		adjacents[vertex0].Add(vertex1);
		EdgeCount++;
	}
	
	public void Add((int vertex0, int vertex1) edge) => AddEdge(edge.vertex0, edge.vertex1);
	
	/// <inheritdoc />
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
		
		Remove(vertex0, vertex1);
		EdgeCount--;
		return true;
	}
	
	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex) => adjacents[vertex];
	
	/// <summary>
	/// Checks if the graph contains an edge between from one vertex to another.
	/// </summary>
	/// <param name="vertex0">The start of the edge.</param>
	/// <param name="vertex1">The end of the edge.</param>
	/// <returns><see langword="true"/> if the graph contains the edge, <see langword="false"/> otherwise.</returns>4
	// 4.2.4
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0].Contains(vertex1);

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator()
	{
		for (int i = 0; i < VertexCount; i++)
		{
			var list = adjacents[0];

			if (!list.Any())
			{
				continue;
			}

			foreach (int j in list)
			{
				yield return (i, j);
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// Represents a directed graph using an array of adjacency lists.
/// </summary>
public class DigraphWithAdjacentsLists : IDigraph
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
	
	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		this.ValidateInRange(vertex0, vertex1);
		adjacents[vertex0].Add(vertex1);
		EdgeCount++;
	}
	
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
	/// <returns><see langword="true"/> if the graph contains the edge, <see langword="false"/> otherwise.</returns>
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0].Contains(vertex1);
}

public class DirectedDepthFirstSearch
{
	private readonly bool[] reached;
	
	public DirectedDepthFirstSearch(IDigraph graph, int startVertex)
	{
		reached = new bool[graph.VertexCount];
		Search(graph, startVertex);
	}

	public DirectedDepthFirstSearch(IDigraph graph, IEnumerable<int> startVertexes)
	{
		reached = new bool[graph.VertexCount];
		
		foreach (int s in startVertexes)
		{
			if (!reached[s])
			{
				Search(graph, s);
			}
		}
	}
	
	public void Search(IDigraph graph, int vertex)
	{
		reached[vertex] = true;
		
		foreach (int adjacent in graph.GetAdjacents(vertex))
		{
			if (!reached[adjacent])
			{
				Search(graph, adjacent);
			}
		}
	}
	
	public bool Reachable(int vertex) => reached[vertex];
}

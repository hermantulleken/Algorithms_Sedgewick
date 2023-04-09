namespace Algorithms_Sedgewick.Graphs;

// 4.1.5
public class GraphWithAdjacentsSet : IGraph
{
	private readonly Set.ISet<int>[] adjacents;
	
	public int VertexCount { get; }
	
	public int EdgeCount { get; private set; }

	// 4.1.3
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
	
	public void AddEdge(int vertex0, int vertex1)
	{
		if (this.AreAdjacent(vertex0, vertex1))
		{
			ThrowHelper.ThrowInvalidOperationException("Vertices are already adjacent.");
		}

		if (vertex0 == vertex1)
		{
			ThrowHelper.ThrowInvalidOperationException("Self loops are not supported.");
		}
		
		adjacents[vertex0].Add(vertex1);
		adjacents[vertex1].Add(vertex0);
		EdgeCount++;
	}

	// TODO: Verify that when we do lookups on this we do constant time lookups if we use a proper set
	public IEnumerable<int> GetAdjacents(int vertex) => adjacents[vertex];
}

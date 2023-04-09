namespace Algorithms_Sedgewick.Graphs;

public interface IGraph
{
	public int VertexCount { get; }
	
	public int EdgeCount { get; }

	public bool IsEmpty => VertexCount == 0;

	IEnumerable<int> Vertices => Enumerable.Range(0, VertexCount); 
	
	public void AddEdge(int vertex0, int vertex1);

	IEnumerable<int> GetAdjacents(int vertex);
	
	public string Pretty()
	{
		string @string = VertexCount + " vertices, " + EdgeCount + " edges\n";
		
		foreach (int vertex in Vertices)
		{
			@string += vertex 
						+ ": " 
						+ string.Join(" ", GetAdjacents(vertex))
						+ "\n";
		}

		return @string;
	}
}

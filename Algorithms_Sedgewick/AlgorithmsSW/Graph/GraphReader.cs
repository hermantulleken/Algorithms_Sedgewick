using System.IO;

namespace AlgorithmsSW.Graph;

public class GraphReader
{
	/// <summary>
	/// Reads in a graph from a text file. 
	/// </summary>
	/// <remarks>
	/// The first item in the file is the number of vertices, the second item is the number of edges,
	/// and each subsequent pair of items represents an edge. Any whitespace can separate items.
	/// </remarks>
	public void ReadGraph<T>(TextReader reader, IComparer<T> comparer)
	{
		int vertexCount = reader.ReadWordAndConvert<int>();
		int edgeCount = reader.ReadWordAndConvert<int>();
		var graph = new GraphWithAdjacentsLists(vertexCount);
		var indexer = new Indexer<T>(comparer);
		
		for (int i = 0; i < edgeCount; i++)
		{
			var symbol0 = reader.ReadWordAndConvert<T>();
			var symbol1 = reader.ReadWordAndConvert<T>();
			int vertex0 = indexer.GetIndex(symbol0);
			int vertex1 = indexer.GetIndex(symbol1);
			
			graph.AddEdge(vertex0, vertex1);
		}
	}
}

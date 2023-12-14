// ReSharper disable IdentifierTypo
namespace Algorithms_Sedgewick.Digraphs;

/// <summary>
/// Provides an algorithm for finding the strongly connected components of a directed graph.
/// </summary>
public class Kosaraju
{
	private readonly bool[] marked;
	private readonly int[] id;

	/// <summary>
	/// Gets the number of strongly connected components in the graph.
	/// </summary>
	public int ConnectedComponentCount { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Kosaraju"/> class.
	/// </summary>
	/// <param name="digraph">The graph to find the strongly connected components of.</param>
	public Kosaraju(IDigraph digraph)
	{
		marked = new bool[digraph.VertexCount];
		id = new int[digraph.VertexCount];
		
		var order = new DepthFirstOrder(digraph.Reverse());
		
		foreach (int vertex in order.ReversePostOrder)
		{
			if (marked[vertex])
			{
				continue;
			}
			
			Search(digraph, vertex);
			ConnectedComponentCount++;
		}
	}
	
	/// <summary>
	/// Checks whether two vertices are strongly connected.
	/// </summary>
	public bool StronglyConnected(int vertex1, int vertex2) => id[vertex1] == id[vertex2];

	/// <summary>
	/// Gets the connected component ID of a vertex.
	/// </summary>
	/// <param name="vertex">The vertex to get the connected component ID of.</param>
	public int GetConnectedComponentId(int vertex) => id[vertex];
	
	private void Search(IDigraph digraph, int vertex)
	{
		marked[vertex] = true;
		id[vertex] = ConnectedComponentCount;
		
		foreach (int adjacent in digraph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				Search(digraph, adjacent);
			}
		}
	}
}

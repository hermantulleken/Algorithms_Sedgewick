namespace AlgorithmsSW.Digraphs;

// Note: Kosaraju is a more efficient algorithm for finding strong components

/// <summary>
/// An algorithm that finds the strong components of a directed graph.
/// </summary>
// 4.2.23
public class StrongComponents
{
	private readonly IDigraph reversedGraph;
	private readonly int[] componentIds;
	
	/// <summary>
	/// Gets the component id of a given vertex.
	/// </summary>
	public int ComponentCount { get; private set; }	
	
	/// <summary>
	/// Gets a value indicating whether the graph is strongly connected.
	/// </summary>
	public bool StronglyConnected => ComponentCount == 1;

	/// <summary>
	/// Initializes a new instance of the <see cref="StrongComponents"/> class.
	/// </summary>
	/// <param name="digraph">The graph to find the strong components of.</param>
	public StrongComponents(IDigraph digraph)
	{
		reversedGraph = digraph.Reverse();
		componentIds = new int[digraph.VertexCount];
		Array.Fill(componentIds, -1);

		int componentId = 0;
		
		for (int i = 0; i < digraph.VertexCount; i++)
		{
			if (componentIds[i] == -1)
			{
				FindStringConnectedComponent(componentId, digraph, i);
				componentId++;
			}
		}

		ComponentCount = componentId;
	}
	
	/// <summary>
	/// Gets the component ID of a given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to get the component ID of.</param>
	public int GetComponentId(int vertex) => componentIds[vertex];
	
	/// <summary>
	/// Gets a value indicating whether two vertices are strongly connected.
	/// </summary>
	/// <returns><see langword="true"/> if the vertices are strongly connected; otherwise, <see langword="false"/>.</returns>
	public bool IsStronglyConnected(int vertex1, int vertex2) => componentIds[vertex1] == componentIds[vertex2];

	/// <summary>
	/// Gets the vertices in a given strongly connected component.
	/// </summary>
	/// <param name="componentId">The component ID to get the vertices of.</param>
	public IEnumerable<int> GetStronglyConnectedComponent(int componentId)
	{
		for (int vertex = 0; vertex < componentIds.Length; vertex++)
		{
			if (componentIds[vertex] == componentId)
			{
				yield return vertex;
			}
		}
	}

	private void FindStringConnectedComponent(int componentId, IDigraph digraph, int source)
	{
		var connected = new DirectedDepthFirstSearch(digraph, source);
		var reverseConnected = new DirectedDepthFirstSearch(reversedGraph, source);

		foreach (int vertex in digraph.Vertexes)
		{
			if (connected.Reachable(vertex) && reverseConnected.Reachable(vertex))
			{
				componentIds[vertex] = componentId;
			}
		}
	}
}

namespace Algorithms_Sedgewick.Graphs;

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
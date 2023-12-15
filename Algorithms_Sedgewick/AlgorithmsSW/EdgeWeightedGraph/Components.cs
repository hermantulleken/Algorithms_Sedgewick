namespace AlgorithmsSW.EdgeWeightedGraph;

public class Components<T>
{
	private bool[] marked;
	private int[] componentIds;
	
	public Components(EdgeWeightedGraphWithAdjacencyLists<T> graph, IComparer<T> comparer)
	{
		int componentId = 0;
		foreach (var vertex in graph.Vertexes)
		{
			if (!marked[vertex])
			{
				Search(graph, vertex, componentId);
				componentId++;
			}
		}
	}

	private void Search(EdgeWeightedGraphWithAdjacencyLists<T> graph, int vertex, int componentId)
	{
		marked[vertex] = true;
		componentIds[vertex] = componentId;
		
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int otherVertex = edge.OtherVertex(vertex);
			
			if (!marked[otherVertex])
			{
				Search(graph, otherVertex, componentId);
			}
		}
	}

	public int GetComponentId(int vertex) => componentIds[vertex];
}

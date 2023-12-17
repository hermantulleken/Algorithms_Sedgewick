namespace AlgorithmsSW.EdgeWeightedGraph;

public class Components<T>
{
	private bool[] marked;
	private int[] componentIds;
	
	public Components(EdgeWeightedGraphWithAdjacencyLists<T> graph)
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

		ComponentCount = componentId;
	}

	public int ComponentCount { get;  }

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
	
	public bool AreConnected(int vertex0, int vertex1) => GetComponentId(vertex0) == GetComponentId(vertex1);
}

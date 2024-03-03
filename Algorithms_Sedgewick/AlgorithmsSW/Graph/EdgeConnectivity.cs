namespace AlgorithmsSW.Graph;

[ExerciseReference(4, 1, 36)]
public class EdgeConnectivity
{
	public IEnumerable<(int vertex0, int vertex)> Bridges { get; private init; }

	private bool IsEdgeConnected => !Bridges.Any();

	public static EdgeConnectivity Build(IGraph graph) 
		=> new()
		{
			Bridges = graph.Where(edge => IsBridge(graph, edge)),
		};

	private static bool IsBridge(IGraph graph, (int vertex0, int vertex1) edge)
	{
		graph.RemoveEdge(edge.vertex0, edge.vertex1);
		var connectivity = new Connectivity(graph);
		bool bridge = !connectivity.IsConnected;
		graph.Add(edge.vertex0, edge.vertex1);

		return bridge;
	}
}

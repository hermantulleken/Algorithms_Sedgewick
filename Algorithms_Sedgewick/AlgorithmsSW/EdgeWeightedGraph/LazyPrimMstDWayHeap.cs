namespace AlgorithmsSW.EdgeWeightedGraph;

public class LazyPrimMSTDWayHeap<TWeight>
{
	private bool[] marked;
	private Queue<Edge<TWeight>> minimumSpanningTree;
	private PriorityQueue.FixedCapacityMinNHeap<Edge<TWeight>> priorityQueue;
	private double weight;

	public LazyPrimMSTDWayHeap(IEdgeWeightedGraph<TWeight> graph, int heapDegree)
	{
		priorityQueue = new(heapDegree, graph.EdgeCount, new EdgeComparer<TWeight>(graph.Comparer));
		marked = new bool[graph.VertexCount];
		minimumSpanningTree = new();

		Visit(graph, 0); // Assumes graph is connected

		while (!priorityQueue.IsEmpty)
		{
			Edge<TWeight> edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;

			if (marked[vertex0] && marked[vertex1]) continue;

			minimumSpanningTree.Enqueue(edge);

			if (!marked[vertex0]) Visit(graph, vertex0);
			if (!marked[vertex1]) Visit(graph, vertex1);
		}
	}

	private void Visit(IEdgeWeightedGraph<TWeight> graph, int vertex)
	{
		marked[vertex] = true;
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			if (!marked[edge.OtherVertex(vertex)]) priorityQueue.Push(edge);
		}
	}

	public IEnumerable<Edge<TWeight>> Edges => minimumSpanningTree;

	public double Weight => weight;
}
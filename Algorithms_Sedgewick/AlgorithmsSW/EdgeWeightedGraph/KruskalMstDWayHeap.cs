namespace AlgorithmsSW.EdgeWeightedGraph;

using PriorityQueue;

public class KruskalMSTDWayHeap<TWeight>
{
	private Queue<Edge<TWeight>> minimumSpanningTree;

	public KruskalMSTDWayHeap(IEdgeWeightedGraph<TWeight> graph, int heapDegree)
	{
		minimumSpanningTree = new Queue<Edge<TWeight>>();
		var priorityQueue = new FixedCapacityMinNHeap<Edge<TWeight>>(heapDegree, graph.EdgeCount, new EdgeComparer<TWeight>(graph.Comparer));

		foreach (var edge in graph.Edges)
		{
			priorityQueue.Push(edge);
		}

		var unionFind = new UnionFind(graph.VertexCount);

		while (!priorityQueue.IsEmpty && minimumSpanningTree.Count < graph.VertexCount - 1)
		{
			var edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;

			if (unionFind.IsConnected(vertex0, vertex1))
			{
				continue;
			}

			unionFind.Union(vertex0, vertex1);
			minimumSpanningTree.Enqueue(edge);
		}
	}

	public IEnumerable<Edge<TWeight>> Edges => minimumSpanningTree;
}
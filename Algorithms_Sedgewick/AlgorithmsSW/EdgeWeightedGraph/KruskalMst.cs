using AlgorithmsSW.PriorityQueue;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class KruskalMst<T> : IMst<T>
{
	private readonly Queue<Edge<T>> mst;
	
	public IEnumerable<Edge<T>> Edges => mst;

	public T Weight => throw new NotImplementedException();

	public KruskalMst(EdgeWeightedGraphWithAdjacencyLists<T> graph, IComparer<T> comparer)
	{
		mst = new Queue<Edge<T>>();
		
		IPriorityQueue<Edge<T>> priorityQueue = new FixedCapacityMinBinaryHeap<Edge<T>>(graph.EdgeCount, new EdgeComparer<T>(comparer));
		
		foreach (var edge in graph.Edges)
		{
			priorityQueue.Push(edge);
		}
		
		var unionFind = new UnionFind(graph.VertexCount);
		
		while (!priorityQueue.IsEmpty() && mst.Count < graph.VertexCount - 1)
		{
			var edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;
			
			if (unionFind.IsConnected(vertex0, vertex1))
			{
				continue;
			}
			
			unionFind.Union(vertex0, vertex1);
			mst.Enqueue(edge);
		}
	}
	
	
}

using Algorithms_Sedgewick.PriorityQueue;
using Algorithms_Sedgewick.Queue;

namespace Algorithms_Sedgewick.EdgeWeightedGraph;

public class LazyPrimMst<T>
	where T : IComparable<T>
{
	private readonly bool[] marked;
	private readonly IQueue<Edge<T>> mst = DataStructures.Queue<Edge<T>>();
	private readonly IPriorityQueue<Edge<T>> priorityQueue;
	
	public IEnumerable<Edge<T>> Edges => mst;
	
	public LazyPrimMst(EdgeWeightedGraphWithAdjacencyLists<T> graph)
	{
		marked = new bool[graph.VertexCount];
		priorityQueue = new FixedCapacityMinBinaryHeap<Edge<T>>(graph.EdgeCount);
		
		Visit(graph, 0);
		
		while (!priorityQueue.IsEmpty())
		{
			var edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;
			
			if (marked[vertex0] && marked[vertex1])
			{
				continue;
			}
			
			mst.Enqueue(edge);
			
			if (!marked[vertex0])
			{
				Visit(graph, vertex0);
			}
			
			if (!marked[vertex1])
			{
				Visit(graph, vertex1);
			}
		}
	}

	public LazyPrimMst(IPriorityQueue<Edge<T>> priorityQueue)
	{
		this.priorityQueue = priorityQueue;
	}

	private void Visit(EdgeWeightedGraphWithAdjacencyLists<T> graph, int vertex)
	{
		marked[vertex] = true;
		
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			if (!marked[edge.OtherVertex(vertex)])
			{
				priorityQueue.Push(edge);
			}
		}
	}
}

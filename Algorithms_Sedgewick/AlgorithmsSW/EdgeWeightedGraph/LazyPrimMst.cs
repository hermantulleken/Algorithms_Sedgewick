using AlgorithmsSW.PriorityQueue;
using AlgorithmsSW.Queue;

namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

public class LazyPrimMst<T> : IMst<T>
	where T : INumber<T>
{
	private readonly bool[] marked;
	private readonly IQueue<Edge<T>> mst = DataStructures.Queue<Edge<T>>();
	private readonly IPriorityQueue<Edge<T>> priorityQueue;
	
	public IEnumerable<Edge<T>> Edges => mst;
	
	public T Weight => throw new NotImplementedException();
	
	public LazyPrimMst(IEdgeWeightedGraph<T> graph)
	{
		marked = new bool[graph.VertexCount];
		priorityQueue = new FixedCapacityMinBinaryHeap<Edge<T>>(graph.EdgeCount, new EdgeComparer<T>());
		
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

	private void Visit(IEdgeWeightedGraph<T> graph, int vertex)
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

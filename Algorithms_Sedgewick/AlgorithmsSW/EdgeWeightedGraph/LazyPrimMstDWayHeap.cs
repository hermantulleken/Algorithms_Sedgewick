namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

/// <summary>
/// A lazy implementation of Prim's algorithm for finding the minimum spanning tree of a weighted graph, using a N-heap.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class LazyPrimMstDWayHeap<TWeight> : IMst<TWeight>
	where TWeight : IFloatingPoint<TWeight>
{
	private readonly bool[] marked;
	private readonly Queue<Edge<TWeight>> minimumSpanningTree;
	private readonly PriorityQueue.FixedCapacityMinNHeap<Edge<TWeight>> priorityQueue;

	/// <inheritdoc />
	public IEnumerable<Edge<TWeight>> Edges => minimumSpanningTree;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="LazyPrimMstDWayHeap{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the minimum spanning tree of.</param>
	/// <param name="heapDegree">The degree of the heap to use.</param>
	public LazyPrimMstDWayHeap(IReadOnlyEdgeWeightedGraph<TWeight> graph, int heapDegree)
	{
		priorityQueue = new(heapDegree, graph.EdgeCount, new EdgeComparer<TWeight>());
		marked = new bool[graph.VertexCount];
		minimumSpanningTree = new();

		Visit(graph, 0); // Assumes graph is connected

		while (!priorityQueue.IsEmpty)
		{
			var edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;

			if (marked[vertex0] && marked[vertex1])
			{
				continue;
			}

			minimumSpanningTree.Enqueue(edge);

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
	
	private void Visit(IReadOnlyEdgeWeightedGraph<TWeight> graph, int vertex)
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

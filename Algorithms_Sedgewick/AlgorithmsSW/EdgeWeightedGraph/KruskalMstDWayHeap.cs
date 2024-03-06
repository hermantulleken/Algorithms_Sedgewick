namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;
using PriorityQueue;

/// <summary>
/// An implementation of Kruskal's algorithm for finding the minimum spanning tree of a weighted graph, using a N-heap.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class KruskalMstDWayHeap<TWeight> : IMst<TWeight>
	where TWeight : INumber<TWeight>
{
	private readonly Queue<Edge<TWeight>> minimumSpanningTree;

	/// <inheritdoc />
	public IEnumerable<Edge<TWeight>> Edges => minimumSpanningTree;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="KruskalMstDWayHeap{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the minimum spanning tree of.</param>
	/// <param name="heapDegree">The degree of the heap to use.</param>
	public KruskalMstDWayHeap(IEdgeWeightedGraph<TWeight> graph, int heapDegree)
	{
		minimumSpanningTree = new Queue<Edge<TWeight>>();
		var priorityQueue = new FixedCapacityMinNHeap<Edge<TWeight>>(heapDegree, graph.EdgeCount, new EdgeComparer<TWeight>());

		foreach (var edge in graph.WeightedEdges)
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
}

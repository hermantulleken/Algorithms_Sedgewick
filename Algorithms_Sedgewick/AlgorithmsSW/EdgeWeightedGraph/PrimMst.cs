using AlgorithmsSW.PriorityQueue;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

public class PrimMst<T> : IMst<T> 
	where T : IComparable<T>, IFloatingPoint<T>
{
	private readonly Edge<T>[] edgeTo;
	private readonly T[] distTo;
	private readonly bool[] marked;
	private readonly IndexPriorityQueue<T> priorityQueue;

	public IEnumerable<Edge<T>> Edges => edgeTo.Where(e => e != null);

	public T Weight => throw new NotImplementedException(); // Calculate weight as necessary

	public PrimMst(IEdgeWeightedGraph<T> graph, T minValue, T maxValue)
	{
		graph.ThrowIfNull();

		edgeTo = new Edge<T>[graph.VertexCount];
		distTo = new T[graph.VertexCount];
		marked = new bool[graph.VertexCount];
		priorityQueue = new(graph.VertexCount, Comparer<T>.Default);

		// Initialize distTo with maxValue
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			distTo[vertex] = maxValue;
		}

		// Start with vertex 0
		distTo[0] = minValue;
		priorityQueue.Insert(0, distTo[0]);

		// Assert correct initialization
		Assert(distTo[0] == minValue, "distTo[0] should be initialized to minValue");
		Assert(distTo.Skip(1).All(d => d == maxValue), "All distTo values should be initialized to maxValue");

		while (!priorityQueue.IsEmpty)
		{
			var (vertex, _) = priorityQueue.PopMin();
			Visit(graph, vertex);
		}

		// Assert that all vertices are marked
		Assert(marked.All(m => m), "All vertices should be marked in the MST");
	}

	private void Visit(IEdgeWeightedGraph<T> graph, int vertex)
	{
		marked[vertex] = true;

		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int otherVertex = edge.OtherVertex(vertex);

			if (marked[otherVertex])
			{
				continue;
			}

			if (edge.Weight >= distTo[otherVertex])
			{
				continue;
			}

			distTo[otherVertex] = edge.Weight;
			edgeTo[otherVertex] = edge;

			// Assert the edge is the minimum for the vertex
			Assert(edgeTo[otherVertex] != null && edgeTo[otherVertex].Weight == distTo[otherVertex], "Edge should be the minimum for the vertex");

			if (priorityQueue.Contains(otherVertex))
			{
				priorityQueue.UpdateValue(otherVertex, distTo[otherVertex]);
			}
			else
			{
				priorityQueue.Insert(otherVertex, distTo[otherVertex]);
			}
		}
	}
}

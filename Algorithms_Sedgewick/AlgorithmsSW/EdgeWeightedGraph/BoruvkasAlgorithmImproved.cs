namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;
using List;
using Support;

using static System.Diagnostics.Debug;

// ReSharper disable once IdentifierTypo
/// <summary>
/// An implementation of Boruvka's algorithm to find the MST of a graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/// <remarks>This algorithm is not really better than <see cref="BoruvkasAlgorithm{TWeight}"/>.</remarks>
public class BoruvkasAlgorithmImproved<TWeight> : IMst<TWeight>
	where TWeight : IFloatingPoint<TWeight>
{
	private readonly int[] component;
	private readonly ResizeableArray<Edge<TWeight>> mstEdges;

	// ReSharper disable once IdentifierTypo
	public BoruvkasAlgorithmImproved(IEdgeWeightedGraph<TWeight> graph)
	{
		component = new int[graph.VertexCount];
		mstEdges = new(graph.EdgeCount);
		
		var forest = new DoublyLinkedList<int>[graph.VertexCount];
		int componentCount = graph.VertexCount;
		
		forest.Fill(index => [index]);
		component.Fill(index => index);
		var minEdge = new Edge<TWeight>?[graph.VertexCount];
		
		IterationGuard.Reset();
		while (componentCount > 1)
		{
			IterationGuard.Inc();
			minEdge.Fill((Edge<TWeight>?) null);
			
			foreach (var edge in graph.WeightedEdges)
			{
				int component0 = component[edge.Vertex0];
				int component1 = component[edge.Vertex1];

				if (component0 == component1)
				{
					continue;
				}
				
				if (minEdge[component0] == null || edge.Weight < minEdge[component0]!.Weight)
				{
					minEdge[component0] = edge;
				}

				if (minEdge[component1] == null || edge.Weight < minEdge[component1]!.Weight)
				{
					minEdge[component1] = edge;
				}
			}
			
			// Add the found minimum edges to the MST
			foreach (var edge in minEdge)
			{
				/*
					Is this connectivity test really necessary?
					Yes, because the min edges attached to component 0 and 1, for example, may be the sae edge.
				*/
				if (edge == null || IsConnected(edge.Vertex0, edge.Vertex1))
				{
					continue;
				}

				mstEdges.Add(edge);
				
				int component0 = component[edge.Vertex0];
				int component1 = component[edge.Vertex1];

				Assert(forest[component1] != null);
				
				foreach (int vertex in forest[component1])
				{
					component[vertex] = component0;
				}
				
				Assert(forest[component0] != null);
				
				forest[component0].Concat(forest[component1]);
				forest[component1] = null;
				componentCount--;
			}
		}

		return;

		bool IsConnected(int vertex0, int vertex1) => component[vertex0] == component[vertex1];
	}

	public IEnumerable<Edge<TWeight>> Edges => mstEdges;
}

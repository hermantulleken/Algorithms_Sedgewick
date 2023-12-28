namespace AlgorithmsSW.EdgeWeightedGraph;

using List;
using Support;

public class BoruvkasAlgorithm<TWeight> 
	: IMst<TWeight>
{
	private readonly ResizeableArray<Edge<TWeight>> mstEdges = new();
	
	public IEnumerable<Edge<TWeight>> Edges => mstEdges;

	#region GuardExample
	public BoruvkasAlgorithm(IReadOnlyEdgeWeightedGraph<TWeight> graph)
	{
		var unionFind = new UnionFind(graph.VertexCount);
		var comparer = graph.Comparer;
		
		IterationGuard.Reset();
		while (unionFind.ComponentCount > 1)
		{
			IterationGuard.Inc();
			
			var minEdge = FindMinimumEdges(graph, unionFind, comparer);
			AddMinimumEdgesToMst(minEdge, unionFind);
		}
	}
	#endregion

	private void AddMinimumEdgesToMst(Edge<TWeight>?[] minEdge, UnionFind unionFind)
	{
		foreach (var edge in minEdge)
		{
			/*
					Is this connectivity test really necessary?
					Yes, because the min edges attached to component 0 and 1, for example, may be the sae edge.
				*/
			if (edge == null || unionFind.IsConnected(edge.Vertex0, edge.Vertex1))
			{
				continue;
			}

			mstEdges.Add(edge);
			unionFind.Union(edge.Vertex0, edge.Vertex1);
		}
	}

	private static Edge<TWeight>?[] FindMinimumEdges(IReadOnlyEdgeWeightedGraph<TWeight> graph, UnionFind unionFind, IComparer<TWeight> comparer)
	{
		Edge<TWeight>?[] minEdge = new Edge<TWeight>[graph.VertexCount];

		foreach (var edge in graph.WeightedEdges)
		{
			int component1 = unionFind.GetComponentIndex(edge.Vertex0);
			int component2 = unionFind.GetComponentIndex(edge.Vertex1);

			if (component1 == component2)
			{
				continue;
			}
				
			if (minEdge[component1] == null || comparer.Compare(edge.Weight, minEdge[component1].Weight) < 0)
			{
				minEdge[component1] = edge;
			}

			if (minEdge[component2] == null || comparer.Compare(edge.Weight, minEdge[component2].Weight) < 0)
			{
				minEdge[component2] = edge;
			}
		}

		return minEdge;
	}
}

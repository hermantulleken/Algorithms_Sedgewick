namespace AlgorithmsSW.EdgeWeightedGraph;

using List;

/// <summary>
/// This implementation is the same as <see cref="BoruvkasAlgorithmImproved{TWeight}"/> with a few tweaks, but the algorithms
/// perform nearly the same.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class BoruvkasAlgorithmImprovement2<TWeight> : IMst<TWeight>
{
	private readonly Queue<Edge<TWeight>> minimumSpanningTree;

	public BoruvkasAlgorithmImprovement2(IReadOnlyEdgeWeightedGraph<TWeight> edgeWeightedGraph)
	{
		minimumSpanningTree = new Queue<Edge<TWeight>>();
		var forest = new DoublyLinkedList<int>[edgeWeightedGraph.VertexCount];
			
		for (int i = 0; i < edgeWeightedGraph.VertexCount; i++)
		{
			forest[i] = [i];
		}
			
		var comparer = edgeWeightedGraph.Comparer;

		for (int stage = 1; stage < edgeWeightedGraph.VertexCount; stage += stage)
		{
			if (minimumSpanningTree.Count == edgeWeightedGraph.VertexCount - 1)
			{
				break;
			}

			var minEdges = new Edge<TWeight>?[edgeWeightedGraph.VertexCount];

			foreach (var edge in edgeWeightedGraph.Edges)
			{
				int vertex0 = edge.Vertex0;
				int vertex1 = edge.Vertex1;

				int component0 = forest[vertex0].First.Item;
				int component1 = forest[vertex1].First.Item;

				if (component0 == component1)
				{
					continue;
				}

				if (minEdges[component0] == null || comparer.Compare(edge.Weight, minEdges[component0].Weight) < 0)
				{
					minEdges[component0] = edge;
				}

				if (minEdges[component1] == null || comparer.Compare(edge.Weight, minEdges[component1].Weight) < 0)
				{
					minEdges[component1] = edge;
				}
			}

			for (int i = 0; i < edgeWeightedGraph.VertexCount; i++)
			{
				var closestEdge = minEdges[i];

				if (closestEdge == null)
				{
					continue;
				}

				int vertex1 = closestEdge.Vertex0;
				int vertex2 = closestEdge.Vertex1;

				int treeId1 = forest[vertex1].First.Item;
				int treeId2 = forest[vertex2].First.Item;

				if (treeId1 == treeId2)
				{
					continue;
				}

				minimumSpanningTree.Enqueue(closestEdge);
						
				if (forest[vertex1].Count <= forest[vertex2].Count)
				{
					MergeForests(forest, vertex1, vertex2);
				}
				else
				{
					MergeForests(forest, vertex2, vertex1);
				}
			}
		}
	}

	private void MergeForests(DoublyLinkedList<int>[] forest, int smallerTree, int largerTree)
	{
		var elementsToUpdate = new HashSet<int>();
			
		foreach (int element in forest[smallerTree])
		{
			elementsToUpdate.Add(element);
		}

		forest[largerTree].Concat(forest[smallerTree]);

		foreach (int element in elementsToUpdate)
		{
			forest[element] = forest[largerTree];
		}
	}

	public IEnumerable<Edge<TWeight>> Edges => minimumSpanningTree;
}

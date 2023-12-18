using AlgorithmsSW.Queue;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class Vyssotsky {

	// O(E * (V + E)) = O(E^2)
	public IQueue<Edge<T>> minimumSpanningTree<T>(IEdgeWeightedGraph<T> graph, T minValue) 
	{
		var putativeTree = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount, graph.Comparer);

		foreach (var edge in graph.Edges)
		{
			putativeTree.AddEdge(edge);
			Set.HashSet<int> vertexToSearch = new Set.HashSet<int>(Comparer<int>.Default) { edge.Vertex0 };

			EdgeWeightedCycle<T> edgeWeightedCycle = new EdgeWeightedCycle<T>(putativeTree, vertexToSearch);

			// If a cycle was formed, delete the maximum-weight edge in it
			if (edgeWeightedCycle.HasCycle())
			{
				Stack<Edge<T>> cycle = edgeWeightedCycle.Cycle();
				Edge<T> maxWeightEdge = null;
				T maxWeight = minValue;

				foreach (Edge<T> edgeInCycle in cycle) 
				{
					if (graph.Comparer.Compare(edgeInCycle.Weight, maxWeight) > 0)
					{
						maxWeight = edgeInCycle.Weight;
						maxWeightEdge = edgeInCycle;
					}
				}

				putativeTree.RemoveEdge(maxWeightEdge);
			}
		}

		var minimumSpanningTree = DataStructures.Queue<Edge<T>>();
		
		foreach (var edge in putativeTree.Edges)
		{
			minimumSpanningTree.Enqueue(edge);
		}
		
		return minimumSpanningTree;
	}
}

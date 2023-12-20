using AlgorithmsSW.Queue;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class Vyssotsky<T> : IMst<T>
{
	private readonly IQueue<Edge<T>> minimumSpanningTree;

	/// <inheritdoc />
	public IEnumerable<Edge<T>> Edges => minimumSpanningTree;
	
	// O(E * (V + E)) = O(E^2)
	public Vyssotsky(IEdgeWeightedGraph<T> graph, T minValue) 
	{
		var putativeTree = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount, graph.Comparer);

		foreach (var edge in graph.Edges)
		{
			putativeTree.AddEdge(edge);
			
			var vertexToSearch = new Set.HashSet<int>(Comparer<int>.Default) { edge.Vertex0 };
			var edgeWeightedCycle = new EdgeWeightedCycle<T>(putativeTree, vertexToSearch);

			// If a cycle was formed, delete the maximum-weight edge in it
			if (!edgeWeightedCycle.HasCycle())
			{
				continue;
			}
			
			var cycle = edgeWeightedCycle.Cycle();
			Edge<T> maxWeightEdge = null;
			var maxWeight = minValue;

			foreach (var edgeInCycle in cycle) 
			{
				if (graph.Comparer.Compare(edgeInCycle.Weight, maxWeight) > 0)
				{
					maxWeight = edgeInCycle.Weight;
					maxWeightEdge = edgeInCycle;
				}
			}

			putativeTree.RemoveEdge(maxWeightEdge);
		}

		minimumSpanningTree = DataStructures.Queue<Edge<T>>();
		
		foreach (var edge in putativeTree.Edges)
		{
			minimumSpanningTree.Enqueue(edge);
		}
	}
}

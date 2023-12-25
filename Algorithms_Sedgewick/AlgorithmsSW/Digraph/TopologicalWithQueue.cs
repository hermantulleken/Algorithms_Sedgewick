using AlgorithmsSW.List;

namespace AlgorithmsSW.Digraph;

public class TopologicalWithQueue : ITopological
{
	private readonly ResizeableArray<int> order;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="TopologicalWithQueue"/> class.
	/// </summary>
	public TopologicalWithQueue(IReadOnlyDigraph digraph)
	{
		var degrees = new Degrees(digraph);

		var indegrees = digraph.Vertexes.Select(degrees.GetIndegree).ToResizableArray(digraph.VertexCount);

		var queue = DataStructures.Queue<int>();
		foreach (int source in degrees.Sources)
		{
			queue.Enqueue(source);
		}

		order = [];
		
		while (queue.Any())
		{
			var item = queue.Dequeue();
			order.Add(item);
			foreach (int target in digraph.GetAdjacents(item))
			{
				indegrees[target]--;
				if (indegrees[target] == 0)
				{
					queue.Enqueue(target);
				}
			}
		}
	}

	/// <inheritdoc />
	public bool IsDirectedAcyclic => order.Count == order.Distinct().Count();
	
	/// <inheritdoc />
	public IEnumerable<int>? Order => order;
}

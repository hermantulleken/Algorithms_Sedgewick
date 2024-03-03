namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using EdgeWeightedGraph;
using List;
using PriorityQueue;
using Support;

/// <summary>
/// This is an implementation of  (a modified) Dijkstra's algorithm that finds the k shortest paths between a source
/// and a target.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/// <remarks>
/// Reference: <see href="https://en.wikipedia.org/wiki/K_shortest_path_routing"/>.
/// </remarks>
[ExerciseReference(4, 4, 7)]
public class KShortestPaths<TWeight> : IKShortestPaths<TWeight>
	where TWeight : IFloatingPoint<TWeight>
{
	private readonly ResizeableArray<DirectedPath<TWeight>> shortestPaths;

	/// <summary>
	/// Initializes a new instance of the <see cref="KShortestPaths{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex.</param>
	/// <param name="target">The target vertex.</param>
	/// <param name="k">The number of shortest paths to find.</param>
	public KShortestPaths(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph,
		int source,
		int target,
		int k)
	{
		#region PseudoCodeExample
		shortestPaths = [];
		var paths = new DirectedPath<TWeight>[graph.VertexCount];
		int[] count = new int[graph.VertexCount];
		var queue = DataStructures.PriorityQueue(graph.VertexCount, new DirectedPathComparer<TWeight>(Comparer<TWeight>.Default));
		
		//queue.Push(new([source], zero));

		foreach (var edge in graph.GetIncidentEdges(source))
		{
			queue.Push(new(edge));
		}
		
		IterationGuard.Reset();
		
		while (!queue.IsEmpty() && count[target] < k)
		{
			IterationGuard.Inc();
			
			var shortestPath = queue.PopMin();
			int lastVertex = shortestPath.TargetVertex;
			paths[lastVertex] = shortestPath;
			count[lastVertex]++;

			if (lastVertex == target)
			{
				shortestPaths.Add(shortestPath);
			}

			/*	The Wikipedia keeps track of the number of paths for each vertex, and has this early bailout.
				However, when there are equal paths, I think this causes the algorithm to find too few paths.

				if (count[u] >= k) continue;
			*/

			foreach (var edge in graph.GetIncidentEdges(lastVertex))
			{
				var newPath = shortestPath.Combine(edge);
					
				queue.Push(newPath);
			}
		}
		#endregion
	}
	
	/// <summary>
	/// Gets the kth shortest path between the source and target.
	/// </summary>
	/// <param name="rank">The rank of the path to get.</param>
	/// <exception cref="InvalidOperationException">No path exists with the given rank.</exception>
	/// <returns>The kth shortest path.</returns>
	public DirectedPath<TWeight> GetPath(int rank)
	{
		if (!HasPath(rank))
		{
			throw new InvalidOperationException($"There are only {shortestPaths.Count} paths between the source and target.");
		}
		
		return shortestPaths[rank];
	}

	/// <summary>
	/// Gets whether there is a path with the given rank.
	/// </summary>
	/// <param name="k">The rank of the path.</param>
	public bool HasPath(int k) => shortestPaths.Count > k;
	
	/// <summary>
	/// Gets the distance of the kth shortest path between the source and target.
	/// </summary>
	/// <param name="k">The rank of the path whose distance to get.</param>
	/// <exception cref="InvalidOperationException">No path exists with the given rank.</exception>
	public TWeight GetDistance(int k)
	{
		if (!HasPath(k))
		{
			throw new InvalidOperationException($"There are only {shortestPaths.Count} paths between the source and target.");
		}
		
		return shortestPaths[k].Distance;
	}
}

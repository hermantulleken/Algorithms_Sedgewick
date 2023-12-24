namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using PriorityQueue;
using Support;
using static System.Diagnostics.Debug;


/// <summary>
/// This is an implementation of  (a modified) Dijkstra's algorithm that finds the k shortest paths between a source
/// and a target.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/// <remarks>
/// Reference: <see href"https://en.wikipedia.org/wiki/K_shortest_path_routing"/>
/// </remarks>
public class KShortestPaths<TWeight>
{
	private readonly ResizeableArray<KShortestPaths<TWeight>.Path> shortestPaths;

	private record Path(ResizeableArray<int> Vertices, TWeight Distance)
	{
		public override string ToString() => Vertices.Pretty();
	}

	private class PathComparer(IComparer<TWeight> weightComparer)
		: IComparer<Path>
	{
		public int Compare(Path? x, Path? y)
		{
			Assert(x != null);
			Assert(y != null);
			
			return weightComparer.Compare(x.Distance, y.Distance);
		}
	}
	
	public KShortestPaths(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph,
		int source,
		int target,
		int k,
		TWeight zero,
		Func<TWeight, TWeight, TWeight> add)
	{
		shortestPaths = DataStructures.List<Path>();
		var paths = new Path[graph.VertexCount];
		int[] count = new int[graph.VertexCount];
		var queue = DataStructures.PriorityQueue(graph.VertexCount, new PathComparer(graph.Comparer));
		queue.Push(new([source], zero));

		while (!queue.IsEmpty() && count[target] < k)
		{
			
			var shortestPath = queue.PopMin();
			int lastVertex = shortestPath.Vertices[^1];
			paths[lastVertex] = shortestPath;
			count[lastVertex]++;

			if (lastVertex == target)
			{
				shortestPaths.Add(shortestPath);
			}

			/*
			The Wikipedia has this early bailout. However, when there are equal paths, I think this causes the
			algorithm to find too few paths.

			if (count[u] >= k)
			{
				continue;
			}
		*/

			foreach (int adjacent in graph.GetAdjacents(lastVertex))
			{
				var newPath = new Path(
					[..shortestPath.Vertices, adjacent], 
					add(shortestPath.Distance, graph.GetUniqueEdge(lastVertex, adjacent).Weight));
					
				queue.Push(newPath);
			}
		}
	}
	
	public IRandomAccessList<int> GetPath(int k)
	{
		if (!HasPath(k))
		{
			throw new InvalidOperationException($"There are only {shortestPaths.Count} paths between the source and target.");
		}
		return shortestPaths[k].Vertices;
	}

	public bool HasPath(int k) => shortestPaths.Count > k;
	
	public TWeight GetDistance(int k) => shortestPaths[k].Distance;
}

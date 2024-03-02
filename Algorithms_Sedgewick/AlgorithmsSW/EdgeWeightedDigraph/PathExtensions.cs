namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;

public static class PathExtensions
{
	public static bool TryFindCycle(this IRandomAccessList<int> path, out int startIndex, out int endIndex)
	{
		var vertexToPathIndex = DataStructures.HashTable<int, int>(Comparer<int>.Default);
		
		for (int pathIndex = 0; pathIndex < path.Count; pathIndex++)
		{
			int vertex = path[pathIndex];
			
			if (vertexToPathIndex.ContainsKey(vertex))
			{
				startIndex = vertexToPathIndex[vertex];
				endIndex = pathIndex;
				return true;
			}
			
			vertexToPathIndex[vertex] = pathIndex;
		}

		startIndex = -1;
		endIndex = -1;
		
		return false;
	}

	public static bool TryFindCycle<TWeight>(
		this DirectedPath<TWeight> path, 
		TWeight zero,
		Func<TWeight, TWeight, TWeight> add,
		out DirectedPath<TWeight>? cycle)
	where TWeight : IFloatingPoint<TWeight>
	{
		// Indexes:        0 1 2 3 4 5 6 7 8
		// Path:           0 1 2 3 4 5 6 3 8
		// EdgeIndex:       0 1 2 3 4 5 6 7 
		// Vertex Start End      3       7
		// Edge Start             3     6
		// Skip 3          (0 1 2)
		// Take 4 = 7 - 3  (3 4 5 6)
		bool hasCycle = path.Vertexes.TryFindCycle(out int startIndex, out int endIndex);
		
		if (hasCycle)
		{
			cycle = path.Skip(startIndex).Take(endIndex - startIndex);
			return true;
		}

		cycle = null;
		return false;
	}

	public static bool TryGetPath<TWeight>(
		IEdgeWeightedDigraph<TWeight> graph, 
		IEnumerable<int> vertexPath,
		out DirectedPath<TWeight>? path)
		where TWeight : IFloatingPoint<TWeight>
	{
		var edges = new ResizeableArray<DirectedEdge<TWeight>>();
		foreach (var pair in vertexPath.SlidingWindow2())
		{
			var found = graph.TryGetUniqueEdge(pair.first, pair.last, out var edge);
			
			if (found != EdgeExistance.Unique)
			{
				path = null;
				return false;
			}
			
			edges.Add(edge);
		}
		
		path = new(edges);

		return true;
	}
}

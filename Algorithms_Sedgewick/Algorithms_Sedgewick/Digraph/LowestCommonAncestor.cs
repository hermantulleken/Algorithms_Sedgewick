using System.Numerics;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Digraphs;

public class LowestCommonAncestor
{
	private int[,] ancestors;
	
	public LowestCommonAncestor(IDigraph digraph)
	{
		var degrees = new Degrees(digraph);
		var roots = degrees.Sources.ToResizableArray(digraph.VertexCount);
		var paths = CreatePaths(digraph);
		
		int GetHeight(int vertex) => roots.Select(root => paths[vertex].PathTo(root).Count()).Max();
		
		int[,] heights = new int[digraph.VertexCount, digraph.VertexCount];

		var nodeHeights = digraph.Vertexes.Select(GetHeight).ToList();

		GetLeastCommonAncestors(digraph, roots, paths, heights, nodeHeights);
	}

	private void GetLeastCommonAncestors(IDigraph digraph, ResizeableArray<int> roots, DepthFirstDirectedPaths[] paths, int[,] heights, List<int> nodeHeights)
	{
		ancestors = CreateAncestors(digraph.VertexCount);

		for (int i = 0; i < digraph.VertexCount; i++)
		{
			for (int j = i; j < digraph.VertexCount; j++)
			{
				for (int potentialAncestor = 0; potentialAncestor < roots.Count; potentialAncestor++)
				{
					if (!paths[roots[potentialAncestor]].HasPathTo(i) || !paths[roots[potentialAncestor]].HasPathTo(j))
					{
						// Not an ancestor
						continue;
					}
					
					UpdateAncestorsAndHeights(i, j, potentialAncestor, nodeHeights, heights);
				}
			}
		}
	}
	
	private void UpdateAncestorsAndHeights(int vertex1, int vertex2, int potentialAncestor, List<int> nodeHeights, int[,] heights)
	{
		int potentialHeight = nodeHeights[potentialAncestor];

		if (ancestors[vertex1, vertex2] != -1 && potentialHeight <= heights[vertex1, vertex2])
		{
			return;
		}
		
		ancestors[vertex1, vertex2] = potentialAncestor;
		heights[vertex1, vertex2] = potentialHeight;
	}

	private static int[,] CreateAncestors(int vertexCount)
	{
		int[,] ancestors = new int[vertexCount, vertexCount];

		for (int i = 0; i < vertexCount; i++)
		{
			for (int j = 0; j < vertexCount; j++)
			{
				ancestors[i, j] = -1;
			}
		}

		return ancestors;
	}
	
	private static DepthFirstDirectedPaths[] CreatePaths(IDigraph digraph)
	{
		var paths = new DepthFirstDirectedPaths[digraph.VertexCount];
		for (int i = 0; i < digraph.VertexCount; i++)
		{
			paths[i] = new DepthFirstDirectedPaths(digraph, i);
		}

		return paths;
	}
	
	public bool HasCommonancestor(int vertex0, int vertex1) => TryGetLowestCommonAncestor(vertex0, vertex1, out _);

	public int GetLowestCommonAncestor(int vertex0, int vertex1)
	{
		if (TryGetLowestCommonAncestor(vertex0, vertex1, out int result))
		{
			return result;
		}

		throw ThrowHelper.TriedButFailed(
			nameof(GetLeastCommonAncestors), 
			nameof(HasCommonancestor), 
			nameof(TryGetLowestCommonAncestor));
	}
	
	public bool TryGetLowestCommonAncestor(int vertex0, int vertex1, out int lowestCommonAncestor)
	{
		lowestCommonAncestor = GetLowestCommonAncestorUnchecked(vertex0, vertex1);

		return lowestCommonAncestor != -1;
	}
	
	private int GetLowestCommonAncestorUnchecked(int vertex0, int vertex1)
		=> vertex0 < vertex1 ? ancestors[vertex0, vertex1] : ancestors[vertex1, vertex0];

	private int GetHeight(ResizeableArray<int> roots, DepthFirstDirectedPaths[] paths, int vertex)
		=> roots.Select(root => paths[root].PathTo(vertex).Count()).Max();
}

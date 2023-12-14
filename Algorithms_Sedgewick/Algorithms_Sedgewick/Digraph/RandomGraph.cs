using Algorithms_Sedgewick.Digraphs;
using static System.Diagnostics.Debug;

namespace Algorithms_Sedgewick.Digraph;

/// <summary>
/// Generates random digraphs.
/// </summary>
public static class RandomGraph
{
	public static IDigraph ErdosRenyiGraph(int vertexCount, int edgeCount)
	{
		var vertex0Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var vertex1Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var graph = new DigraphWithAdjacentsLists(vertexCount);

		foreach ((int vertex0, int vertex1) in vertex0Generator.Zip(vertex1Generator))
		{
			graph.AddEdge(vertex0, vertex1);
		}

		return graph;
	}

	public static IDigraph RandomSimple(int vertexCount, int edgeCount)
	{
		int maxEdgeCount = Math2.Sqr(vertexCount - 1);
		var edgeIndexes = Generator.UniqueUniformRandomInt_WithShuffledList(maxEdgeCount, edgeCount);
		var graph = new DigraphWithAdjacentsLists(vertexCount);

		foreach (int edgeIndex in edgeIndexes)
		{
			(int i, int j) = NoSelfLoop(vertexCount, edgeIndex);
			graph.AddEdge(i, j);
		}

		return graph;
	}
	
	private static (int, int) NoSelfLoop(int vertexCount, int index)
	{
		int i = index / (vertexCount - 1);
		int j = index % (vertexCount - 1);

		if (j >= i)
		{
			j++;
		}
		
		Assert(i >= 0);
		Assert(j >= 0);
		Assert(i < vertexCount);
		Assert(j < vertexCount);
		Assert(i != j);

		return (i, j);
	}
}

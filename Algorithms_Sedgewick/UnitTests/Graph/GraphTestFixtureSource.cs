namespace UnitTests;

using AlgorithmsSW.Graph;

public static class GraphTestFixtureSource
{
	private static IGraph MakeDynamicGraph(int vertCount)
	{
		var graph = new DynamicGraph();

		for (int i = 0; i < vertCount; i++)
		{
			graph.AddVertexes(i);
		}

		return graph;
	}
	
	public static Func<int, IGraph>[] TestCases =>
	[
		vertexCount => new GraphWithAdjacentsIntArray(vertexCount),
		vertexCount => new GraphWithAdjacentsBoolArray(vertexCount),
		vertexCount => new GraphWithAdjacentsLists(vertexCount),
		vertexCount => new GraphWithAdjacentsSet(vertexCount),
		vertexCount => new GraphWithAdjacentsCounters(vertexCount),
		vertexCount => new GraphWithNoSelfLoops(() => new GraphWithAdjacentsLists(vertexCount)),
		MakeDynamicGraph
	];
}

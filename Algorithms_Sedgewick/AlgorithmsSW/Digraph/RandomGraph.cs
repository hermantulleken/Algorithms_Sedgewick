using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Digraph;

/// <summary>
/// Generates random digraphs.
/// </summary>
public static class RandomGraph
{
	/// <summary>
	/// Generates a graph with edges uniformly chosen from the set of all possible edges, including self-loops.
	/// </summary>
	/// <param name="vertexCount">How many vertexes the graph should have.</param>
	/// <param name="edgeCount">How many edges the graph should have.</param>
	/// <returns>A random graph.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexCount"/> or <paramref name="edgeCount"/> is
	/// not positive.</exception>
	public static IDigraph ErdosRenyiGraph(int vertexCount, int edgeCount)
	{
		vertexCount.ThrowIfNotPositive();
		edgeCount.ThrowIfNotPositive();
		
		var vertex0Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var vertex1Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var graph = new DigraphWithAdjacentsLists(vertexCount);

		foreach ((int vertex0, int vertex1) in vertex0Generator.Zip(vertex1Generator))
		{
			graph.AddEdge(vertex0, vertex1);
		}

		return graph;
	}

	/// <summary>
	/// Generates a graph with edges uniformly chosen from the set of all possible edges, excluding self-loops.
	/// </summary>
	/// <param name="vertexCount">How many vertexes the graph should have.</param>
	/// <param name="edgeCount">How many edges the graph should have.</param>
	/// <returns>A random graph.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexCount"/> or <paramref name="edgeCount"/> is
	/// not positive.</exception>
	public static IDigraph RandomSimple(int vertexCount, int edgeCount)
	{
		vertexCount.ThrowIfNotPositive();
		edgeCount.ThrowIfNotPositive();
		
		int maxEdgeCount = MathX.Sqr(vertexCount - 1);
		var edgeIndexes = Generator.UniqueUniformRandomInt_WithShuffledList(maxEdgeCount, edgeCount);
		var graph = new DigraphWithAdjacentsLists(vertexCount);

		foreach (int edgeIndex in edgeIndexes)
		{
			(int i, int j) = NoSelfLoop(vertexCount, edgeIndex);
			graph.AddEdge(i, j);
		}

		return graph;
	}
	
	// This maps pairs to a new pair without self loops. 
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

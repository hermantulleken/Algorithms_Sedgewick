using System.Diagnostics;

namespace AlgorithmsSW.Graph;

using static Debug;

public static class RandomGraph
{
	public static IGraph ErdosRenyiGraph(int vertexCount, int edgeCount)
	{
		var vertex0Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var vertex1Generator = Generator.UniformRandomInt(vertexCount).Take(edgeCount);
		var graph = new GraphWithAdjacentsSet(vertexCount);

		foreach ((int vertex0, int vertex1) in vertex0Generator.Zip(vertex1Generator))
		{
			graph.AddEdge(vertex0, vertex1);
		}

		return graph;
	}
	
	public static IGraph GetRandomSimpleGraph(int vertexCount, int edgeCount)
	{
		int maxEdgeCount = vertexCount * (vertexCount - 1) / 2;
		
		if (edgeCount > maxEdgeCount)
		{
			throw new ArgumentException($"The maximum number of edges for a graph with {vertexCount} vertices is {maxEdgeCount}.");
		}
		
		if (edgeCount < 0)
		{
			throw new ArgumentException("The number of edges must be non-negative.");
		}
		
		IGraph graph = new GraphWithAdjacentsSet(vertexCount);

		if (edgeCount == 0)
		{
			return graph;
		}
		
		var vertex0Generator = Generator.UniformRandomInt(vertexCount);
		var vertex1Generator = Generator.UniformRandomInt(vertexCount);

		if (edgeCount > maxEdgeCount / 2)
		{
			graph = Graph.CreateCompleteGraph(n => new GraphWithAdjacentsSet(n), vertexCount);
			
			foreach ((int vertex0, int vertex1) in vertex0Generator.Zip(vertex1Generator))
			{
				if (vertex0 == vertex1)
				{
					continue;
				}
			
				if (graph.ContainsEdge(vertex0, vertex1))
				{
					graph.RemoveEdge(vertex0, vertex1);
				}
			
				if (graph.EdgeCount == edgeCount)
				{
					break;
				}
			}
		}
		else
		{
			foreach ((int vertex0, int vertex1) in vertex0Generator.Zip(vertex1Generator))
			{
				if (vertex0 == vertex1)
				{
					continue;
				}
			
				if (!graph.ContainsEdge(vertex0, vertex1))
				{
					graph.AddEdge(vertex0, vertex1);
				}
			
				if (graph.EdgeCount == edgeCount)
				{
					break;
				}
			}
		}

		return graph;
	}
	
	public static IGraph GetRandomSimpleGraph2(int vertexCount, int edgeCount)
	{
		var edgeIndexes = Generator.UniqueUniformRandomInt_WithShuffledList(Triangle(vertexCount - 1), edgeCount);
		var graph = new GraphWithAdjacentsSet(vertexCount);
		
		foreach (int edgeIndex in edgeIndexes)
		{
			int i = InverseTriangle(edgeIndex);
			int j = edgeIndex - Triangle(i);

			Assert(j <= i);
			Assert(i + 1 <= vertexCount);
			Assert(j <= vertexCount);
			
			graph.AddEdge(j, i + 1);
		}

		return graph;
	}

	public static int Triangle(int n) => n * (n + 1) / 2;
	
	public static int InverseTriangle(int n) => (int)Math.Floor((-1 + Math.Sqrt(1 + 8 * n)) / 2);
}

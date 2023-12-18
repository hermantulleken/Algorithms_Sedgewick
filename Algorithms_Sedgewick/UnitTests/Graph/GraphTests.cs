using System.Linq;
using AlgorithmsSW.Graph;
using NUnit.Framework;

namespace UnitTests;

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
	
	public static Func<int, IGraph>[] TestCases = 
	{
		vertexCount => new GraphWithAdjacentsIntArray(vertexCount),
		vertexCount => new GraphWithAdjacentsBoolArray(vertexCount),
		vertexCount => new GraphWithAdjacentsLists(vertexCount),
		vertexCount => new GraphWithAdjacentsSet(vertexCount),
		vertexCount => new GraphWithAdjacentsCounters(vertexCount),
		vertexCount => new GraphWtihNoSelfLoops(() => new GraphWithAdjacentsLists(vertexCount)),
		MakeDynamicGraph,
	};
}

[TestFixtureSource(typeof(GraphTestFixtureSource), nameof(GraphTestFixtureSource.TestCases))]
public class GraphTests(Func<int, IGraph> graphFactory)
{
	[Test]
	public void TestEmptyGraphWithZeroVertices()
	{
		IGraph graph = graphFactory(0);
		Assert.That(graph.VertexCount, Is.EqualTo(0));
		Assert.That(graph.EdgeCount, Is.EqualTo(0));
		Assert.That(graph.IsEmpty, Is.True);
		Assert.That(graph, Is.Empty);
	}
	
	[Test]
	public void TestEmptyGraphWithFourVertices()
	{
		IGraph graph = graphFactory(4);
		Assert.That(graph.VertexCount, Is.EqualTo(4));
		Assert.That(graph.EdgeCount, Is.EqualTo(0));
		Assert.That(graph.IsEmpty, Is.False);
		Assert.That(graph.Vertexes, Has.Exactly(4).Items);
	}
	
	[Test]
	public void TestGraphWithFiveVerticesAndEdges()
	{
		IGraph graph = graphFactory(5);
		graph.AddEdge(0, 1);
		graph.AddEdge(0, 2);
		graph.AddEdge(1, 2);

		Assert.That(graph.VertexCount, Is.EqualTo(5));
		Assert.That(graph.EdgeCount, Is.EqualTo(3));
    
		var adjacents = graph.GetAdjacents(0).ToList();

		Assert.That(adjacents, !Contains.Item(0));
		Assert.That(adjacents, Contains.Item(1));
		Assert.That(adjacents, Contains.Item(2));
		Assert.That(adjacents, !Contains.Item(3));
		Assert.That(adjacents, !Contains.Item(4));

		adjacents = graph.GetAdjacents(1).ToList();
		
		Assert.That(adjacents, Contains.Item(0));
		Assert.That(adjacents, !Contains.Item(1));
		Assert.That(adjacents, Contains.Item(2));
		Assert.That(adjacents, !Contains.Item(3));
		Assert.That(adjacents, !Contains.Item(4));
		
		adjacents = graph.GetAdjacents(2).ToList();
		
		Assert.That(adjacents, Contains.Item(0));
		Assert.That(adjacents, Contains.Item(1));
		Assert.That(adjacents, !Contains.Item(2));
		Assert.That(adjacents, !Contains.Item(3));
		Assert.That(adjacents, !Contains.Item(4));
		
		Assert.That(graph.GetAdjacents(3), Is.Empty);
		Assert.That(graph.GetAdjacents(4), Is.Empty);
	}
	
	[Test]
	public void TestGraphWithInvalidVertices()
	{
		var graph = graphFactory(5);
		Assert.Throws<IndexOutOfRangeException>(() => graph.AddEdge(-1, 6));
		
		// Assert the graph state remains unchanged
		Assert.That(graph.VertexCount, Is.EqualTo(5));
		Assert.That(graph.EdgeCount, Is.EqualTo(0));
	}
	
	[Test]
	public void TestAddSelfLoop()
	{
		var graph = graphFactory(5);
		
		if(graph.SupportsSelfLoops)
		{
			graph.AddEdge(0, 0);

			Assert.That(graph.EdgeCount, Is.EqualTo(1));
		}
		else
		{
			Assert.That(() => graph.AddEdge(0, 0), Throws.ArgumentException);
			Assert.That(graph.EdgeCount, Is.EqualTo(0));
		}
	}

	public void TestAddParallelEdge()
	{
		var graph = graphFactory(5);
		graph.AddEdge(0, 0);

		if (graph.SupportsParallelEdges)
		{
			graph.AddEdge(0, 0);
			Assert.That(graph.EdgeCount, Is.EqualTo(2));
		}
		else
		{
			Assert.That(() => graph.AddEdge(0, 0), Throws.ArgumentException);
			Assert.That(graph.EdgeCount, Is.EqualTo(1));
		}
	}
	
	// Test ContainsEdge
	[Test]
	public void TestContainsEdge()
	{
		var graph = graphFactory(5);
		graph.Add(0, 1);
		
		Assert.That(graph.ContainsEdge(0, 1), Is.True);
		Assert.That(graph.ContainsEdge(1, 0), Is.True);
		
		Assert.That(graph.Contains((0, 1)));
		Assert.That(!graph.Contains((1, 0))); // enumeration does not contain the same edge twice
	}
}

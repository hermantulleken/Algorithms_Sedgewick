using System.Collections.Generic;
using System.Linq;
using Algorithms_Sedgewick.Graphs;
using NUnit.Framework;

namespace UnitTests;

public static class GraphTestFixtureSource
{
	public static Func<int, IGraph>[] TestCases = 
	{
		vertexCount => new GraphWithAdjacentsArrays(vertexCount),
		vertexCount => new GraphWithAdjacentsSet(vertexCount),
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
		Assert.That(graph.Vertices, Has.Exactly(4).Items);
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
}

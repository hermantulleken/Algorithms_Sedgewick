using AlgorithmsSW.Graphs;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class BipartiteTests
{
	[Test]
	public void TestBuildWithNullGraph()
	{
		Assert.That(() => Bipartite.Build(null!), Throws.TypeOf<ArgumentNullException>());
	}
	
	[Test]
	public void TestBuildWithEmptyGraph()
	{
		var emptyGraph = Graph.CreateEmptyGraph(Factory, 0);
		var bipartite = Bipartite.Build(emptyGraph);
		Assert.IsTrue(bipartite.IsBipartite);
	}
	
	[Test]
	public void TestBuildWithSingleVertexGraph()
	{
		var singleVertexGraph = Graph.CreateEmptyGraph(Factory, 1);
		var bipartite = Bipartite.Build(singleVertexGraph);
		Assert.IsTrue(bipartite.IsBipartite);
	}
	
	[Test]
	public void TestBuildWithNoEdgesGraph()
	{
		var emptyGraph = Graph.CreateEmptyGraph(Factory, 4);
		var bipartite = Bipartite.Build(emptyGraph);
		Assert.IsTrue(bipartite.IsBipartite);
	}
	
	[Test]
	public void TestBuildWithBipartiteGraph()
	{
		IGraph bipartiteGraph = new GraphWithAdjacentsLists(4)
		{
			{ 0, 1 },
			{ 0, 3 },
			{ 1, 2 },
			{ 3, 2 },
		};
		
		var result = Bipartite.Build(bipartiteGraph);
		Assert.That(result.IsBipartite, Is.True);
	}

	[Test]
	public void TestBuildWithNonBipartiteGraph()
	{
		IGraph nonBipartiteGraph = new GraphWithAdjacentsLists(3)
		{
			{ 0, 1 },
			{ 1, 2 },
			{ 2, 0 },
		};
		var bipartite = Bipartite.Build(nonBipartiteGraph);
		Assert.That(bipartite.IsBipartite, Is.False);
	}

	[Test]
	public void TestColorGraphWithDisconnectedGraph()
	{
		IGraph disconnectedGraph = new GraphWithAdjacentsLists(6)
		{
			{ 0, 1 },
			{ 2, 3 },
			{ 4, 5 },
		};
		var bipartite = Bipartite.Build(disconnectedGraph);
		Assert.That(bipartite.IsBipartite, Is.True);
	}
	
	[TestCase(3, false)] // Odd cycle
	[TestCase(4, true)] // Even cycle
	public void TestColorGraphWithCyclicGraph(int vertexCount, bool expected)
	{
		IGraph cyclicGraph = Graph.CreateCyclicGraph(Factory, vertexCount);
		var bipartite = Bipartite.Build(cyclicGraph);
		Assert.That(bipartite.IsBipartite, Is.EqualTo(expected));
	}

	[Test]
	public void TestColorGraphWithCompleteBipartiteGraph()
	{
		var completeBipartiteGraph = Graph.CreateCompleteBipartiteGraph(n => new GraphWithAdjacentsLists(n), 3, 3);
		var bipartite = Bipartite.Build(completeBipartiteGraph);
		Assert.That(bipartite.IsBipartite, Is.True);
	}
	
	private static IGraph Factory(int n) => new GraphWithAdjacentsLists(n);
}

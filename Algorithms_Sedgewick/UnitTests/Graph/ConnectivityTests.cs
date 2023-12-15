using AlgorithmsSW.Graphs;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class ConnectivityTests
{
	private GraphWithAdjacentsCounters graph;
	private Connectivity connectivity;

	[SetUp]
	public void Setup()
	{
		// Initialize a graph with a specific number of vertices.
		graph = new GraphWithAdjacentsCounters(5)
		{
			{ 0, 1 },
			{ 1, 2 },
		};

		graph.AddEdge(0, 1);
		graph.AddEdge(1, 2);

		connectivity = new Connectivity(graph);
	}

	[Test]
	public void TestIsConnected()
	{
		Assert.IsFalse(connectivity.IsConnected);
	}

	[Test]
	public void TestAreConnected()
	{
		Assert.IsTrue(connectivity.AreConnected(0, 2));
	}

	[Test]
	public void TestComponentCount()
	{
		// Vertexes 3, 4 are isolated and contribute to the component count.
		Assert.AreEqual(3, connectivity.ComponentCount);
	}

	[Test]
	public void TestGetShortestPathBetween()
	{
		var path = connectivity.GetShortestPathBetween(0, 2);
		CollectionAssert.AreEqual(new[] { 0, 1, 2 }, path);
	}
}

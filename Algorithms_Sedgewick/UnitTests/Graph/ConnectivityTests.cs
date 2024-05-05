namespace UnitTests;

using AlgorithmsSW.Graph;
using NUnit.Framework.Legacy;

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
		Assert.That(connectivity.IsConnected, Is.False);
	}

	[Test]
	public void TestAreConnected()
	{
		Assert.That(connectivity.AreConnected(0, 2));
	}

	[Test]
	public void TestComponentCount()
	{
		// Vertexes 3, 4 are isolated and contribute to the component count.
		Assert.That(connectivity.ComponentCount, Is.EqualTo(3));
	}

	[Test]
	public void TestGetShortestPathBetween()
	{
		var path = connectivity.GetShortestPathBetween(0, 2);
		Assert.That(path, Is.EqualTo(new[] { 0, 1, 2 }).AsCollection);
	}
}

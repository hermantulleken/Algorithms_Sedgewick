namespace UnitTests;

using System.Collections.Generic;
using System.Linq;
using AlgorithmsSW.EdgeWeightedDigraph;
using NUnit.Framework;
using Support;

[TestFixture]
public class KShortestPathsTests
{
	private EdgeWeightedDigraphWithAdjacencyLists<double> graph;

	[SetUp]
	public void Setup()
	{
		// Initialize your graph here
		graph = new(5, Comparer<double>.Default);
		graph.AddEdge(new(0, 1, 1.0));
		graph.AddEdge(new(0, 2, 2.5));
		graph.AddEdge(new(1, 2, 1.0));
		graph.AddEdge(new(1, 3, 3.0));
		graph.AddEdge(new(2, 3, 1.0));
		graph.AddEdge(new(3, 4, 2.0));
	}

	[Test]
	public void KShortestPaths_FirstShortestPath()
	{
		var ksp = new KShortestPaths<double>(graph, 0, 4, 2, 0.0, (x, y) => x + y);
		var path = ksp.GetPath(0).ToList();
		var distance = ksp.GetDistance(0);
		
		Console.WriteLine(ksp.GetPath(0).Pretty());
//		Console.WriteLine(ksp.GetPath(1).Pretty());

		Assert.AreEqual(new[] { 0, 1, 2, 3, 4 }, path);
		Assert.AreEqual(5.0, distance);
	}

	[Test]
	public void KShortestPaths_SecondShortestPath()
	{
		var ksp = new KShortestPaths<double>(graph, 0, 4, 2, 0.0, (x, y) => x + y);
		var path = ksp.GetPath(1).ToList();
		var distance = ksp.GetDistance(1);

		Assert.AreEqual(new[] { 0, 2, 3, 4 }, path);
		Assert.AreEqual(5.5, distance);
	}

	[Test]
	public void KShortestPaths_NoPathExists()
	{
		var edge = graph.GetUniqueEdge(3, 4);
		graph.RemoveEdge(edge); // Remove edge to make no path
		var ksp = new KShortestPaths<double>(graph, 0, 4, 1, 0.0, (x, y) => x + y);

		Assert.Throws<InvalidOperationException>(() => ksp.GetPath(0));
		graph.AddEdge(edge);
	}

	[Test]
	public void KShortestPaths_RequestingNonExistentPath()
	{
		var ksp = new KShortestPaths<double>(graph, 0, 4, 1, 0.0, (x, y) => x + y);

		Assert.Throws<InvalidOperationException>(() => ksp.GetPath(1));
	}
}

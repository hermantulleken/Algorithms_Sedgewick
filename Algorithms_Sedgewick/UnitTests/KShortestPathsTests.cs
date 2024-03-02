namespace UnitTests;

using System.Collections.Generic;
using System.Linq;
using AlgorithmsSW.EdgeWeightedDigraph;
using AlgorithmsSW.EdgeWeightedGraph;


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
	[TestCaseSource(nameof(GetAlgorithms))]
	public void KShortestPaths_FirstShortestPath(Func<EdgeWeightedDigraphWithAdjacencyLists<double>, int, int, int, IKShortestPaths<double>> findPath)
	{
		var ksp = findPath(graph, 0, 4, 2);
		var path = ksp.GetPath(0);
		var vertexes = path.Vertexes.ToList();
		var distance = path.Distance;
		
		Assert.AreEqual(new[] { 0, 1, 2, 3, 4 }, vertexes);
		Assert.AreEqual(5.0, distance);
	}

	[Test]
	[TestCaseSource(nameof(GetAlgorithms))]
	public void KShortestPaths_SecondShortestPath(Func<EdgeWeightedDigraphWithAdjacencyLists<double>, int, int, int, IKShortestPaths<double>> findPath)
	{
		var ksp = findPath(graph, 0, 4, 2);
		var path = ksp.GetPath(1);
		var vertexes = path.Vertexes.ToList();
		double distance = path.Distance;

		Assert.AreEqual(new[] { 0, 2, 3, 4 }, vertexes);
		Assert.AreEqual(5.5, distance);
	}

	[Test]
	[TestCaseSource(nameof(GetAlgorithms))]
	public void KShortestPaths_NoPathExists(Func<EdgeWeightedDigraphWithAdjacencyLists<double>, int, int, int, IKShortestPaths<double>> findPath)
	{
		var edge = graph.GetUniqueEdge(3, 4);
		graph.RemoveEdge(edge); // Remove edge to make no path
		var ksp = findPath(graph, 0, 4, 1);

		Assert.Throws<InvalidOperationException>(() => ksp.GetPath(0));
		graph.AddEdge(edge);
	}

	[Test]
	[TestCaseSource(nameof(GetAlgorithms))]
	public void KShortestPaths_RequestingNonExistentPath(
		Func<EdgeWeightedDigraphWithAdjacencyLists<double>, int, int, int, IKShortestPaths<double>> findPath)
	{
		var ksp = findPath(graph, 0, 4, 1);

		Assert.Throws<InvalidOperationException>(() => ksp.GetPath(1));
	}

	private static IEnumerable<Func<EdgeWeightedDigraphWithAdjacencyLists<double>, int, int, int, IKShortestPaths<double>>> GetAlgorithms()
	{
		yield return (graph, source, target, k) => new KShortestPaths<double>(graph, source, target, k);
		yield return (graph, source, target, k) => new YensAlgorithm<double>(graph, source, target, k, 0.0, double.MaxValue, (x, y) => x + y);
	}
}

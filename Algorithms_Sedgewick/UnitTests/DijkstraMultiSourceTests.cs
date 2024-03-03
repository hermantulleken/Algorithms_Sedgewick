namespace UnitTests;

using System.Linq;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;
using NUnit.Framework;

public class DijkstraMultiSourceTests
{
	[Test]
	public void TestTwoSourcesOneSink()
	{
		var graph = DataStructures.EdgeWeightedDigraph<double>(3);
		graph.AddEdge(0, 2, 1.0);
		graph.AddEdge(1, 2, 2.0);

		var algorithm = new DijkstraMultiSource(graph, [0, 1]);
		
		Assert.That(algorithm.PathExists(2));
		Assert.That(algorithm.GetPath(2).Vertexes.SequenceEqual([0, 2]));
	}
}

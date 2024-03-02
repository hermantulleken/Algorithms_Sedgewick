namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

public class CriticalEdges
{
	public void TestSimpleCase()
	{
		var graph = DataStructures.EdgeWeightedDigraph(3, Comparer<double>.Default);
		
		graph.AddEdge(0, 1, 6.0);
		graph.AddEdge(1, 2, 4.0);
		graph.AddEdge(1, 2, 5.0);

		var algorithm = new CriticalEdgesExamineShortestPath<double>(graph, 0, 2, (x, y) => x + y, 0.0, double.PositiveInfinity);
		
		Assert.That(algorithm.HasCriticalEdge, Is.True);
		Assert.That(algorithm.CriticalEdge!.Source, Is.EqualTo(1));
		Assert.That(algorithm.CriticalEdge.Target, Is.EqualTo(2));
		Assert.That(algorithm.CriticalEdge.Weight, Is.EqualTo(4.0));
	}
}

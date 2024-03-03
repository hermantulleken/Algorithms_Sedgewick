namespace UnitTests;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

public class CriticalEdges
{
	public void TestSimpleCase()
	{
		var graph = DataStructures.EdgeWeightedDigraph<double>(3);
		
		graph.AddEdge(0, 1, 6.0);
		graph.AddEdge(1, 2, 4.0);
		graph.AddEdge(1, 2, 5.0);

		var algorithm = new CriticalEdgesExamineShortestPath<double>(graph, 0, 2);
		
		Assert.That(algorithm.HasCriticalEdge, Is.True);
		Assert.That(algorithm.CriticalEdge!.Source, Is.EqualTo(1));
		Assert.That(algorithm.CriticalEdge.Target, Is.EqualTo(2));
		Assert.That(algorithm.CriticalEdge.Weight, Is.EqualTo(4.0));
	}
}

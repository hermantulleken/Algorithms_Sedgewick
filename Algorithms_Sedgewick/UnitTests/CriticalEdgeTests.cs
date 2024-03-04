namespace UnitTests;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

public class CriticalEdgeTests
{
	/*
		0--(6.0)--1--(4.0)--2
					\-(5.0)-/
					
		 /------(3.0)------\
		0--(1.0)--1--(1.0)--2--(1.0)--3
		           \------(4.0)------/
		
	*/
	[TestCase("0,1,6.0;1,2,4.0;1,2,5.0", 0, 2, 1)]
	[TestCase("0,2,3.0;0,1,0.1;1,2,1.0;2,3,1.0;1,3,4.0", 0, 3, 3)]
	public void Test_CriticalEdgesExamineShortestPath(string edgeString, int source, int target, int criticalEdgeIndex)
	{
		Func<IEdgeWeightedDigraph<double>, ICriticalEdge<double>> factory = 
			(graph) => new CriticalEdgesExamineShortestPath<double>(graph, source, target);
		
		Test2(edgeString, criticalEdgeIndex, factory);
	}
	
	[TestCase("0,1,6.0;1,2,4.0;1,2,5.0", 0, 2, 1)]
	[TestCase("0,2,3.0;0,1,0.1;1,2,1.0;2,3,1.0;1,3,4.0", 0, 3, 3)]
	public void Test_CriticalEdgesExamineIntersectingShortestPaths(string edgeString, int source, int target, int criticalEdgeIndex)
	{
		Func<IEdgeWeightedDigraph<double>, ICriticalEdge<double>> factory = 
			(graph) => new CriticalEdgesExamineIntersectingShortestPaths<double>(graph, source, target);
		
		Test2(edgeString, criticalEdgeIndex, factory);
	}
	
	public void Test2(
		string edgeString, 
		int criticalEdgeIndex,
		Func<IEdgeWeightedDigraph<double>, ICriticalEdge<double>> factory)
	{
		var edges = edgeString.ParseEdges<double>();
		var graph = edges.ToDigraph();
		var algorithm = factory(graph);
		var criticalEdge = edges[criticalEdgeIndex];
		
		Assert.That(algorithm.HasCriticalEdge, Is.True);
		Assert.That(algorithm.CriticalEdge.Source, Is.EqualTo(criticalEdge.source));
		Assert.That(algorithm.CriticalEdge.Target, Is.EqualTo(criticalEdge.target));
		Assert.That(algorithm.CriticalEdge.Weight, Is.EqualTo(criticalEdge.weight));
	}
}

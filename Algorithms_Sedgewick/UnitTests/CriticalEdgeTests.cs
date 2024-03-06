namespace UnitTests;

using System.Linq;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

[TestFixture(typeof(CriticalEdgesExamineShortestPath<double>))]
[TestFixture(typeof(CriticalEdgesExamineIntersectingShortestPaths<double>))]
public class CriticalEdgeTests<TAlgorithm>
	where TAlgorithm : ICriticalEdge<double>
{
	/*
		0--(6.0)--1--(4.0)--2--(0)--3
					\-(5.0)-4--(0)-/
	*/
	private const string SimpleGraph = "0,1,6.0;" +
										"1,2,4.0;" +
										"2,3,0.0;" +
										"1,4,5.0;" +
										"4,3,0";

	/*
		 /------(3.0)------\
		0--(1.0)--1--(1.0)--2--(1.0)--3
				   \------(4.0)------/
	*/
	private const string ComplexGraph = "0,2,3.0;" +
										"0,1,0.1;" +
										"1,2,1.0;" +
										"2,3,1.0;" +
										"1,3,4.0";

	private ICriticalEdge<double> GetAlgorithm(IEdgeWeightedDigraph<double> graph, int source, int target)
	{
		return typeof(TAlgorithm) switch
		{
			_ when typeof(TAlgorithm) == typeof(CriticalEdgesExamineShortestPath<double>) => new CriticalEdgesExamineShortestPath<double>(graph, source, target),
			_ when typeof(TAlgorithm) == typeof(CriticalEdgesExamineIntersectingShortestPaths<double>) => new CriticalEdgesExamineIntersectingShortestPaths<double>(graph, source, target),
			_ => throw new InvalidOperationException($"The type {typeof(TAlgorithm)} is not supported."),
		};
	}

	[TestCase(SimpleGraph, 0, 3, new[] { 1, 2 })]
	[TestCase(ComplexGraph, 0, 3, new[] { 3 })]
	public void Test(string edgeString, int source, int target, int[] criticalEdgeIndexes)
	{
		var edges = edgeString.ParseEdges<double>();
		var graph = edges.ToDigraph();
		var algorithm = GetAlgorithm(graph, source, target);
		var criticalEdges = criticalEdgeIndexes.Select(index => edges[index]);
		
		Assert.That(algorithm.CriticalEdges.Count(), Is.EqualTo(criticalEdgeIndexes.Length));
		Assert.That(algorithm.CriticalEdges.Select(e => (e.Source, e.Target, e.Weight)), Is.EquivalentTo(criticalEdges));
		
	}
}

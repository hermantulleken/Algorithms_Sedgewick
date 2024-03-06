namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.Digraph;
using AlgorithmsSW.EdgeWeightedDigraph;
using BenchmarkDotNet.Attributes;
using RandomGraph = AlgorithmsSW.EdgeWeightedDigraph.RandomGraph;

public class CriticalEdgesBenchmarks
{
	[Params(10, 20, 40)]
	public int VertexCount { get; set; }
	
	[Params(0.2, 0.5, 0.7)]
	public double EdgeFraction { get; set; }
	
	[Benchmark]
	public object CriticalEdgesExamineShortestPath()
	{
		var graph = MakeGraph((VertexCount, EdgeFraction));
		CriticalEdgesExamineShortestPath<double> algorithm = new(graph, 0, 1);
		
		return algorithm.CriticalEdges;
	}
	
	[Benchmark]
	public object CriticalEdgesExamineIntersectingShortestPaths()
	{
		var graph = MakeGraph((VertexCount, EdgeFraction));
		CriticalEdgesExamineIntersectingShortestPaths<double> algorithm = new(graph, 0, 1);
		
		return algorithm.CriticalEdges;
	}
	
	private IEdgeWeightedDigraph<double> MakeGraph((int vertexCount, double edgeFraction) parameters)
	{
		int edgeCount = (int) (parameters.vertexCount * (parameters.vertexCount - 1) * parameters.edgeFraction/2);
		var graph = AlgorithmsSW.Digraph.RandomGraph.RandomSimple(parameters.vertexCount, edgeCount);
		
		graph.ConnectComponents();
		var weights = Generator.UniformRandomDouble(1.0, 10.0);
		return RandomGraph.AssignWeights(graph, weights);
	}
}

namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.Digraph;
using AlgorithmsSW.EdgeWeightedDigraph;
using BenchmarkDotNet.Attributes;
using RandomGraph = AlgorithmsSW.EdgeWeightedDigraph.RandomGraph;

public class CriticalEdgesBenchmarks
{
	[Params(100, 200, 400)]
	public int VertexCount { get; set; }
	
	[Params(0.2, 0.5, 0.7)]
	public double EdgeFraction { get; set; }
	
	private Comparer<double> Comparer = Comparer<double>.Default;
	
	
	public CriticalEdgesBenchmarks()
	{
	}
	
	[Benchmark]
	public void KruskalMstDWayHeap()
	{
		var graph = MakeGraph((VertexCount, EdgeFraction));
		CriticalEdgesExamineShortestPath<double> algorithm = new(graph, 0, 1, (x, y) => x + y, 0, double.PositiveInfinity);
	}
	
	private IEdgeWeightedDigraph<double> MakeGraph((int vertexCount, double edgeFraction) parameters)
	{
		int edgeCount = (int) (parameters.vertexCount * (parameters.vertexCount - 1) * parameters.edgeFraction/2);
		var graph = AlgorithmsSW.Digraph.RandomGraph.RandomSimple(parameters.vertexCount, edgeCount);
		
		graph.ConnectComponents();
		var weights = Generator.UniformRandomDouble(1.0, 10.0);
		return RandomGraph.AssignWeights(graph, weights, Comparer);
	}
}

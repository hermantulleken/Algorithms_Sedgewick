namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedGraph;
using AlgorithmsSW.Graph;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using RandomGraph = AlgorithmsSW.Graph.RandomGraph;

[SimpleJob(
	RunStrategy.Throughput, 
	iterationCount: 50, 
	launchCount: 1, 
	warmupCount: 1, 
	invocationCount: 1,
	id: "Quick")]
public class HeapBasedMstAlgorithms
{
	private static readonly IComparer<double> Comparer = Comparer<double>.Default;
	private readonly Cache<(int vertexCount, double edgeFraction), IEdgeWeightedGraph<double>> graphCache;
	
	[Params(100, 200, 400)]
	public int VertexCount { get; set; }
	
	[Params(0.2, 0.5, 0.7)]
	public double EdgeFraction { get; set; }
	
	[Params(2, 3, 4, 7, 8, 13)]
	public int HeapDegree { get; set; }
	
	public HeapBasedMstAlgorithms()
	{
		graphCache = new(MakeGraph, Comparer<(int vertexCount, double edgeFraction)>.Default);
	}
	
	[Benchmark]
	public void KruskalMstDWayHeap()
	{
		var graph = graphCache[(VertexCount, EdgeFraction)];
		var algorithm = new KruskalMstDWayHeap<double>(graph, HeapDegree);
		graphCache.Clear(); // This is a temporary measure since the caching is not 100 correct and give inconsistent results
	}
	
	[Benchmark]
	public void LazyPrimMstDWayHeap()
	{
		var graph = graphCache[(VertexCount, EdgeFraction)];
		var algorithm = new LazyPrimMstDWayHeap<double>(graph, HeapDegree);
		graphCache.Clear();
	}
	
	private IEdgeWeightedGraph<double> MakeGraph((int vertexCount, double edgeFraction) parameters)
	{
		int edgeCount = (int) (parameters.vertexCount * (parameters.vertexCount - 1) * parameters.edgeFraction/2);
		var graph = RandomGraph.GetRandomSimpleGraph2(parameters.vertexCount, edgeCount);
		
		graph.ConnectComponents();
		var weights = Generator.UniformRandomDouble(1.0, 10.0);
		return AlgorithmsSW.EdgeWeightedGraph.RandomGraph.AssignWeights(graph, weights, Comparer);
	}
}

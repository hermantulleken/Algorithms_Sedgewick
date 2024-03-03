namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.Digraph;
using AlgorithmsSW.EdgeWeightedDigraph;
using BenchmarkDotNet.Attributes;
using RandomGraph = AlgorithmsSW.EdgeWeightedDigraph.RandomGraph;

/*[SimpleJob(
	RunStrategy.Throughput, 
	iterationCount: 2, 
	launchCount: 2, 
	warmupCount: 2, 
	invocationCount: 2,
	id: "Quick")]*/
public class BellmanFordBenchmarks
{
	private IEdgeWeightedDigraph<double>? graph;

	[Params(1000, 2000, 4000)] // Example graph sizes
	public int VertexCount { get; set; }
	
	[Params(.95)]
	public double EdgeFraction { get; set; } 
	
	[IterationSetup]
	public void MakeGraph()
	{
		Write(nameof(MakeGraph));
		
		// I moved the div by two to divide the double first
		int edgeCount = (int)(EdgeFraction / 2 * (VertexCount * (VertexCount - 1)));
		graph = RandomGraph.AddRandomWeights(
			MakeConnected(AlgorithmsSW.Digraph.RandomGraph.RandomSimple),
			VertexCount, 
			edgeCount, 
			Generator.UniformRandomDouble(1, 10));
	}
	
	public static Func<int, int, IDigraph> MakeConnected(Func<int, int, IDigraph> graphFactory)
	{
		return (vertexCount, edgeCount) =>
		{
			var graph = graphFactory(vertexCount, edgeCount);
			graph.ConnectComponents();
			return graph;
		};
	}

	[Benchmark]
	public bool BellmanFord()
	{
		Write(nameof(BellmanFord));
		
		// Since the graph is randomly generated, we can simply use vertex 0 and 1 each time. 
		var algorithm = new BellmanFord<double>(graph, 0);
		return algorithm.HasPathTo(1);
	}

	[Benchmark]
	public bool BellmanFordWithParentCheckingHeuristic()
	{
		Write(nameof(BellmanFordWithParentCheckingHeuristic));
		
		var algorithm = new BellmanFordWithParentCheckingHeuristic<double>(graph, 0);
		return algorithm.HasPathTo(1);
	}
	
	private static double Add(double x, double y) => x + y;

	private static void Write(string message)
	{
		//Console.WriteLine("<<< " + message);
	}
}

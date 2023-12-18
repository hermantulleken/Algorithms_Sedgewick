namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedGraph;
using AlgorithmsSW.Graph;
using BenchmarkDotNet.Attributes;
using RandomGraph = AlgorithmsSW.Graph.RandomGraph;

public class MstAlgorithms
{
	private readonly IComparer<double> Comparer = Comparer<double>.Default;

	private readonly int[] VertexCounts = [10, 50, 100]; //10 .. 1000

    private readonly IEnumerable<int>[] vertexGenerators;
	private readonly IEnumerable<double>[] weightGenerators;
	
	[Params(0.3)]
	public double EdgeFraction { get; set; }
	
	[Params(1)]
	public int ParameterIndex { get; set; }
	
	public MstAlgorithms()
	{
		vertexGenerators = VertexCounts.Select(Generator.UniformRandomInt).ToArray();
        weightGenerators = new[] { 0, 1, 2 }.Select(i => Generator.UniformRandomDouble(1, 10.0)).ToArray();
	}
	
	private IEdgeWeightedGraph<double> MakeGraph(int index, double edgeFraction)
	{
		int vertexCount = VertexCounts[index];
		int edgeCount = (int) (vertexCount * (vertexCount - 1) * edgeFraction/2);
		var graph = RandomGraph.GetRandomSimpleGraph2(VertexCounts[index], edgeCount);
		
		graph.ConnectComponents();
		
		var weights = weightGenerators[index];
		return AlgorithmsSW.EdgeWeightedGraph.RandomGraph.AssignWeights(graph, weights, Comparer);
	}

	//[Benchmark]
	public void LazyPrimMst()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var algorithm = new LazyPrimMst<double>(graph);
	}
	
	[Benchmark]
	public void PrimMst()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var algorithm = new PrimMst<double>(graph, 0, double.MaxValue);
	}
	
	//[Benchmark]
	public void KruskalMst()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var algorithm = new KruskalMst<double>(graph);
	}
	
	//[Benchmark]
	public void MstVyssotsky()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var mst = Mst.MstVyssotsky_Old(graph);
	}
	
	[Benchmark]
	public void MstVyssotsky2()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var mst = Mst.MstVyssotsky_Old2(graph);
	}
	
	[Benchmark]
	public void MstVyssotsky3()
	{
		var graph = MakeGraph(ParameterIndex, EdgeFraction);
		var vysstotsky = new Vyssotsky();
		vysstotsky.minimumSpanningTree(graph, double.MinValue);
	}
}

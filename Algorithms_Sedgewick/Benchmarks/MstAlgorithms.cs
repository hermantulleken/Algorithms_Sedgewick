using Support;

namespace Benchmarks;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedGraph;
using AlgorithmsSW.Graph;
using BenchmarkDotNet.Attributes;
using RandomGraph = AlgorithmsSW.Graph.RandomGraph;

public struct Parameter<T> : IComparable<Parameter<T>>
{
	public T Value;
	public string Name;

	public override string ToString() => Name;

	public int CompareTo(Parameter<T> other)
	{
		return Value switch
		{
			IComparable comparableValue when other.Value is IComparable otherValue => comparableValue.CompareTo(otherValue),
			_ => string.Compare(Name, other.Name, StringComparison.Ordinal)
		};
	}
}

public static class ParameterExtensions
{
	public static Parameter<T> Name<T, TName>(this T obj, TName name)=> new() { Value = obj, Name = name.AsText() };
}

public class MstAlgorithms
{
	private static readonly IComparer<double> Comparer = Comparer<double>.Default;

	private static readonly int[] VertexCounts = [10, 50, 200];

	private readonly IEnumerable<int>[] vertexGenerators;
	private readonly IEnumerable<double>[] weightGenerators;
	
	[Params(/*0.2, 0.5,*/ 0.7)]
	public double EdgeFraction { get; set; }
	
	[ParamsSource(nameof(ParameterIndexSource))]
	public Parameter<int> ParameterIndex { get; set; }

	public IEnumerable<Parameter<int>> ParameterIndexSource { get; } = new List<Parameter<int>>
	{
		//0.Name(VertexCounts[0]),
		//1.Name(VertexCounts[1]),
		2.Name(VertexCounts[2])
	};
	
	public MstAlgorithms()
	{
		vertexGenerators = VertexCounts.Select(Generator.UniformRandomInt).ToArray();
		weightGenerators = new[] { 0, 1, 2 }.Select(_ => Generator.UniformRandomDouble(1, 10.0)).ToArray();
	}
	
	private IEdgeWeightedGraph<double> MakeGraph(int index, double edgeFraction)
	{
		int vertexCount = VertexCounts[index];
		int edgeCount = (int) (vertexCount * (vertexCount - 1) * edgeFraction/2);
		var graph = RandomGraph.GetRandomSimpleGraph2(VertexCounts[index], edgeCount);
		
		graph.ConnectComponents();
		
		var weights = weightGenerators[index];
		return AlgorithmsSW.EdgeWeightedGraph.RandomGraph.AssignWeights(graph, weights);
	}

	[Benchmark]
	public void LazyPrimMst()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new LazyPrimMst<double>(graph);
	}
	
	[Benchmark]
	public void PrimMst()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new PrimMst<double>(graph, 0, double.MaxValue);
	}
	
	[Benchmark]
	public void KruskalMst()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new KruskalMst<double>(graph);
	}
	
	[Benchmark]
	public void MstVyssotsky()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var vysstotsky = new Vyssotsky<double>(graph, double.MinValue);
	}
	
	[Benchmark]
	public void MstBoruvka()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new BoruvkasAlgorithm<double>(graph);
	}
	
	[Benchmark]
	public void BoruvkasAlgorithmImproved()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new BoruvkasAlgorithmImproved<double>(graph);
	}
	
	[Benchmark]
	public void BoruvkasAlgorithmImprovement2()
	{
		var graph = MakeGraph(ParameterIndex.Value, EdgeFraction);
		var algorithm = new BoruvkasAlgorithmImprovement2<double>(graph);
	}
}

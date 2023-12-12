using Algorithms_Sedgewick;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class UniqueRandomInt
{
	[Params(10, 100, 1_000, 10_000)]
	public int MaxValue {get; set; }
	
	
	[Params(0.05f, 0.1f, 0.15f, 0.2f)]
	public float Fraction { get; set; }

	[Benchmark]
	public int UniqueUniformRandomInt_WithShuffledList()
	{
		int count = (int) (MaxValue * Fraction);
		var list = Generator.UniqueUniformRandomInt_WithShuffledList(MaxValue, count);
		return list[0];
	}
	
	[Benchmark]
	public int UniqueUniformRandomInt_WithSet()
	{
		int count = (int) (MaxValue * Fraction);
		var list = Generator.UniqueUniformRandomInt_WithSet(MaxValue, count);
		return list[0];
	}
}

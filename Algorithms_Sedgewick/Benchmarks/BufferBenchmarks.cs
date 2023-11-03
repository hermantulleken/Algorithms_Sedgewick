using Algorithms_Sedgewick;
using Algorithms_Sedgewick.Buffer;
using Algorithms_Sedgewick.List;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;

[MedianColumn]
[RPlotExporter]
[SimpleJob(RunStrategy.ColdStart, launchCount: 5, iterationCount: 1)] 
public class BufferBenchmarks
{
	private readonly ResizeableArray<float> list;

	public BufferBenchmarks()
	{
		int range = 100000;
		int count = 100000000;
		
		list = 
			Generator
				.UniformRandomInt(range)
				.Take(count)
				.Select(n => n / (float) range)
				.ToResizableArray(count);
	}
	
	public static void CalcAverageDistance(IBuffer<float> buffer, ResizeableArray<float> values)
	{
		float differenceSum = 0;
			
		foreach (float f in values)
		{
			buffer.Insert(f);

			if (buffer.Count == 2)
			{
				float difference = buffer.Last - buffer.First;
				differenceSum += difference;
			}
		}
			
		Console.WriteLine(differenceSum / (values.Count - 1));
	}

	[Benchmark]
	public void Capacity2Buffer() => CalcAverageDistance(new Capacity2Buffer<float>(), list);
	
	[Benchmark]
	public void RingBuffer() => CalcAverageDistance(new RingBuffer<float>(2), list);
	
	[Benchmark]
	public void OptimizedCapacity2Buffer() => CalcAverageDistance(new OptimizedCapacity2Buffer<float>(), list);
}

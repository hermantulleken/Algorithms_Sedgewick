using AlgorithmsSW;
using AlgorithmsSW.List;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks;

[HtmlExporter]
[MedianColumn]
[SimpleJob(RunStrategy.Throughput, launchCount: 1, iterationCount: 5)]
public class LinkedListBenchMarks
{
	private const int Count = 1 << 18;
	
	private ResizeableArray<int> list;
	private readonly LinkedListWithPooledNodes<int> targetList2;
	private readonly AlgorithmsSW.List.LinkedList<int> targetList1;
	private readonly LinkedListWithPooledClassNodes<int> targetList3;

	[Params( /*Count / 64, Count / 32, Count / 16, Count / 8, Count / 4, */Count / 2, Count)]
	public int ItemCount { get; set; }

	public LinkedListBenchMarks()
	{
		targetList2 = new LinkedListWithPooledNodes<int>(Count);
		targetList1 = new AlgorithmsSW.List.LinkedList<int>();
		targetList3 = new LinkedListWithPooledClassNodes<int>(Count);
	}

	[Benchmark]
	public void Add1()
	{
		ResetList();

		foreach (int item in list)
		{
			targetList1.InsertAtBack(item);
		}
		
		while (!targetList1.IsEmpty)
		{
			targetList1.RemoveFromFront();
		}
	}
	
	[Benchmark]
	public void Add2()
	{
		ResetList();
		
		
		foreach (int item in list)
		{
			targetList2.InsertAtBack(item);
		}

		while (!targetList2.IsEmpty)
		{
			targetList2.RemoveFromFront();
		}
	}
	
	[Benchmark]
	public void Add3()
	{
		ResetList();
		
		foreach (int item in list)
		{
			targetList3.InsertAtBack(item);
		}

		while (!targetList3.IsEmpty)
		{
			targetList3.RemoveFromFront();
		}
	}
	
	[Benchmark]
	public void AddClear1()
	{
		ResetList();

		foreach (int item in list)
		{
			targetList1.InsertAtBack(item);
		}
		
		targetList1.Clear();
	}
	
	[Benchmark]
	public void AddClear2()
	{
		ResetList();
		
		
		foreach (int item in list)
		{
			targetList2.InsertAtBack(item);
		}

		targetList2.Clear();
	}
	
	[Benchmark]
	public void AddClear3()
	{
		ResetList();
		
		foreach (int item in list)
		{
			targetList3.InsertAtBack(item);
		}

		targetList3.Clear();
	}
	
	
	private void ResetList()
		=> list = Generator.UniformRandomInt(int.MaxValue)
			.Take(ItemCount)
			.ToResizableArray(ItemCount);
}

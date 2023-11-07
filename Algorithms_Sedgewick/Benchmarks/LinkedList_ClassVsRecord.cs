using Algorithms_Sedgewick.List;
using BenchmarkDotNet.Attributes;

using LinkedList = Algorithms_Sedgewick.List.LinkedList<int>;

namespace Benchmarks;

public class LinkedList_ClassVsRecord
{
	private const int Iterations = 100_000;

	[Benchmark]
	public void InsertAtBack_Class()
	{
		var list = new LinkedListWithClassNode<int>();
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtBack(i);
		}
	}

	[Benchmark]
	public void InsertAtBack_Record()
	{
		var list = new LinkedList();
		
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtBack(i);
		}
	}
	
	[Benchmark]
	public void InsertAtFront_Class()
	{
		var list = new LinkedListWithClassNode<int>();
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtFront(i);
		}
	}

	[Benchmark]
	public void InsertAtFront_Record()
	{
		var list = new LinkedList();
		
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtFront(i);
		}
	}
	
	[Benchmark]
	public void InsertAtFrontClear_Class()
	{
		var list = new LinkedListWithClassNode<int>();
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtFront(i);
		}
		
		list.Clear();
	}

	[Benchmark]
	public void InsertAtFrontClear_Record()
	{
		var list = new LinkedList();
		
		for (int i = 0; i < Iterations; i++)
		{
			list.InsertAtFront(i);
		}
		
		list.Clear();
	}
}

using System.Text;
using AlgorithmsSW;
using AlgorithmsSW.List;
using AlgorithmsSW.Object;

namespace PerformanceTests;

public class LinkedListWithPooledNodesTest
{
	private LinkedListWithPooledNodes<int> targetList;
	private ResizeableArray<int> list;
	private const int Count = 1000000;
	private const int IterationCount = 1000;

	public LinkedListWithPooledNodesTest()
	{
		targetList = new LinkedListWithPooledNodes<int>(Count);
		list = CreateList();
	}
	public void Run()
	{
		for (int i = 0; i < IterationCount; i++)
		{
			RunIteration();
		}
		
		Console.WriteLine(targetList.Count);
	}

	private void RunIteration()
	{
		foreach (var item in list)
		{
			targetList.InsertAtBack(item);
		}
		
		while (!targetList.IsEmpty)
		{
			targetList.RemoveFromFront();
		}
	}

	private ResizeableArray<int> CreateList()
		=> Generator.UniformRandomInt(int.MaxValue)
			.Take(Count)
			.ToResizableArray(Count);
}

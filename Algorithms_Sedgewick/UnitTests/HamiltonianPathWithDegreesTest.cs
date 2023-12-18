using AlgorithmsSW.Digraph;

using NUnit.Framework;

namespace UnitTests;

[TestFixture, Parallelizable]
public class HamiltonianPathWithDegreesTest
{
	private static readonly Func<IDigraph, IHamiltonianPath>[] Algorithms = new Func<IDigraph, IHamiltonianPath>[]
	{
		digraph => new HamiltonianPathWithDegrees(digraph),
		digraph => new HamiltonianPathWithTopologicalSort(digraph),
	};
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestEmpty(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(0);
		var hamiltonianCycle = algorithm(digraph);
		Assert.True(hamiltonianCycle.HasHamiltonianPath);
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestSingleton(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(1);
		var hamiltonianCycle = algorithm(digraph);
		Assert.True(hamiltonianCycle.HasHamiltonianPath);
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestCycle(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(3)
		{
			(0, 1),
			(1, 2),
			(2, 0),
		};

		Assert.Throws<ArgumentException>(() => { _ = algorithm(digraph); });
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestSelfLoop(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(4)
		{
			(0, 0),
			(0, 1),
			(1, 2),
			(2, 3),
		};

		Assert.Throws<ArgumentException>(() => { _ = algorithm(digraph); });
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestDag(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(3)
		{
			(0, 1),
			(1, 2),
		};

		var hamiltonianCycle = algorithm(digraph);
		Assert.True(hamiltonianCycle.HasHamiltonianPath);
		Assert.AreEqual(new[] { 0, 1, 2 }, hamiltonianCycle.Path);
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestTree(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(4)
		{
			(0, 1),
			(1, 2),
			(1, 3),
		};

		var hamiltonianCycle = algorithm(digraph);
		Assert.False(hamiltonianCycle.HasHamiltonianPath);
		Assert.Throws<InvalidOperationException>(() => _ = hamiltonianCycle.Path);
	}

	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestDisconnected(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(5)
		{
			(0, 1),
			(1, 2),
			(3, 4),
		};
		
		var hamiltonianCycle = algorithm(digraph);
		Assert.False(hamiltonianCycle.HasHamiltonianPath);
		Assert.Throws<InvalidOperationException>(() => _ = hamiltonianCycle.Path);
	}
	
	[Test, TestCaseSource(nameof(Algorithms))]
	public void TestGraphWithParallelEdges(Func<IDigraph, IHamiltonianPath> algorithm)
	{
		var digraph = new DigraphWithAdjacentsLists(3)
		{
			(0, 1),
			(0, 1),
			(1, 2),
		};

		var hamiltonianCycle = algorithm(digraph);
		Assert.True(hamiltonianCycle.HasHamiltonianPath);
	}
}

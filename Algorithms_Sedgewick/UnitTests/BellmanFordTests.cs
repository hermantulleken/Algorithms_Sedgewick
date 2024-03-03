namespace UnitTests;

using System.Collections.Generic;
using System.Diagnostics;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;
using Support;

[TestFixture]
public class BellmanFordTests
{
	[Test]
	public void TestShortestPathCalculation()
	{
		// Arrange
		var graph = DataStructures.EdgeWeightedDigraph<double>(5);
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 3.0);
		graph.AddEdge(0, 3, 6.0);
		graph.AddEdge(3, 2, 2.0);
		graph.AddEdge(2, 4, 5.0);

		var bellmanFord = new BellmanFord<double>(graph, 0);

		// Act
		var distanceTo2 = bellmanFord.GetDistanceTo(2);
		var distanceTo4 = bellmanFord.GetDistanceTo(4);

		// Assert
		Assert.That(distanceTo2, Is.EqualTo(4.0)); // Shortest path 0 -> 1 -> 2
		Assert.That(distanceTo4, Is.EqualTo(9.0)); // Shortest path 0 -> 1 -> 2 -> 4
	}
	
	[Test]
	public void TestShortestPathWithCycle1()
	{
		// Arrange
		var graph = DataStructures.EdgeWeightedDigraph<double>(5);
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 2.0);
		graph.AddEdge(2, 3, 3.0);
		graph.AddEdge(3, 4, 4.0);
		graph.AddEdge(4, 0, 5.0);

		var bellmanFord = new BellmanFord<double>(graph, 0);

		// Act
		var distanceTo2 = bellmanFord.GetDistanceTo(2);
		var distanceTo4 = bellmanFord.GetDistanceTo(4);

		// Assert
		Assert.That(distanceTo2, Is.EqualTo(3.0)); // Shortest path 0 -> 1 -> 2
		Assert.That(distanceTo4, Is.EqualTo(10.0)); // Shortest path 0 -> 1 -> 2 -> 3-> 4
	}
	
	[Test]
	public void TestShortestPathWithCycle2()
	{
		// Arrange
		var graph = DataStructures.EdgeWeightedDigraph<double>(5);
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 2.0);
		graph.AddEdge(2, 3, 3.0);
		graph.AddEdge(3, 4, 4.0);
		graph.AddEdge(4, 0, 5.0);
		graph.AddEdge(0, 4, 6.0);
		graph.AddEdge(4, 3, 7.0);

		var bellmanFord = new BellmanFord<double>(graph, 0);

		// Act
		var distanceTo3 = bellmanFord.GetDistanceTo(3);
		var distanceTo4 = bellmanFord.GetDistanceTo(4);

		// Assert
		Assert.That(distanceTo3, Is.EqualTo(6.0)); // Shortest path 0 -> 1 -> 2
		Assert.That(distanceTo4, Is.EqualTo(6.0)); // Shortest path 0 -> 1 -> 2 -> 3-> 4
	}

	[Test]
	public void TestNoPathScenario()
	{
		// Arrange
		var graph = new EdgeWeightedDigraphWithAdjacencyLists<double>(3);
		graph.AddEdge(0, 1, 1.0);
		// No edge from 1 to 2 or 0 to 2

		var bellmanFord = new BellmanFord<double>(graph, 0);

		// Act & Assert
		Assert.That(bellmanFord.HasPathTo(2), Is.False);
	}

	[Test]
	public void TestNegativeCycleDetection()
	{
		// Arrange
		var graph = new EdgeWeightedDigraphWithAdjacencyLists<double>(3);
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 1.0);
		graph.AddEdge(2, 0, -3.0); // Negative cycle

		var bellmanFord = new BellmanFord<double>(graph, 0);

		// Act
		var hasNegativeCycle = bellmanFord.HasNegativeCycle();

		// Assert
		Assert.That(hasNegativeCycle, Is.True);
	}

	#if WITH_INSTRUMENTATION 
	[Test]
	public void TraceTest()
	{
		/*	This is the example used here:
			https://github.com/reneargento/algorithms-sedgewick-wayne/blob/master/src/chapter4/section4/Exercise32_ParentCheckingHeuristic.txt
			
			You can see the progression of distanceTo and edgeTo if running with WITH_INSTRUMENTATION is defined.
			
			This is not a proper Unity test
		*/
		int s = 0;
		int p = 1;
		int w = 2;
		int v = 3;
		
		var graph = new EdgeWeightedDigraphWithAdjacencyLists<double>(4, Comparer<double>.Default);
		graph.AddEdge(s, p, 2.0);
		graph.AddEdge(p, v, 2.0);
		graph.AddEdge(s, w, 4.0);
		graph.AddEdge(w, p, -3.0);
		
		var algorithm = new BellmanFordWithParentCheckingHeuristic<double>(graph, 0, (a, b) => a + b, 0.0, double.MaxValue);

		var traceList = Tracer.TraceList;
		Console.WriteLine(traceList.Pretty());

		var trace =
			"[" +
				"distanceTo: [0, 2, 4, 1.7976931348623157E+308], edgeTo: [<null>, [0, 1: 2], [0, 2: 4], <null>], " +
				"distanceTo: [0, 2, 4, 4], edgeTo: [<null>, [0, 1: 2], [0, 2: 4], [1, 3: 2]], " +
				"distanceTo: [0, 1, 4, 4], edgeTo: [<null>, [2, 1: -3], [0, 2: 4], [1, 3: 2]], " +
				"distanceTo: [0, 1, 4, 3], edgeTo: [<null>, [2, 1: -3], [0, 2: 4], [1, 3: 2]], " +
				"distanceTo: [0, 1, 4, 3], edgeTo: [<null>, [2, 1: -3], [0, 2: 4], [1, 3: 2]]" +
			"]";
		
		Assert.That(traceList.Pretty(), Is.EqualTo(trace));
	}
	#endif
}

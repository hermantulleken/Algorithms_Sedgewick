using AlgorithmsSW.Graph;

namespace UnitTests;

[TestFixture]
public class GraphAlgorithmsTests
{
	// Test 1: Null Graph
	[Test]
	public void FindNodeSafeToDelete_NullGraph_ThrowsException()
	{
		IGraph nullGraph = null!;
		Assert.Throws<ArgumentNullException>(() => GraphAlgorithms.FindNodeSafeToDelete(nullGraph));
	}

	// Test 2: Simple Graph
	[Test]
	public void FindNodeSafeToDelete_SimpleGraph_ReturnsCorrectNode()
	{
		DynamicGraph simpleGraph = new DynamicGraph();
		simpleGraph.AddVertexes(0);
		int result = GraphAlgorithms.FindNodeSafeToDelete(simpleGraph);
		Assert.That(result, Is.EqualTo(0)); 
	}

	// Test 3: Graph with Isolated Node
	[Test]
	public void FindNodeSafeToDelete_GraphWithIsolatedNode_ThrowsException()
	{
		DynamicGraph graph = new DynamicGraph();
		graph.AddVertexes(0, 1, 2, 3);
		graph.AddEdge(1, 2);
		graph.AddEdge(2, 3);
		graph.AddEdge(1, 3);
		
		Assert.Throws<ArgumentException>(() => GraphAlgorithms.FindNodeSafeToDelete(graph));
	}

	// Test 4: Non-Connected Graph
	[Test]
	public void FindNodeSafeToDelete_NonConnectedGraph_ThrowsException()
	{
		var graph = new DynamicGraph(0, 1, 2, 3, 4)
		{
			{ 0, 1 },
			{ 1, 2 },
			{ 0, 2 },
			{ 3, 4 },
		};
		
		Assert.Throws<ArgumentException>(() => GraphAlgorithms.FindNodeSafeToDelete(graph));
	}

	// Test 5: Connected Graph with No Isolated Nodes
	[Test]
	public void FindNodeSafeToDelete_ConnectedGraphNoIsolatedNodes_ReturnsValidNode()
	{
		var graph = new DynamicGraph(0, 1, 2, 3, 4)
		{
			{ 0, 1 },
			{ 1, 2 },
			{ 0, 2 },
			{ 2, 3 },
			{ 3, 4 },
		};
		
		int result = GraphAlgorithms.FindNodeSafeToDelete(graph);
		graph.RemoveVertex(result);
		var connectivity = new Connectivity(graph); 
		
		Assert.That(connectivity.IsConnected); 
	}

	// Test 7: Graph with Cycles
	[Test]
	public void FindNodeSafeToDelete_GraphWithCycles_ReturnsValidNode()
	{
		var graph = new DynamicGraph(0, 1, 2, 3, 4)
		{
			{ 0, 1 },
			{ 1, 2 },
			{ 0, 2 },
			{ 0, 3 },
			{ 3, 4 },
		};

		int result = GraphAlgorithms.FindNodeSafeToDelete(graph);
		graph.RemoveVertex(result);
		var connectivity = new Connectivity(graph); 
		
		Assert.That(connectivity.IsConnected); 
	}
}

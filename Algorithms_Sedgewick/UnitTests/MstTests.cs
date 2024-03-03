namespace UnitTests;

using System.Collections.Generic;
using System.Linq;
using AlgorithmsSW.EdgeWeightedGraph;

[TestFixture]
public class MstTests
{
	[Datapoints]
	private static readonly Func<IEdgeWeightedGraph<double>, IMst<double>>[] Factories =
	{
		graph => new KruskalMst<double>(graph),
		graph => new PrimMst<double>(graph, double.MinValue, double.MaxValue),
		graph => new LazyPrimMst<double>(graph),
		graph => new Vyssotsky<double>(graph, double.MinValue),
		graph => new BoruvkasAlgorithm<double>(graph),
		graph => new BoruvkasAlgorithmImproved<double>(graph),
		graph => new BoruvkasAlgorithmImprovement2<double>(graph),
		graph => new LazyPrimMstDWayHeap<double>(graph, 5),
		graph => new KruskalMstDWayHeap<double>(graph, 5),
	};
	
	[Test, TestCaseSource(nameof(Factories))]
	public void TestLine(Func<IEdgeWeightedGraph<double>, IMst<double>> factory)
	{
		Edge<double>[] edges =
		[
			new(0, 1, 1.0),
			new(1, 2, 2.0),
			new(2, 3, 3.0),
			new(3, 4, 4.0),
		];

		int[] edgeIndexesInMst = [0, 1, 2, 3];
		int[] edgeIndexesNotInMst = [];

		Test(factory, 5, edges, edgeIndexesInMst, edgeIndexesNotInMst);
	}
	
	[Test, TestCaseSource(nameof(Factories))]
	public void TestTree(Func<IEdgeWeightedGraph<double>, IMst<double>> factory)
	{
		Edge<double>[] edges =
		[
			new(0, 1, 1.0),
			new(1, 2, 2.0),
			new(2, 3, 3.0),
			new(3, 4, 4.0),
			new(0, 5, 5.0),
			new(1, 6, 6.0),
			new(2, 7, 7.0),
			new(3, 8, 8.0),
		];

		int[] edgeIndexesInMst = [0, 1, 2, 3];
		int[] edgeIndexesNotInMst = [];

		Test(factory, 9, edges, edgeIndexesInMst, edgeIndexesNotInMst);
	}
		
	[Test, TestCaseSource(nameof(Factories))]
	public void TestCycle(Func<IEdgeWeightedGraph<double>, IMst<double>> factory)
	{
		Edge<double>[] edges =
		[
			new(0, 1, 1.0),
			new(1, 2, 2.0),
			new(2, 3, 3.0),
			new(3, 4, 4.0),
			new(4, 0, 5.0),
		];

		int[] edgeIndexesInMst = [0, 1, 2, 3];
		int[] edgeIndexesNotInMst = [4];

		Test(factory, 5, edges, edgeIndexesInMst, edgeIndexesNotInMst);
	}
	
	[Test, TestCaseSource(nameof(Factories))]
	public void TestTwoCycles(Func<IEdgeWeightedGraph<double>, IMst<double>> factory)
	{
		Edge<double>[] edges =
		[
			new(0, 1, 1.0),
			new(1, 2, 2.0),
			new(2, 3, 3.0),
			new(3, 4, 4.0),
			new(4, 0, 5.0),
			new(0, 3, 6.0),
		];

		int[] edgeIndexesInMst = [0, 1, 2, 3];
		int[] edgeIndexesNotInMst = [4, 5];

		Test(factory, 5, edges, edgeIndexesInMst, edgeIndexesNotInMst);
	}

	public void Test(Func<IEdgeWeightedGraph<double>, IMst<double>> factory,
		int vertexCount,
		Edge<double>[] edges,
		IEnumerable<int> edgeIndexesInMst,
		IEnumerable<int> edgeIndexesNotInMst)
	{
		var graph = new EdgeWeightedGraphWithAdjacencyLists<double>(vertexCount);

		foreach (var edge in edges)
		{
			graph.AddEdge(edge);
		}
		
		var mst = factory(graph);

		foreach (int edgeIndex in edgeIndexesInMst)
		{
			Assert.That(mst.Edges.Contains(edges[edgeIndex]));
		}
		
		foreach (int edgeIndex in edgeIndexesNotInMst)
		{
			Assert.That(!mst.Edges.Contains(edges[edgeIndex]));
		}
	}
}

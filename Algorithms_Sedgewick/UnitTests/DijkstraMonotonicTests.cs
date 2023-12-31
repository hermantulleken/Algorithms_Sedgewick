namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

[TestFixture]
public class DijkstraMonotonicTests
{
	[Test]
	public void TestSimpleGraph1()
	{
		var graph = DataStructures.EdgeWeightedDigraph(4, Comparer<double>.Default);
		graph.AddEdge(0, 1, 3.0);
		graph.AddEdge(1, 2, 5.0);
		graph.AddEdge(2, 3, 7.0);

		var algorithm = new DijkstraMonotonic(graph, 0);//, int.MaxValue, (x, y) => x + y, 0);
		Assert.That(algorithm.DistTo(0), Is.EqualTo(0));
		Assert.That(algorithm.DistTo(1), Is.EqualTo(3));
		Assert.That(algorithm.DistTo(2), Is.EqualTo(8));
		Assert.That(algorithm.DistTo(3), Is.EqualTo(15));
	}
	
	[Test]
	public void TestSimpleGraph2()
	{
		var graph = DataStructures.EdgeWeightedDigraph(4, Comparer<double>.Default);
		graph.AddEdge(0, 1, 3.0);
		graph.AddEdge(1, 2, 5.0);
		graph.AddEdge(0, 3, 4.0);
		graph.AddEdge(3, 2, 3.0);

		var algorithm = new DijkstraMonotonic(graph, 0);// int.MaxValue, (x, y) => x + y, 0);
		Assert.That(algorithm.DistTo(2), Is.EqualTo(7));
	}
	
	[Test]
	public void TestSimpleGraph3()
	{
		/*
		0--(3)--1--(5)----------\	// Ascending
		\--(4)--3--(3)----------\	// Descending
		\--(2)--4--(1)--5--(1)--2	// Shorter but not monotonic
	*/
		var graph = DataStructures.EdgeWeightedDigraph(6, Comparer<double>.Default);
		graph.AddEdge(0, 1, 3.0);
		graph.AddEdge(1, 2, 5.0);
		graph.AddEdge(0, 3, 4.0);
		graph.AddEdge(3, 2, 3.0);
		graph.AddEdge(0, 4, 1.0);
		graph.AddEdge(4, 5, 2.0);
		graph.AddEdge(5, 2, 1.0);
		
		var algorithm = new DijkstraMonotonic(graph, 0);// int.MaxValue, (x, y) => x + y, 0);
		Assert.That(algorithm.DistTo(2), Is.EqualTo(7));
	}

	[Test]
	public void TestSwitchingAscending()
	{
		var graph = DataStructures.EdgeWeightedDigraph(7, Comparer<double>.Default);
		
		/*	0--(1)--1--(2)--2--(3)--3--(4)--4
		\--(3)--5--(2)-/\--(1)--6--(0)-/

		The shortest monotonic path to 2 is 0->1->2 with weight 3.
		The shortest monotonic path to 4 is 0->5->6->4 with weight 6.
	*/
		
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 2.0);
		graph.AddEdge(2, 3, 3.0);
		graph.AddEdge(3, 4, 4.0);
		graph.AddEdge(0, 5, 3.0);
		graph.AddEdge(5, 2, 2.0);
		graph.AddEdge(2, 6, 1.0);
		graph.AddEdge(6, 4, 0.0);
		
		var algorithm = new DijkstraMonotonic(graph, 0);// int.MaxValue, (x, y) => x + y, 0);
		Assert.That(algorithm.DistTo(2), Is.EqualTo(3.0));
		Assert.That(algorithm.DistTo(4), Is.EqualTo(6.0));
	}
	
	[Test]
	public void TestSubpathsNotShared()
	{
		var graph = DataStructures.EdgeWeightedDigraph(5, Comparer<double>.Default);
		
		/*	0--(1)--1--(7)--2--(6)--3
		\--(4)--4--(5)-/

		The shortest monotonic path to 2 is 0->1->2 with weight 8.
		The shortest monotonic path to 3 is 0->4->5->3 with weight 9.
	*/
		
		graph.AddEdge(0, 1, 1.0);
		graph.AddEdge(1, 2, 7.0);
		graph.AddEdge(2, 3, 6.0);
		graph.AddEdge(0, 4, 4.0);
		graph.AddEdge(4, 2, 5.0);
		
		var algorithm = new DijkstraMonotonic(graph, 0);// int.MaxValue, (x, y) => x + y, 0);
		Assert.That(algorithm.DistTo(2), Is.EqualTo(8.0));
		Assert.That(algorithm.DistTo(3), Is.EqualTo(15.0));
	}

	[Test]
	public void TestWebExample()
	{
		//https://github.com/reneargento/algorithms-sedgewick-wayne/blob/master/src/chapter4/section4/Exercise34_MonotonicShortestPath.java#L5
		
		var graph = DataStructures.EdgeWeightedDigraph(8, Comparer<double>.Default);
		
		graph.AddEdge(new(0, 1, 1));
		graph.AddEdge(new(0, 4, 3));
		graph.AddEdge(new(1, 2, 2));
		graph.AddEdge(new(2, 6, 2));
		graph.AddEdge(new(1, 5, 0));
		graph.AddEdge(new(5, 0, 3));
		graph.AddEdge(new(1, 4, 1));
		graph.AddEdge(new(1, 3, 2));
		graph.AddEdge(new(1, 3, 0));
		graph.AddEdge(new(2, 7, 1));
		
		var algorithm = new DijkstraMonotonic(graph, 0);// int.MaxValue, (x, y) => x + y, 0);
		
		for (int vertex = 0; vertex < graph.VertexCount; vertex++) {
			Console.Write("\nPath from vertex 0 to vertex " + vertex + ": ");

			if (algorithm.HasPathTo(vertex)) {
				foreach (var edge in algorithm.PathTo(vertex)) {
					Console.Write(edge.Source + "->" + edge.Target + " (" + edge.Weight + ") ");
				}
			} else {
				Console.Write("There is no monotonic path to vertex " + vertex);
			}
		}
	}
}

namespace UnitTests;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

[TestFixture(typeof(Dijkstra<double>))]
[TestFixture(typeof(DijkstraLazy<double>))]
public class ShortestPathTests<T> 
	where T : IShortestPath<double>
{
	public IShortestPath<double> GetAlgorithm(IReadOnlyEdgeWeightedDigraph<double> graph, int source)
	{
		return typeof(T) switch
		{
			var type when type == typeof(Dijkstra<double>) 
				=> new Dijkstra<double>(graph, source),
			
			var type when type == typeof(DijkstraLazy<double>) 
				=> new DijkstraLazy<double>(graph, source),
			
			_ 
				=> throw new ArgumentException($"Unsupported type: {typeof(T)}"),
		};
	}

	[Test]
	public void TestMethod()
	{
		const string edges = 
			"0,2,3.0;" +
			"0,1,1.0;" +
			"1,2,1.0;" +
			"2,3,1.0;" +
			"1,3,4.0";

		var graph = edges.ToDigraph<double>();
		var algorithm = GetAlgorithm(graph, 0);
		
		Assert.Multiple(() =>
		{
			Assert.That(algorithm.HasPathTo(0));
			Assert.That(algorithm.HasPathTo(1));
			Assert.That(algorithm.HasPathTo(2));
			Assert.That(algorithm.HasPathTo(3));

			Assert.That(algorithm.GetDistanceTo(0), Is.EqualTo(0.0));
			Assert.That(algorithm.GetDistanceTo(1), Is.EqualTo(1.0));
			Assert.That(algorithm.GetDistanceTo(2), Is.EqualTo(2.0));
			Assert.That(algorithm.GetDistanceTo(3), Is.EqualTo(3.0));
		});
	}
}

namespace UnitTests;

using AlgorithmsSW;
using AlgorithmsSW.EdgeWeightedDigraph;

public class DijkstraSourceSinkTests
{
	[Test]
	public void Test()
	{
		string edges =
			"0,2,3.0;" +
			"0,1,1.0;" +
			"1,2,1.0;" +
			"2,3,1.0;" +
			"1,3,4.0";

		var graph = edges.ToDigraph<double>();
		var algorithm = new DijkstraSourceSink<double>(graph, 0, 3);
		Assert.That(algorithm.PathExists, Is.True);
		var path = algorithm.Path;
		Assert.That(path.Count, Is.EqualTo(3));
		
		Assert.That(path[0].Target, Is.EqualTo(path[1].Source));
		Assert.That(path[1].Target, Is.EqualTo(path[2].Source));

		Assert.That(path[0].Source, Is.EqualTo(0));
		Assert.That(path[1].Source, Is.EqualTo(1));
		Assert.That(path[2].Source, Is.EqualTo(2));
		Assert.That(path[2].Target, Is.EqualTo(3));

	}
}

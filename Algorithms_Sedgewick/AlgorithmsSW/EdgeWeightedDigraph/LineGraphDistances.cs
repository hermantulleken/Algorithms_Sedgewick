namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;
using List;

// 4.4.31
public class LineGraphDistances
{
	private readonly double[] distanceTo;

	public LineGraphDistances(IEdgeWeightedDigraph<double> graph)
	{
		var degrees = new Degrees(graph);
		distanceTo = new double[graph.VertexCount];

		if (degrees.SourcesCount != 1)
		{
			throw new ArgumentException("Not a line graph.", nameof(graph));
		}
		
		if(degrees.SinksCount != 1)
		{
			throw new ArgumentException("Not a line graph.", nameof(graph));
		}

		int sink = degrees.Sinks.First();
		
		int vertex = degrees.Sources.First();
		distanceTo[vertex] = 0;
		int vertexCount = 1;
		var edgesInOrder = new ResizeableArray<DirectedEdge<double>>(graph.VertexCount);
		do
		{
			var edges = graph.GetIncidentEdges(vertex);

			if (edges.Count() != 1)
			{
				throw new ArgumentException("Not a line graph.", nameof(graph));
			}

			var edge = edges.First();
			edgesInOrder.Add(edge);
			distanceTo[edge.Target] = distanceTo[vertex] + edge.Weight;
			vertex = edge.Target;
			vertexCount++;
		} while (vertex != sink);

		if (vertexCount != graph.VertexCount)
		{
			throw new ArgumentException("Graph is not connected.", nameof(graph));
		}
	}
	
	public double GetDistance(int source, int target) => distanceTo[target] - distanceTo[source];
}

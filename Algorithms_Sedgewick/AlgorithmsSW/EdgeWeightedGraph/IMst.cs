namespace AlgorithmsSW.EdgeWeightedGraph;

public interface IMst<T>
{
	public IEnumerable<Edge<T>> Edges { get; }
	T Weight { get; }
}

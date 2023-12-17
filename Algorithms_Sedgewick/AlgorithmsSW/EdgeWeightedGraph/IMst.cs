namespace AlgorithmsSW.EdgeWeightedGraph;

public interface IMst<T>
{
	public IEnumerable<Edge<T>> Edges { get; }

	public T Weight(Func<T, T, T> add) 
		=> Edges
			.Select(e => e.Weight)
			.Aggregate(add);
}

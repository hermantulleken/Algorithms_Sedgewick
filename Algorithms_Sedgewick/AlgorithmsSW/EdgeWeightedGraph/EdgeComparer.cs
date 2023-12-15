namespace AlgorithmsSW.EdgeWeightedGraph;

public class EdgeComparer<T> (IComparer<T> comparer) : IComparer<Edge<T>>
{
	public int Compare(Edge<T> x, Edge<T> y) => comparer.Compare(x.Weight, y.Weight);
}

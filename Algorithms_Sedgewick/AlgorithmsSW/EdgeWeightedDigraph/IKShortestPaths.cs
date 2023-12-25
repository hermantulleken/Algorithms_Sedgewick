namespace AlgorithmsSW.EdgeWeightedGraph;

using EdgeWeightedDigraph;

public interface IKShortestPaths<TWeight>
{
	bool HasPath(int k);
	
	DirectedPath<TWeight> GetPath(int i);
}
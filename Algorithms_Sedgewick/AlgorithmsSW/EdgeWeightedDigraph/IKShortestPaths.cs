namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;

public interface IKShortestPaths<TWeight>
	where TWeight : INumber<TWeight>
{
	bool HasPath(int k);
	
	DirectedPath<TWeight> GetPath(int rank);
}

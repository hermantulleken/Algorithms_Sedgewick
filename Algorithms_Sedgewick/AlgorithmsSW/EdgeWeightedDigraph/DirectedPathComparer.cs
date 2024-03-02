namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics;
using System.Numerics;

/// <summary>
/// Compares two <see cref="DirectedPath{TWeight}"/>s by their <see cref="DirectedPath{TWeight}.Distance"/>.
/// </summary>
/// <param name="weightComparer">The comparer to compare the distances with.</param>
/// <typeparam name="TWeight">The type of the weight.</typeparam>
public class DirectedPathComparer<TWeight>(IComparer<TWeight> weightComparer)
	: IComparer<DirectedPath<TWeight>>
	where TWeight : IFloatingPoint<TWeight>
{
	public int Compare(DirectedPath<TWeight>? x, DirectedPath<TWeight>? y)
	{
		Debug.Assert(x != null);
		Debug.Assert(y != null);
			
		return weightComparer.Compare(x.Distance, y.Distance);
	}
}

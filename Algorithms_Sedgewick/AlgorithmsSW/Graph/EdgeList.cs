using System.Collections;
using System.Runtime.CompilerServices;
using AlgorithmsSW.List;

namespace AlgorithmsSW.Graph;

[CollectionBuilder(typeof(EdgeList), nameof(Create))]
public class EdgeList : IEnumerable<(int, int)>
{
	private readonly ResizeableArray<(int, int)> edges = [];
	
	public static EdgeList Create(ReadOnlySpan<(int, int)> edges)
	{
		var list = new EdgeList();

		foreach (var edge in edges)
		{
			list.Add(edge);
		}

		return list;
	}

	public EdgeList()
	{
	}

	public void Add((int, int) edge) => edges.Add(edge);

	public IEnumerator<(int, int)> GetEnumerator() => edges.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using AlgorithmsSW.List;

namespace AlgorithmsSW.Digraphs;

public class Degrees
{
	private readonly int[] inDegrees;
	private readonly int[] outDegrees;

	private readonly ResizeableArray<int> sources;
	private readonly ResizeableArray<int> sinks;

	public int SourcesCount => sources.Count;

	public int SinksCount => sinks.Count;
	
	public IEnumerable<int> Sources => sources;

	public IEnumerable<int> Sinks => sinks;
	
	public bool IsMap => SinksCount == 0;

	public bool IsSourceless => SourcesCount == 0;

	public Degrees(IDigraph digraph)
	{
		digraph.ThrowIfNull();
		
		inDegrees = new int[digraph.VertexCount];
		outDegrees = new int[digraph.VertexCount];
		sources = new ResizeableArray<int>(digraph.VertexCount);
		sinks = new ResizeableArray<int>(digraph.VertexCount);
		
		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			foreach (int adjacent in digraph.GetAdjacents(vertex))
			{
				inDegrees[adjacent]++;
				outDegrees[vertex]++;
			}
		}
		
		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			if (inDegrees[vertex] == 0)
			{
				sources.Add(vertex);
			}
			
			if (outDegrees[vertex] == 0)
			{
				sinks.Add(vertex);
			}
		}
	}
	
	public int GetIndegree(int vertex)
	{
		ValidateVertex(vertex);
		
		return inDegrees[vertex];
	}
	
	public int GetOutdegree(int vertex)
	{
		ValidateVertex(vertex);
		
		return outDegrees[vertex];
	}
	
	private void ValidateVertex(int vertex)
	{
		if (vertex < 0 || vertex >= inDegrees.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(vertex));
		}
	}
}

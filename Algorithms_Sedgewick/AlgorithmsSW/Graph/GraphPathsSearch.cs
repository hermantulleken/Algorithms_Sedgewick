using System.Diagnostics.CodeAnalysis;

namespace AlgorithmsSW.Graph;

[SuppressMessage(
	"StyleCop.CSharp.MaintainabilityRules", 
	"SA1401:Fields should be private", 
	Justification = Tools.AbstractClassWithProtectedFields)]
public abstract class GraphPathsSearch
{
	protected readonly bool[] Marked;

	protected readonly int[] EdgeOnPathFromSourceTo;
	
	protected readonly int SourceVertex;
	
	public IReadOnlyList<bool> HasPathTo => Marked;
	
	public IEnumerable<int> MarkedVertexes
	{
		get
		{
			for (int i = 0; i < Marked.Length; i++)
			{
				if (Marked[i])
				{
					yield return i;
				}
			}
		}
	}

	protected GraphPathsSearch(IReadOnlyGraph graph, int sourceVertex)
	{
		Marked = new bool[graph.VertexCount];
		EdgeOnPathFromSourceTo = new int[graph.VertexCount];
		SourceVertex = sourceVertex;
	}

	public bool TryGetPathTo(int targetVertex, [NotNullWhen(true)] out IEnumerable<int>? path)
	{
		if (!HasPathTo[targetVertex])
		{
			path = null;
			return false;
		}

		path = GetPathToUnsafe(targetVertex);
		return true;
	}
	
	public IEnumerable<int> GetPathTo(int targetVertex)
	{
		if (!HasPathTo[targetVertex])
		{
			ThrowHelper.ThrowInvalidOperationException("There is no path to the target node");
		}

		return GetPathToUnsafe(targetVertex);
	}

	private IEnumerable<int> GetPathToUnsafe(int targetVertex)
	{
		var path = new Stack<int>();
		
		for (int pathVertex = targetVertex; pathVertex != SourceVertex; pathVertex = EdgeOnPathFromSourceTo[pathVertex])
		{
			path.Push(pathVertex);
		}
		
		path.Push(SourceVertex);
		
		return path;
	}
}

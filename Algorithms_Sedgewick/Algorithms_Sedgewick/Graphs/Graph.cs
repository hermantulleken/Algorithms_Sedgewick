using Algorithms_Sedgewick.SymbolTable;

namespace Algorithms_Sedgewick.Graphs;

public static class Graph
{
	
	public static IGraph CreateEmptyGraph(Func<int, IGraph> graphFactory, int vertexCount)
	{
		var emptyGraph = graphFactory(vertexCount);
		return emptyGraph;
	}
	
	public static IGraph CreateCyclicGraph(Func<int, IGraph> graphFactory, int vertexCount)
	{
		var graph = graphFactory(vertexCount);
		
		for (int i = 0; i < vertexCount; i++)
		{
			graph.AddEdge(i, (i + 1) % vertexCount);
		}
		
		return graph;
	}
	
	public static IGraph CreateCompleteGraph(Func<int, IGraph> graphFactory, int vertexCount)
	{
		var completeGraph = graphFactory(vertexCount);

		for (int vertex0 = 0; vertex0 < vertexCount; vertex0++)
		{
			for (int vertex1 = vertex0 + 1; vertex1 < vertexCount; vertex1++)
			{
				completeGraph.AddEdge(vertex0, vertex1);
			}
		}

		return completeGraph;
	}
	
	public static IGraph CreateCompleteBipartiteGraph(Func<int, IGraph> graphFactory, int m, int n)
	{
		int totalVertices = m + n;
		var completeBipartiteGraph = graphFactory(totalVertices);

		// Connect each vertex in the first set (0 to m-1) with each vertex in the second set (m to m+n-1)
		for (int i = 0; i < m; i++)
		{
			for (int j = m; j < totalVertices; j++)
			{
				completeBipartiteGraph.AddEdge(i, j);
			}
		}

		return completeBipartiteGraph;
	}
}

public class Graph<T>
{
	private readonly IGraph graph;

	private readonly ISymbolTable<T, int> symbolsToInt;

	private readonly T[] intToSymbol;
	
	public Graph(T[] intToSymbol, Func<IGraph> graphFactory, Func<ISymbolTable<T, int>> symbolTableFactory)
		: this(intToSymbol, graphFactory(), symbolTableFactory())
	{
	}

	private Graph(T[] intToSymbol, IGraph graph, ISymbolTable<T, int> symbolsToInt)
	{
		this.graph = graph;
		this.symbolsToInt = symbolsToInt;
		this.intToSymbol = intToSymbol;

		for (int i = 0; i < intToSymbol.Length; i++)
		{
			symbolsToInt[intToSymbol[i]] = i;
		}
	}

	public int VertexCount => graph.VertexCount;

	public int EdgeCount => graph.EdgeCount;

	public bool IsEmpty => VertexCount == 0;

	public IEnumerable<T> Vertices => Enumerable.Range(0, VertexCount).Select(v => intToSymbol[v]);

	public void AddEdge(T vertex0, T vertex1) => graph.AddEdge(symbolsToInt[vertex0], symbolsToInt[vertex1]);

	public IEnumerable<T> GetAdjacents(T vertex) 
		=> graph
			.GetAdjacents(symbolsToInt[vertex])
			.Select(v => intToSymbol[v]);

	public override string ToString()
	{
		string @string = VertexCount + " vertices, " + EdgeCount + " edges\n";
		
		foreach (T vertex in Vertices)
		{
			@string += vertex 
						+ ": " 
						+ string.Join(" ", GetAdjacents(vertex))
						+ "\n";
		}

		return @string;
	}
}

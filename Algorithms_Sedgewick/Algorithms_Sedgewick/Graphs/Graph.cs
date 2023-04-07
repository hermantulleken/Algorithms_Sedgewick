using Algorithms_Sedgewick.SymbolTable;

namespace Algorithms_Sedgewick.Graphs;

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
namespace AlgorithmsSW.Graph;

using System.Collections;

/*	Design note: There is a lot of different ways to implement this. 
	
	Probably the best way is to use functions to check connectivity, since this can be done in constant time with no 
	extra memory. To support this design, we would probably implement a class that uses functions for connectivity, 
	and then supply that with specific functions for a grid. This will also allow other grids to be supported. 
	
	A related design is to not necessarily use a graph data structure in the first place, but simply a function that
	given a node expand to connected nodes. 
	
	Both of these will need changes to the IGraph interface or the implemented algorithms, and is therefor a big design 
	task. 
	
	This version here is a simple implementation that does not require sweeping design changes, although it is not 
	super efficient. 
*/
public class GridGraph : IGraph
{
	private readonly int width;
	private readonly int height;
	private readonly IGraph graph;
	
	public int VertexCount { get; }
	
	public int EdgeCount { get; }

	public IEnumerable<(int x, int y)> Cells => ((IGraph)this).Vertexes.Select(GetCoordinates);
	
	public bool SupportsParallelEdges => graph.SupportsParallelEdges;

	public bool SupportsSelfLoops => graph.SupportsSelfLoops;
	
	public GridGraph(int width, int height)
	{
		this.width = width;
		this.height = height;
		VertexCount = this.width * this.height;
		EdgeCount = 2 * this.width * this.height - this.height - this.width; // (h - 1) * w + (w - 1) * h
		
		graph = DataStructures.Graph(VertexCount);
	}

	public IEnumerable<int> GetAdjacents(int vertex) => graph.GetAdjacents(vertex);
	
	public IEnumerable<(int x, int y)> GetAdjacents((int x, int y) cell) 
		=> graph
			.GetAdjacents(GetVertexIndex(cell))
			.Select(GetCoordinates);
	
	public bool ContainsEdge(int vertex0, int vertex1) => graph.ContainsEdge(vertex0, vertex1);
	
	public bool ContainsEdge((int x, int y) cell0, (int x, int y) cell1) 
		=> graph.ContainsEdge(GetVertexIndex(cell0), GetVertexIndex(cell1));

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() => graph.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)graph).GetEnumerator();

	public void AddEdge(int vertex0, int vertex1) => graph.AddEdge(vertex0, vertex1);
	
	public void AddEdge((int x, int y) cell0, (int x, int y) cell1) 
		=> graph.AddEdge(GetVertexIndex(cell0), GetVertexIndex(cell1));

	public bool RemoveEdge(int vertex0, int vertex1) => graph.RemoveEdge(vertex0, vertex1);
	
	public bool RemoveEdge((int x, int y) cell0, (int x, int y) cell1) 
		=> graph.RemoveEdge(GetVertexIndex(cell0), GetVertexIndex(cell1));
	
	private int GetVertexIndex((int x, int y) cell) => cell.y * width + cell.x;
	
	private (int x, int y) GetCoordinates(int index) => (index % width, index / width);
}

namespace Algorithms_Sedgewick.Graphs;

public class DepthFirstOrder
{
	private readonly bool[] marked;
	private readonly Queue<int> preOrder;
	private readonly Queue<int> postOrder;
	private readonly Stack<int> reversePostOrder;
	
	public IEnumerable<int> PreOrder => preOrder;
	
	public IEnumerable<int> PostOrder => postOrder;
	
	public IEnumerable<int> ReversePostOrder => reversePostOrder;
	
	public DepthFirstOrder(IDigraph graph)
	{
		graph.ThrowIfNull();
		
		preOrder = new Queue<int>();
		postOrder = new Queue<int>();
		reversePostOrder = new Stack<int>();
		marked = new bool[graph.VertexCount];
		
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(graph, vertex);
			}
		}
	}
	
	private void Search(IDigraph graph, int vertex)
	{
		preOrder.Enqueue(vertex);
		marked[vertex] = true;
		
		foreach (int adjacent in graph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				Search(graph, adjacent);
			}
		}
		
		postOrder.Enqueue(vertex);
		reversePostOrder.Push(vertex);
	}
}
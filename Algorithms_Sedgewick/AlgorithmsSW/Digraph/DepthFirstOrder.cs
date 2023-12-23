namespace AlgorithmsSW.Digraph;

/// <summary>
/// Provides an algorithm for finding the vertices in a directed graph in depth-first pre-order, post-order, and
/// reverse post-order.
/// </summary>
public class DepthFirstOrder
{
	private readonly bool[] marked;
	private readonly Queue<int> preOrder;
	private readonly Queue<int> postOrder;
	private readonly Stack<int> reversePostOrder;
	
	/// <summary>
	/// Gets the vertices in depth-first pre-order.
	/// </summary>
	public IEnumerable<int> PreOrder => preOrder;
	
	/// <summary>
	/// Gets the vertices in depth-first post-order.
	/// </summary>
	public IEnumerable<int> PostOrder => postOrder;
	
	/// <summary>
	/// Gets the vertices in depth-first reverse post-order.
	/// </summary>
	public IEnumerable<int> ReversePostOrder => reversePostOrder;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DepthFirstOrder"/> class.
	/// </summary>
	/// <param name="digraph">The graph to search.</param>
	public DepthFirstOrder(IReadOnlyDigraph digraph)
	{
		digraph.ThrowIfNull();
		
		preOrder = new Queue<int>();
		postOrder = new Queue<int>();
		reversePostOrder = new Stack<int>();
		marked = new bool[digraph.VertexCount];
		
		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(digraph, vertex);
			}
		}
	}
	
	private void Search(IReadOnlyDigraph digraph, int vertex)
	{
		preOrder.Enqueue(vertex);
		marked[vertex] = true;
		
		foreach (int adjacent in digraph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				Search(digraph, adjacent);
			}
		}
		
		postOrder.Enqueue(vertex);
		reversePostOrder.Push(vertex);
	}
}

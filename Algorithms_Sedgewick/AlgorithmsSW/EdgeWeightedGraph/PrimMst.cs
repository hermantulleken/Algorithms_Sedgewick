using AlgorithmsSW.PriorityQueue;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class PrimMst<T> : IMst<T>
{
	private readonly Edge<T>[] edgeTo;
	private readonly T[] distTo;
	private readonly bool[] marked;
	private readonly IndexPriorityQueue<T> priorityQueue;
	
	public IEnumerable<Edge<T>> Edges => throw new NotImplementedException();
	
	public T Weight => throw new NotImplementedException();
	
	public PrimMst(
		EdgeWeightedGraphWithAdjacencyLists<T> graph, 
		IComparer<T> comparer,
		T minValue,
		T maxValue)
	{
		edgeTo = new Edge<T>[graph.VertexCount];
		distTo = new T[graph.VertexCount];
		marked = new bool[graph.VertexCount];
		priorityQueue = new IndexPriorityQueue<T>(graph.VertexCount, comparer);
		
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			distTo[vertex] = maxValue;
		}
		
		distTo[0] = minValue;
		priorityQueue.Insert(0, distTo[0]);
		
		while (!priorityQueue.IsEmpty())
		{
			int vertex = priorityQueue.PopMin();
			Visit(graph, vertex);
		}
	}
	
	private void Visit(EdgeWeightedGraphWithAdjacencyLists<T> graph, int vertex)
	{
		marked[vertex] = true;
		
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int otherVertex = edge.OtherVertex(vertex);
			
			if (marked[otherVertex])
			{
				continue;
			}

			if (Comparer<T>.Default.Compare(edge.Weight, distTo[otherVertex]) >= 0)
			{
				continue;
			}
			
			distTo[otherVertex] = edge.Weight;
			edgeTo[otherVertex] = edge;
				
			if (priorityQueue.Contains(otherVertex))
			{
				priorityQueue.Change(otherVertex, distTo[otherVertex]);
			}
			else
			{
				priorityQueue.Insert(otherVertex, distTo[otherVertex]);
			}
		}
	}
}

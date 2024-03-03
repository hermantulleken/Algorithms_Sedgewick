namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Collections.Generic;

public class EdgeWeightedCycle<T>
{
	private bool[] marked;
	private Edge<T>[] edgeTo;
	private Stack<Edge<T>>? cycle; // Edges on a cycle (if one exists)
	private bool[] onStack; // Vertices on recursive call stack
	private HashSet<Edge<T>> visitedEdges;
	private bool cycleFound;

	public EdgeWeightedCycle(IEdgeWeightedGraph<T> edgeWeightedGraph)
	{
		InitVariables(edgeWeightedGraph);

		for (int vertex = 0; vertex < edgeWeightedGraph.VertexCount; vertex++)
		{
			if (cycleFound)
			{
				break;
			}

			if (!marked[vertex])
			{
				Dfs(edgeWeightedGraph, vertex);
			}
		}
	}

	// Constructor that only searches for cycles using the vertices passed as parameter as sources.
	// Useful for subgraph search or when many searches have to be done and not all vertices need to be analyzed.
	public EdgeWeightedCycle(IEdgeWeightedGraph<T> edgeWeightedGraph, Set.HashSet<int> vertices)
	{
		InitVariables(edgeWeightedGraph);

		foreach (int vertex in vertices)
		{
			if (cycleFound)
			{
				break;
			}

			if (!marked[vertex])
			{
				Dfs(edgeWeightedGraph, vertex);
			}
		}
	}

	private void InitVariables(IEdgeWeightedGraph<T> edgeWeightedGraph)
	{
		marked = new bool[edgeWeightedGraph.VertexCount];
		edgeTo = new Edge<T>[edgeWeightedGraph.VertexCount];
		onStack = new bool[edgeWeightedGraph.VertexCount];
		visitedEdges = new HashSet<Edge<T>>();
		cycleFound = false;
		cycle = null;
	}

	private void Dfs(IEdgeWeightedGraph<T> edgeWeightedGraph, int vertex)
	{
		onStack[vertex] = true;
		marked[vertex] = true;

		foreach (var neighbor in edgeWeightedGraph.GetIncidentEdges(vertex))
		{
			if (visitedEdges.Contains(neighbor))
			{
				continue;
			}

			visitedEdges.Add(neighbor);
			int neighborVertex = neighbor.OtherVertex(vertex);

			if (HasCycle())
			{
				return;
			}
			else if (!marked[neighborVertex])
			{
				edgeTo[neighborVertex] = neighbor;
				Dfs(edgeWeightedGraph, neighborVertex);
			}
			else if (onStack[neighborVertex])
			{
				cycleFound = true;
				cycle = new Stack<Edge<T>>();

				for (int currentVertex = vertex; currentVertex != neighborVertex;
					currentVertex = edgeTo[currentVertex].OtherVertex(currentVertex))
				{
					cycle.Push(edgeTo[currentVertex]);
				}

				cycle.Push(neighbor);
			}
		}

		onStack[vertex] = false;
	}
	
	public bool HasCycle() => cycle != null;

	public Stack<Edge<T>> Cycle()
	{
		return cycle;
	}
}

// Assume that the Edge and EdgeWeightedGraphInterface classes are defined elsewhere in your code.

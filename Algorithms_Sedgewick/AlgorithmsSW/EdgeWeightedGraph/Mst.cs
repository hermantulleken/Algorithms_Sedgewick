using static System.Diagnostics.Debug;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class Mst
{
	
	/// <summary>
	/// Finds a new spanning tree if we delete the given edge from a graph, given an existing MST.
	/// </summary>
	/// <param name="graph">The graph to delete the edge from.</param>
	/// <param name="mst">The existing MST.</param>
	/// <param name="edge">The edge to delete.</param>
	/// <typeparam name="T">The type of the weight.</typeparam>
	/// <returns>A new MST.</returns>
	// 4.3.14
	public EdgeWeightedGraphWithAdjacencyLists<T> DeleteEdge<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IMst<T> mst, Edge<T> edge)
	{
		var newMst = new EdgeWeightedGraphWithAdjacencyLists<T>(graph);
		graph.RemoveEdge(edge);
		
		var (component0, component1) = GetComponents(newMst);
		var minEdge = FindMinimumEdgeThatConnectTwoComponents(newMst, component0, component1);
		newMst.AddEdge(minEdge);

		return newMst;
	}

	private (ISet<int> component0, ISet<int> component1) GetComponents<T>(EdgeWeightedGraphWithAdjacencyLists<T> newMst)
	{
		var components = new Components<T>(newMst, newMst.Comparer);
		var component0 = DataStructures.Set<int>();
		var component1 = DataStructures.Set<int>();

		foreach (int vertex in newMst.Vertexes)
		{
			if (components.GetComponentId(vertex) == 0)
			{
				component0.Add(vertex);
			}
			else
			{
				Assert(components.GetComponentId(vertex) == 1);
				component1.Add(vertex);
			}
		}

		return (component0, component1);
	}

	private Edge<T> FindMinimumEdgeThatConnectTwoComponents<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, ISet<int> component0, ISet<int> component1)
	{
		Edge<T>? minEdge = null;
		
		foreach (var e in graph.Edges)
		{
			if ((!component0.Contains(e.Vertex0) || !component1.Contains(e.Vertex1)) &&
				(!component1.Contains(e.Vertex0) || !component0.Contains(e.Vertex1)))
			{
				continue;
			}
			
			if (minEdge != null && graph.Comparer.Compare(e.Weight, minEdge.Weight) >= 0)
			{
				continue;
			}
			
			minEdge = e;
		}
		
		Assert(minEdge != null);

		return minEdge;
	}
 }

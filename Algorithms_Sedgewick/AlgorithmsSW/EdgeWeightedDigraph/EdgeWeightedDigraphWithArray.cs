namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Collections;
using Digraph;

public class EdgeWeightedDigraphWithArray<TWeight> : IEdgeWeightedDigraph<TWeight>
{
	/*
		I'm not sure this presentation is good. Storing weights directly and doing without the Weighted is probably 
		better. Of course, this means we cannot implement IEdgeWeightedDigraph<TWeight> anymore (or not quite 
		efficiently).
	*/
	
	private readonly DirectedEdge<TWeight>?[,] edges; 

	public int VertexCount { get; }

	/// <inheritdoc/>
	public int EdgeCount { get; private set; }

	/// <inheritdoc/>
	public IEnumerable<DirectedEdge<TWeight>> WeightedEdges
	{
		get
		{
			for (int i = 0; i < VertexCount; i++)
			{
				for (int j = 0; j < VertexCount; j++)
				{
					if (edges[i, j] != null)
					{
						yield return edges[i, j]!;
					}
				}
			}
		}
	}
	
	public EdgeWeightedDigraphWithArray(int vertexCount)
	{
		VertexCount = vertexCount;
		edges = new DirectedEdge<TWeight>[vertexCount, vertexCount];
		EdgeCount = 0;
	}
	
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			if (edges[vertex, i] != null)
			{
				yield return i;
			}
		}
	}

	/// <inheritdoc/>
	public bool SupportsParallelEdges => false;
	
	/// <inheritdoc/>
	public bool SupportsSelfLoops => true;

	/// <inheritdoc/>
	public IEnumerable<DirectedEdge<TWeight>> GetIncidentEdges(int vertex)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			if (edges[vertex, i] != null)
			{
				yield return edges[vertex, i]!;
			}
		}
	}

	/// <inheritdoc/>
	public void AddEdge(DirectedEdge<TWeight> edge)
	{
		if (edges[edge.Source, edge.Target] != null)
		{
			throw new ArgumentException("Edge already exists.");
		}

		edges[edge.Source, edge.Target] = edge;
		EdgeCount++;
	}

	// TODO: Is this correct? should we check it is the same edge??
	/// <inheritdoc/>
	public bool RemoveEdge(DirectedEdge<TWeight> edge)
	{
		if (edges[edge.Source, edge.Target] == null)
		{
			return false;
		}

		edges[edge.Source, edge.Target] = null;
		EdgeCount--;
		return true;
	}

	/// <inheritdoc/>
	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() => ((IReadOnlyDigraph)this).Edges.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

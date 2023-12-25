﻿namespace AlgorithmsSW.EdgeWeightedDigraph;

public class EdgeWeightedDigraphWithArray<TWeight> : IEdgeWeightedDigraph<TWeight>
{
	/*
		I'm not sure this presentation is good. Storing weights directly and doing without the Weighted is probably 
		better. Of course, this means we cannot implement IEdgeWeightedDigraph<TWeight> anymore (or not quite 
		efficiently).
	*/
	
	private DirectedEdge<TWeight>?[,] edges;

	public int VertexCount { get; }
	
	public int EdgeCount { get; }
	
	public IComparer<TWeight> Comparer { get; }

	public IEnumerable<DirectedEdge<TWeight>> Edges
	{
		get
		{
			for(int i = 0; i < VertexCount; i++)
			{
				for(int j = 0; j < VertexCount; j++)
				{
					if (edges[i, j] != null)
					{
						yield return edges[i, j]!;
					}
				}
			}
		}
	}
	
	EdgeWeightedDigraphWithArray(int vertexCount, IComparer<TWeight> comparer)
	{
		VertexCount = vertexCount;
		Comparer = comparer;
		edges = new DirectedEdge<TWeight>[vertexCount, vertexCount];
	}
	
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		for(int i = 0; i < VertexCount; i++)
		{
			if (edges[vertex, i] != null)
			{
				yield return i;
			}
		}
	}

	
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

	public void AddEdge(DirectedEdge<TWeight> edge)
	{
		if(edges[edge.Source, edge.Target] != null)
		{
			throw new ArgumentException("Edge already exists.");
		}

		edges[edge.Source, edge.Target] = edge;
	}

	//IS this correct? should we check it is the same edge??
	public void RemoveEdge(DirectedEdge<TWeight> edge)
	{
		if(edges[edge.Source, edge.Target] == null)
		{
			throw new ArgumentException("Edge does not exists.");
		}

		edges[edge.Source, edge.Target] = null;
	}
}
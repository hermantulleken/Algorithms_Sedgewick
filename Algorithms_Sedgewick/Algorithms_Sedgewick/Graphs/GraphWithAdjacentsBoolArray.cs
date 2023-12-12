﻿using System.Collections;

namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// Represents a graph with a fixed number of vertices and edges, where adjacency is represented by a boolean array.
/// </summary>
public class GraphWithAdjacentsArray(int vertexCount) : IGraph
{
	private readonly bool[,] adjacents = new bool[vertexCount, vertexCount];

	/// <inheritdoc />
	public int VertexCount { get; } = vertexCount;

	/// <inheritdoc />
	public int EdgeCount { get; private set; } = 0;

	/// <inheritdoc />
	public void AddEdge(int vertex0, int vertex1)
	{
		if (ContainsEdge(vertex0, vertex1))
		{
			return;
		}
		
		adjacents[vertex0, vertex1] = true;

		if (vertex0 != vertex1)
		{
			adjacents[vertex1, vertex0] = true;
		}
		
		EdgeCount++;
	}

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex)
	{
		for (int i = 0; i < VertexCount; i++)
		{
			if (adjacents[vertex, i])
			{
				yield return i;
			}
		}
	}

	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex2)> GetEnumerator()
	{
		for (int vertex1 = 0; vertex1 < VertexCount; vertex1++)
		{
			for (int vertex0 = 0; vertex0 <= vertex1; vertex0++)
			{
				if (adjacents[vertex1, vertex0])
				{
					yield return (vertex0, vertex1);
				}
			}
		}
	}
	
	/// <inheritdoc/>
	public bool ContainsEdge(int vertex0, int vertex1) => adjacents[vertex0, vertex1];

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
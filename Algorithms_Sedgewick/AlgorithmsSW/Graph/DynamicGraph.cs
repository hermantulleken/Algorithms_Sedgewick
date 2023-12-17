using System.Collections;
using AlgorithmsSW.Counter;
using AlgorithmsSW.HashTable;
using AlgorithmsSW.SymbolTable;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Graph;

public class DynamicGraph // Supports removing vertices
	: IGraph
{
	private readonly ISymbolTable<int, Counter<int>> adjacents = new HashTableWithLinearProbing2<int, Counter<int>>(Comparer<int>.Default);

	private int vertexCount = 0;
	private int edgeCount = 0;

	public int VertexCount
	{
		get => vertexCount;
		
		set
		{
			vertexCount = value;
			Assert(IsVertexCountValid());
		}
	}

	public int EdgeCount
	{
		get => edgeCount;
		
		set
		{
			edgeCount = value;
			Assert(IsEdgeCountValid());
		}
	}
	
	public IEnumerable<int> Vertexes => adjacents.Keys;
	
	public DynamicGraph(params int[] vertexes)
	{
		AddVertexes(vertexes);
	}

	public void AddVertexes(params int[] vertexes)
	{
		// Throw exceptions first so we do not change the graph halfway through the operation
		foreach (int vertex in vertexes)
		{
			if (adjacents.ContainsKey(vertex))
			{
				throw new ArgumentException($"Vertex {vertex} already exists.");
			}
		}

		foreach (int vertex in vertexes)
		{
			adjacents[vertex] = new Counter<int>(Comparer<int>.Default);
			VertexCount++;
		}
	}
	
	public void RemoveVertex(int vertex)
	{
		if (!adjacents.ContainsKey(vertex))
		{
			throw new ArgumentException($"Vertex {vertex} does not exist.");
		}

		int edgesToRemove = GetAdjacents(vertex).Count();
		
		adjacents.RemoveKey(vertex);
		EdgeCount -= edgesToRemove;
		
		foreach (int vertex1 in Vertexes) // TODO what can we call vertex1
		{
			adjacents[vertex1].RemoveAll(vertex);
		}

		VertexCount--;
	}

	public bool SupportsParallelEdges => true;
	
	public bool SupportsSelfLoops => true;

	public void AddEdge(int vertex0, int vertex1)
	{
		if (!adjacents.ContainsKey(vertex0))
		{
			throw new ArgumentException($"Vertex {vertex0} does not exist.");
		}
		
		if (!adjacents.ContainsKey(vertex1))
		{
			throw new ArgumentException($"Vertex {vertex1} does not exist.");
		}
		
		adjacents[vertex0].Add(vertex1);

		if (vertex0 != vertex1)
		{
			adjacents[vertex1].Add(vertex0);
		}

		EdgeCount++;
	}

	public bool RemoveEdge(int vertex0, int vertex1)
	{
		if (!adjacents.ContainsKey(vertex0) || adjacents[vertex0][vertex1] == 0)
		{
			return false;
		}
		
		Assert(adjacents[vertex1][vertex0] > 0); // otherwise above if would have succeeded
		
		if (!adjacents.ContainsKey(vertex1))
		{
			return false;
		}
		
		adjacents[vertex0].Remove(vertex1);

		if (vertex0 != vertex1)
		{
			adjacents[vertex1].Remove(vertex0);
		}

		EdgeCount--;

		return true;
	}

	public IEnumerable<int> GetAdjacents(int vertex)
	{
		if (!adjacents.ContainsKey(vertex))
		{
			throw new ArgumentException($"Vertex {vertex} does not exist.");
		}

		var counter = adjacents[vertex];

		return counter.Keys.SelectMany(adjacent => Enumerable.Repeat(adjacent, counter[adjacent]));
	}

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator()
	{
		foreach (int vertex0 in Vertexes)
		{
			var counter = adjacents[vertex0];
			
			foreach (int vertex1 in counter.Keys)
			{
				if (vertex0 > vertex1)
				{
					continue;
				}
				
				for (int i = 0; i < counter[vertex1]; i++)
				{
					yield return (vertex0, vertex1);
				}
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private bool IsVertexCountValid() => VertexCount == adjacents.Count;

	private bool IsEdgeCountValid()
	{
		int selfLoops = 0;
		int edgeEndpoints = 0;
		
		foreach (int vertex0 in Vertexes)
		{
			selfLoops += adjacents[vertex0][vertex0];
			edgeEndpoints += Vertexes.Sum(vertex1 => adjacents[vertex0][vertex1]);
		}

		return EdgeCount == (edgeEndpoints + selfLoops) / 2;
	}
}

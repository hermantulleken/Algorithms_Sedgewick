using System.Collections;
using System.Runtime.CompilerServices;
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
			ValidateVertexDoesNotExist(vertex);
		}

		foreach (int vertex in vertexes)
		{
			adjacents[vertex] = new Counter<int>(Comparer<int>.Default);
			VertexCount++;
		}
	}
	
	public void RemoveVertex(int vertex)
	{
		ValidateVertexExists(vertex);
		
		if (!adjacents.ContainsKey(vertex))
		{
			throw new ArgumentException($"Vertex {vertex} does not exist.");
		}

		int edgesToRemove = GetAdjacents(vertex).Count();
		
		adjacents.RemoveKey(vertex);
		EdgeCount -= edgesToRemove;
		
		// TODO what can we call vertex1
		foreach (int vertex1 in Vertexes) 
		{
			adjacents[vertex1].RemoveAll(vertex);
		}

		VertexCount--;
	}

	public bool SupportsParallelEdges => true;
	
	public bool SupportsSelfLoops => true;

	public void AddEdge(int vertex0, int vertex1)
	{
		ValidateVertexExists(vertex0);
		ValidateVertexExists(vertex1);
		
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
		ValidateVertexExists(vertex);
		var counter = adjacents[vertex];

		return counter.Keys.SelectMany(adjacent => Enumerable.Repeat(adjacent, counter[adjacent]));
	}

	public bool ContainsEdge(int vertex0, int vertex1)
	{
		ValidateVertexExists(vertex0);
		ValidateVertexExists(vertex1);
		return adjacents[vertex0][vertex1] > 0;
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
	
	private void ValidateVertexExists(int vertex, [CallerArgumentExpression(nameof(vertex))] string? vertexName = null)
	{
		if (!adjacents.ContainsKey(vertex))
		{
			/*
				TODO: This should really be an ArgumentOutOfRangeException, but that would break the tests
				since all the other ones throw IndexOutOfRangeException. The other containers need to change but we will
				keep this for now. 
			*/
			throw new IndexOutOfRangeException($"Vertex {vertex} does not exist. Argument: {vertexName}.");
		}
	}
	
	private void ValidateVertexDoesNotExist(int vertex, [CallerArgumentExpression(nameof(vertex))] string? vertexName = null)
	{
		if (adjacents.ContainsKey(vertex))
		{
			throw new ArgumentException($"Vertex {vertex} already exists.", vertexName);
		}
	}
}

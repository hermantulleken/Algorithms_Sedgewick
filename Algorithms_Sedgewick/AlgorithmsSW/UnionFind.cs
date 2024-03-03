using static System.Diagnostics.Debug;

namespace AlgorithmsSW;

/// <summary>
/// An implementation of the union-find data structure.
/// </summary>
[AlgorithmReference(1, 5)]
public class UnionFind
{
	private readonly int[] componentIndex; // access to component id (site indexed)

	public int ComponentCount { get; private set; }
	
	public UnionFind(int vertexCount)
	{
		ComponentCount = vertexCount;
		componentIndex = new int[vertexCount];
		for (int i = 0; i < vertexCount; i++)
		{
			componentIndex[i] = i;
		}
	}
	
	public void Union(int vertex0, int vertex1)
	{
		int component0 = GetComponentIndex(vertex0);
		int component1 = GetComponentIndex(vertex1);

		if (component0 == component1)
		{
			return;
		}

		for (int i = 0; i < componentIndex.Length; i++)
		{
			if (componentIndex[i] == component1)
			{
				componentIndex[i] = component0;
			}
		}

		ComponentCount--;
		Assert(ComponentCount > 0);
	}
	
	public int GetComponentIndex(int vertex)
	{
		return componentIndex[vertex];
	}
	
	public bool IsConnected(int vertex0, int vertex1) 
		=> GetComponentIndex(vertex0) == GetComponentIndex(vertex1);
}

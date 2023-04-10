using Algorithms_Sedgewick.Graphs;

namespace UnitTests;

using NUnit.Framework;

// Define the abstract base test class
public abstract class GraphTestsBase
{
	protected abstract IGraph CreateInstance(int vertexCount);

	[Test]
	public void TestAddNode()
	{
		IGraph graph = CreateInstance(10);
		// Test implementation for adding nodes using the graph instance
	}

	// Other common tests for IGraph go here...
}

// Define the test class for the GraphWithAdjacentsArrays implementation
[TestFixture]
public class GraphWithAdjacentsArraysTests : GraphTestsBase
{
	protected override IGraph CreateInstance(int vertexCount)
	{
		return new GraphWithAdjacentsArrays(vertexCount); // Return an instance of the specific implementation
	}
}

// Define the test class for the GraphWithAdjacentsSet implementation
[TestFixture]
public class GraphWithAdjacentsSetTests : GraphTestsBase
{
	protected override IGraph CreateInstance(int vertexCount)
	{
		return new GraphWithAdjacentsSet(vertexCount); // Return an instance of the specific implementation
	}
}

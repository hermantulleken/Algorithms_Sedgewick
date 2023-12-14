namespace Algorithms_Sedgewick.Digraphs;

public interface IHamiltonianPath
{
	/// <summary>
	/// Gets a value indicating whether the digraph has a Hamiltonian path.
	/// </summary>
	bool HasHamiltonianPath { get; }
	
	/// <summary>
	/// Gets the vertices on a Hamiltonian path in the digraph, if there is one.
	/// </summary>
	/// <exception cref="InvalidOperationException">there is no Hamiltonian cycle.</exception>
	IEnumerable<int> Path { get; }
}
namespace AlgorithmsSW.Graph;

public interface IEdgeData<T>
{
	public T this[int vertex0, int vertex1] { get; set; }
}

public interface IVertexData<T>
{
	public T this[int vertext] { get; set; }
}

namespace AlgorithmsSW.Pool;

public class DefaultFactory<T> : IFactory<T> 
	where T : new()
{
	public T GetNewInstance() => new T();
	
	public void Reset(T instance) => instance = default!;
}

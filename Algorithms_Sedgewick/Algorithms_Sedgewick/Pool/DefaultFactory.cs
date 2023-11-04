namespace Algorithms_Sedgewick.Pool;

public class DefaultFactory<T> : IFactory<T> 
	where T : new()
{
	public T GetNewInstance() => new T();
	
	public void DestroyInstance(T instance) => instance = default!;
}
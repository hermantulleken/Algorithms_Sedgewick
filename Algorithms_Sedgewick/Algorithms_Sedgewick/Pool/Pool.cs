namespace Algorithms_Sedgewick.Pool;

public interface IFactory<T>
{
	T GetNewInstance();
	
	void DestroyInstance(T instance);
}

public class DefaultFactory<T> : IFactory<T> 
	where T : new()
{
	public T GetNewInstance() => new T();
	
	public void DestroyInstance(T instance) => instance = default!;
}

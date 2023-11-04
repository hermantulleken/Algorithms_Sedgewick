namespace Algorithms_Sedgewick.Pool;

public interface IFactory<T>
#pragma warning restore SA1649
{
	T GetNewInstance();
	
	void Reset(T instance);
}
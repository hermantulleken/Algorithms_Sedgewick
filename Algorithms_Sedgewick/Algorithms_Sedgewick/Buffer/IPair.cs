namespace Algorithms_Sedgewick.Buffer;

public interface IPair<out T>
{
	T First { get; }

	T Last { get; }
}

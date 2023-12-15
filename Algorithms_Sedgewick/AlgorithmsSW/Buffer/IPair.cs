namespace AlgorithmsSW.Buffer;

/// <summary>
/// Represents a pair of items.
/// </summary>
/// <typeparam name="T">The type of items in the pair.</typeparam>
public interface IPair<out T>
{
	/// <summary>
	/// Gets the first item in the pair.
	/// </summary>
	T? First { get; }

	/// <summary>
	/// Gets the last item in the pair.
	/// </summary>
	T? Last { get; }
}

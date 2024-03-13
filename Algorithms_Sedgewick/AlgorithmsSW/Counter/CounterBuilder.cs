namespace AlgorithmsSW.Counter;

using System.Runtime.CompilerServices;

internal class CounterBuilder
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Counter<T> Create<T>(ReadOnlySpan<T> values)
	{
		var counter = new Counter<T>(Comparer<T>.Default);

		foreach (var value in values)
		{
			counter.Add(value);
		}

		return counter;
	}
}

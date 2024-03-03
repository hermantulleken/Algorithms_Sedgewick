namespace AlgorithmsSW;

using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides extension methods related to <see cref="IComparer{T}"/>.
/// </summary>
public static class ComparerExtensions
{
	private class InvertedComparer<T> : IComparer<T>
	{
		private readonly IComparer<T> comparer;

		public InvertedComparer(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		public int Compare(T? x, T? y) => comparer.Compare(y, x);
	}
	
	/*public static IComparer<TTarget> Convert<TSource, TTarget>(this IComparer<TSource> comparer, Func<TTarget, TSource> converter)
	{
		comparer.ThrowIfNull();
		converter.ThrowIfNull();
		
		return new ConvertComparer<TSource, TTarget>(comparer, converter);
	}*/

	private class ComparerToEqualityComparerAdapter<T>(IComparer<T> comparer, Func<T, int> getHashCode)
		: IEqualityComparer<T>
	{
		public bool Equals(T? x, T? y) => comparer.Compare(x, y) == 0;

		public int GetHashCode(T obj) => getHashCode(obj);
	}
	
	public static IEqualityComparer<T> ToEqualityComparer<T>(
		this IComparer<T> comparer,
		Func<T, int>? getHashCode = null)
	{
		comparer.ThrowIfNull();
		getHashCode ??= obj => obj == null ? 0 : obj.GetHashCode();
		
		return new ComparerToEqualityComparerAdapter<T>(comparer, getHashCode);
	}
	
	public static bool ApproximatelyEqual<T>(this IComparer<T> comparer, T left, T right, T tolerance)
		where T : IFloatingPoint<T>
	{
		comparer.ThrowIfNull();
		tolerance.ThrowIfNull();
		
		// left <= right + tolerance or right <= left + tolerance
		return comparer.LessOrEqual(left, right + tolerance) && comparer.LessOrEqual(right, left + tolerance);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Equal<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Less<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) < 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool LessOrEqual<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) <= 0;
	
	public static IComparer<T> Invert<T>(this IComparer<T> comparer)
	{
		comparer.ThrowIfNull();
		
		return new InvertedComparer<T>(comparer);
	}
}

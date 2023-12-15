namespace AlgorithmsSW;

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

	public static bool Equal<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) == 0;

	public static bool Less<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) < 0;

	public static bool LessOrEqual<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) <= 0;
	
	public static IComparer<T> Invert<T>(this IComparer<T> comparer)
	{
		comparer.ThrowIfNull();
		
		return new InvertedComparer<T>(comparer);
	}
}

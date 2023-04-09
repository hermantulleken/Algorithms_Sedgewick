namespace Algorithms_Sedgewick;

public static class ComparerExtensions
{
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
}

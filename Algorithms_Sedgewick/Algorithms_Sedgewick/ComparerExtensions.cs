﻿using System.Collections.Generic;

namespace Algorithms_Sedgewick;

public static class ComparerExtensions
{
	private sealed class ConvertComparer<TSource, TTarget> : IComparer<TTarget>
	{
		private readonly IComparer<TSource> comparer;
		private readonly Func<TTarget, TSource> converter;

		public ConvertComparer(IComparer<TSource> comparer, Func<TTarget, TSource> converter)
		{
			this.comparer = comparer;
			this.converter = converter;
		}


		public int Compare(TTarget x, TTarget y) => comparer.Compare(converter(x), converter(y));
	}
	public static bool Less<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) < 0;
	
	public static bool Equal<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) == 0;
	
	public static bool LessOrEqual<T>(this IComparer<T> comparer, T left, T right) 
		=> comparer.Compare(left, right) <= 0;

	public static IComparer<TTarget> Convert<TSource, TTarget>(this IComparer<TSource> comparer, Func<TTarget, TSource> converter)
		=> new ConvertComparer<TSource, TTarget>(comparer, converter);
}
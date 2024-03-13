namespace AlgorithmsSW;

[Obsolete("Use lambda expressions instead.")]
/*
	These methods are not so useful when overloads are involved, since we need to specify the types explicitly, so the
	expressions become very long.
*/ 
public static class Functional
{
	public static Func<TResult> ApplyLast<T1, TResult>(Func<T1, TResult> func, T1 arg1)
		=> () => func(arg1);
	
	public static Func<T1, TResult> ApplyLast<T1, T2, TResult>(Func<T1, T2, TResult> func, T2 arg2)
		=> arg1 => func(arg1, arg2);
	
	public static Func<T1, T2, TResult> ApplyLast<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T3 arg3)
		=> (arg1, arg2) => func(arg1, arg2, arg3);
	
	public static Func<T1, T2, T3, TResult> ApplyLast<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T4 arg4)
		=> (arg1, arg2, arg3) => func(arg1, arg2, arg3, arg4);
	
	public static Action ApplyLast<T1>(Action<T1> action, T1 arg1)
		=> () => action(arg1);
	
	public static Action<T1> ApplyLast<T1, T2>(Action<T1, T2> action, T2 arg2)
		=> arg1 => action(arg1, arg2);
	
	public static Action<T1, T2> ApplyLast<T1, T2, T3>(Action<T1, T2, T3> action, T3 arg3)
		=> (arg1, arg2) => action(arg1, arg2, arg3);
	
	public static Action<T1, T2, T3> ApplyLast<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T4 arg4)
		=> (arg1, arg2, arg3) => action(arg1, arg2, arg3, arg4);
}

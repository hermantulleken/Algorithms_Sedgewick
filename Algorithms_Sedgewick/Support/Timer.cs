using System.Diagnostics;

namespace Support;

public static class Timer
{
	public static IList<long> Time(IEnumerable<Action> actions)
	{
		var times = new List<long>();
		foreach (var action in actions)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			action.Invoke();
			stopwatch.Stop();
			times.Add(stopwatch.ElapsedMilliseconds);
		}
		return times;
	}
	
	public static IList<long> Time<T>(
		IEnumerable<Action<T>> actions,
		Func<T> argFactory)
		=> Time(actions.Select<Action<T>, Action>(
			a => () => a(argFactory())));


	public static IList<long> Time<T1, T2>(
		IEnumerable<Action<T1, T2>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2)
		=> Time(actions.Select<Action<T1, T2>, Action>(
			a => () => a(argFactory1(), argFactory2())));


	public static IList<long> Time<T1, T2, T3>(
		IEnumerable<Action<T1, T2, T3>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3)
		=> Time(actions.Select<Action<T1, T2, T3>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3())));


	public static IList<long> Time<T1, T2, T3, T4>(
		IEnumerable<Action<T1, T2, T3, T4>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4)
		=> Time(actions.Select<Action<T1, T2, T3, T4>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4())));


	public static IList<long> Time<T1, T2, T3, T4, T5>(
		IEnumerable<Action<T1, T2, T3, T4, T5>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5)
		=> Time(actions.Select<Action<T1, T2, T3, T4, T5>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5())));


	public static IList<long> Time<T1, T2, T3, T4, T5, T6>(
		IEnumerable<Action<T1, T2, T3, T4, T5, T6>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5,
		Func<T6> argFactory6)
		=> Time(actions.Select<Action<T1, T2, T3, T4, T5, T6>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5(), argFactory6())));

	public static IList<long> Time<T1, T2, T3, T4, T5, T6, T7>(
		IEnumerable<Action<T1, T2, T3, T4, T5, T6, T7>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5,
		Func<T6> argFactory6,
		Func<T7> argFactory7)
		=> Time(actions.Select<Action<T1, T2, T3, T4, T5, T6, T7>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5(), argFactory6(), argFactory7())));

	public static IList<long> Time<T1, T2, T3, T4, T5, T6, T7, T8>(
		IEnumerable<Action<T1, T2, T3, T4, T5, T6, T7, T8>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5,
		Func<T6> argFactory6,
		Func<T7> argFactory7,
		Func<T8> argFactory8)
		=> Time(actions.Select<Action<T1, T2, T3, T4, T5, T6, T7, T8>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5(), argFactory6(), argFactory7(), argFactory8())));

	public static IList<long> Time<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		IEnumerable<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5,
		Func<T6> argFactory6,
		Func<T7> argFactory7,
		Func<T8> argFactory8,
		Func<T9> argFactory9)
		=> Time(actions.Select<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, Action>(
			a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5(), argFactory6(), argFactory7(), argFactory8(), argFactory9())));
	
	public static IList<long> Time<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		IEnumerable<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> actions,
		Func<T1> argFactory1,
		Func<T2> argFactory2,
		Func<T3> argFactory3,
		Func<T4> argFactory4,
		Func<T5> argFactory5,
		Func<T6> argFactory6,
		Func<T7> argFactory7,
		Func<T8> argFactory8,
		Func<T9> argFactory9,
		Func<T10> argFactory10
	) => Time(actions.Select<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, Action>(
		a => () => a(argFactory1(), argFactory2(), argFactory3(), argFactory4(), argFactory5(), argFactory6(), argFactory7(), argFactory8(), argFactory9(), argFactory10())));

}

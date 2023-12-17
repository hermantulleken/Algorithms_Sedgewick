using Support;

namespace Documentation.content;

public class Fibonacci
{
	#region AlgorithmExample

	public static int GetFibonacci_Recursive(int n)
	{
		Tracer.Trace(nameof(GetFibonacci_Recursive), n);
		Tracer.IncLevel();
		
		int result = n switch
		{
			< 0 => throw new ArgumentOutOfRangeException(nameof(n), "Can't be negative."),
			0 => 0,
			1 or 2 => 1,
			_ => GetFibonacci_Recursive(n - 1) + GetFibonacci_Recursive(n - 1),
		};

		Tracer.DecLevel();

		return result;
	}

	#endregion
}

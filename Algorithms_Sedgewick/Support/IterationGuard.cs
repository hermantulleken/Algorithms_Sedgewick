using System.Diagnostics;

namespace Support;

public class Guard(int limit, string limitExceededMessage, string incDecMismatchMessage)
{
	private int counter;
	private int limit;

	public void Reset(int limit)
	{
		counter = 0;
		this.limit = limit;
	}
	
	public void Inc()
	{
		counter++;

		if (counter > limit)
		{
			throw new InvalidOperationException(limitExceededMessage);
		}
	}

	public void Dec()
	{
		counter--;

		if (counter < 0)
		{
			throw new InvalidCastException(incDecMismatchMessage);
		}
	}
}

public static class IterationGuard
{
	private const int DefaultLimit = 1_000_000;
	
	private static readonly Guard Guard 
		= new(
			DefaultLimit,
			"Iteration limit exceeded", 
			"Decrementing counter below zero");
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Reset(int limit = DefaultLimit) => Guard.Reset(limit);
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Inc() => Guard.Inc();
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Dec() => Guard.Dec();
}

public static class RecursionDepthGuard
{
	private const int DefaultLimit = 100;
	
	private static readonly Guard Guard 
		= new(
			DefaultLimit,
			"Iteration limit exceeded", 
			"Decrementing counter below zero");
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Reset(int limit = DefaultLimit) => Guard.Reset(limit);
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Inc() => Guard.Inc();
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Dec() => Guard.Dec();
}

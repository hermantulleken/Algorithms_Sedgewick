namespace Support;

using System.Diagnostics;

/// <summary>
/// Provides methods that can help detect infinite loops in Debug builds. 
/// </summary>
/// <remarks>
/// To use this feature:
/// <list type="number">
///	<item> Call <see cref="Reset"/> before the iteration starts, using a limit that makes sense for your tests.</item>
/// <item> Call <see cref="Inc"/> at the beginning of each iteration. </item>
/// </list>
///
/// If the iteration limit is exceeded, an <see cref="InvalidOperationException"/> is thrown. This makes it easier to
/// use the debugger to find the problem, since the code will break at the point where the iteration limit is exceeded.
///
/// All iteration guard calls use the same guard, so algorithms should not be run in parallel. 
/// </remarks>
/// <example>
/// Here is how the guard can be used in a real example:
/// [!code-csharp[](../../AlgorithmsSW/EdgeWeightedGraph/BoruvkasAlgorithm.cs#GuardExample)]
/// This algorithm relies in the fact that eventually there will only be one connected component. If the algorithm is
/// not implemented correctly, the iteration limit will be exceeded and an exception will be thrown, allowing us to
/// inspect the internal state and see what is going on. 
/// </example>
// TODO: We actually need this to be threadsafe since we want to run Unit tests in parallel.
public static class IterationGuard
{
	private const int DefaultLimit = 1_000_000;
	
	private static readonly Guard Guard 
		= new(
			DefaultLimit,
			"Iteration limit exceeded", 
			"Decrementing counter below zero");
	
	/// <summary>
	/// This resets the counter to zero and sets the limit to the specified value.
	/// </summary>
	/// <param name="limit"></param>
	/// <remarks>Call this before the iteration starts. </remarks>
	[Conditional(Diagnostics.DebugDefine)]
	public static void Reset(int limit = DefaultLimit) => Guard.Reset(limit);
	
	[Conditional(Diagnostics.DebugDefine)]
	public static void Inc() => Guard.Inc();
}

namespace Support;

using System.Diagnostics;

/// <summary>
/// Provides methods that can help detect infinite recursion in Debug builds.
/// </summary>
/// <remarks>
/// To use this feature:
/// <list type="number">
///	<item> Call <see cref="Reset"/> before the recursion starts, using a limit that makes sense for your tests.</item>
/// <item> Call <see cref="Inc"/> when the recursion depth increases. Usually at the beginning of a recursive method. </item>
/// <item> Call <see cref="Dec"/> when the recursion depth decreases. Usually at the end of each return.</item>
/// </list>
///
/// If the recursion limit is exceeded, an <see cref="InvalidOperationException"/> is thrown. This makes it easier to
/// use the debugger to find the problem, since the code will break at the point where the recursion limit is exceeded.
///
/// All recursion guard calls use the same guard, so algorithms should not be run in parallel.
///
/// </remarks>
/// <example>
/// Here is how the guard can be used in a real example:
/// [!code-csharp[](../../AlgorithmsSW/Sort/Sort.cs#RecursionDepthGuardExample)]
/// Note that <see cref="Dec"/>  is called for each exit point of the method.
/// </example>
public static class RecursionDepthGuard
{
	private const int DefaultLimit = 100;
	
	private static readonly Guard Guard 
		= new(
			DefaultLimit,
			"Iteration limit exceeded", 
			"Decrementing counter below zero");
	
	/// <summary>
	/// Resets the counter to zero and sets the limit to the specified value.
	/// </summary>
	/// <param name="limit"></param>
	[Conditional(Diagnostics.DebugDefine)]
	public static void Reset(int limit = DefaultLimit) => Guard.Reset(limit);
	
	/// <summary>
	/// Increases the recursion depth counter.
	/// </summary>
	[Conditional(Diagnostics.DebugDefine), DebuggerHidden]
	public static void Inc() => Guard.Inc();
	
	/// <summary>
	/// Decreases the recursion depth counter.
	/// </summary>
	/*	Hidden from the debugger so that we get the breakpoint at the call site and not here or down the line. For this
		to work Guard.Inc must also be hidden.
	*/
	[Conditional(Diagnostics.DebugDefine)]
	public static void Dec() => Guard.Dec();
}

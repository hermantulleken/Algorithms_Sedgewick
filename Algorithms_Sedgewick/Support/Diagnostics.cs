namespace Support;

using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.Debug;

public static class Diagnostics
{
	public const string WhiteBoxTestingDefine = "WITH_INSTRUMENTATION";
	public const string DebugDefine = "DEBUG";
	public const string ShrinkDynamicContainers = "SHRINK_DYNAMIC_CONTAINERS";

	/// <summary>
	/// An assertion that is questionable.
	/// </summary>
	/// <remarks>Use this in code where a condition looks like it
	/// needs to hold but may not. This is meant to be a temporary assert to help debug or understand code. </remarks>
	public static void Question([DoesNotReturnIf(false)] bool condition) 
		=> Assert(condition);

	/// <inheritdoc cref="Question(bool)"/>
	public static void Question([DoesNotReturnIf(false)] bool condition, string message) 
		=> Assert(condition, message);
}

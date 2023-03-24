namespace Support;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static Tools;

[SuppressMessage(
	"StyleCop.CSharp.NamingRules", 
	"SA1300:Element should begin with upper-case letter", 
	Justification = ShouldBeVisuallyDifferentInCode)]
public static class WhiteBoxTesting
{
#if WHITEBOXTESTING
	/*
		These can be used to examine the inner workings of algorithms.  
	*/
	public static readonly Counter<string> Counter = new();
	public static readonly HashSet<string> Events = new();
#endif
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void __AddCompareTo() => __Add("CompareTo");

	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void __AddPass() => __Add("Pass");

	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void __AddSwap() => __Add("Swap");

	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void __ClearWhiteBoxContainers()
	{
#if WHITEBOXTESTING
		Counter.Clear();
		Events.Clear();
#endif
	}

	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	internal static void __Add(string name)
	{
#if WHITEBOXTESTING
		Counter.Add(name);
#endif
	}
}

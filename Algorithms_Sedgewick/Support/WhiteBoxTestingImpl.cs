using System.Diagnostics.CodeAnalysis;

namespace Support;

#if WHITEBOXTESTING
using System;
using System.Collections.Generic;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = Tools.ShouldBeVisuallyDifferentInCode)]
public static partial class WhiteBoxTesting
{
	public static readonly Counter<string> Counter = new();
	public static readonly HashSet<string> Events = new();

	public static partial void __AddCompareTo() => __Add("CompareTo");

	public static partial void __AddPass() => __Add("Pass");

	public static partial void __AddSwap() => __Add("Swap");

	public static partial void __ClearWhiteBoxContainers()
	{
		Counter.Clear();
		Events.Clear();
	}

	public static partial void __WriteCounts()
	{
		Console.WriteLine(Counter.Counts.Pretty());
	}

	public static partial void __WriteEvents()
	{
		Console.WriteLine(Events.Pretty());
	}

	internal static partial void __Add(string name)
	{
		Counter.Add(name);
	}
}
#endif

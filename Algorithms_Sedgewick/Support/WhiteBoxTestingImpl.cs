namespace Support;

#if WITH_INSTRUMENTATION
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = Tools.ShouldBeVisuallyDifferentInCode)]
public static partial class WhiteBoxTesting
{
	public static readonly HashSet<string> Events = new();
	
	public static readonly ThreadsafeCounter<string> Counter = new(EqualityComparer<string>.Default);

	public static partial void __AddCompareTo() => __Add("CompareTo");

	public static partial void __AddPass() => __Add("Pass");

	public static partial void __AddSwap() => __Add("Swap");

	public static partial void __AddIteration() => __Add("Iteration");

	public static partial void __ClearWhiteBoxContainers()
	{
		Counter.Clear();
		Events.Clear();
	}

	public static partial void __WriteCounts()
	{
		Console.WriteLine(Counter.ToString());
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
#else
using System.Diagnostics;

public static partial class WhiteBoxTesting
{
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __AddCompareTo()
	{
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __AddPass()
	{
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __AddSwap()
	{
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __ClearWhiteBoxContainers()
	{
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __WriteCounts()
	{
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __WriteEvents()
	{
	}

	// ReSharper disable once UnusedParameterInPartialMethod
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	internal static partial void __Add(string name)
	{
	}
	
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static partial void __AddIteration()
	{
	}
}
#endif

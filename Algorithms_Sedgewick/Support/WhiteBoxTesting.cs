using System.Diagnostics.CodeAnalysis;

namespace Support;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = Tools.ShouldBeVisuallyDifferentInCode)]
public static partial class WhiteBoxTesting
{
	public static partial void __AddCompareTo();

	public static partial void __AddPass();

	public static partial void __AddSwap();
	

	public static partial void __AddIteration();

	public static partial void __ClearWhiteBoxContainers();
	
	public static partial void __WriteCounts();

	public static partial void __WriteEvents();

	internal static partial void __Add(string name);
}

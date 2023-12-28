namespace UnitTests;

using System.Collections.Generic;

public class SharedData
{
	public static readonly IComparer<string> StringComparer = Comparer<string>.Default;
	public static readonly IComparer<int> IntComparer = Comparer<int>.Default;
}

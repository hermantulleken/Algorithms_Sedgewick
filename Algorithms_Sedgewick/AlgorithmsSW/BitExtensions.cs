namespace AlgorithmsSW;

using System.Runtime.CompilerServices;

public static class BitExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int BitAt(this int n, int bit) => (n >> bit) & 1;
}

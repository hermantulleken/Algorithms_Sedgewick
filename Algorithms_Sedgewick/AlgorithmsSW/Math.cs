using System.Runtime.CompilerServices;

namespace AlgorithmsSW;

public static class Math2
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int IntegerCeilLog2(int n) 
		=> n <= 1 ? 0 : (int)Math.Ceiling(Math.Log(n, 2));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Sqr(int vertexCount) => vertexCount * vertexCount;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Cub(int vertexCount) => vertexCount * vertexCount * vertexCount;
}

using System.Runtime.CompilerServices;

namespace AlgorithmsSW;

public static class MathX
{
	private const double Epsilon = 0.00001;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int IntegerCeilLog2(int n) 
		=> n <= 1 ? 0 : (int)Math.Ceiling(Math.Log(n, 2));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Sqr(int vertexCount) => vertexCount * vertexCount;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Cub(int vertexCount) => vertexCount * vertexCount * vertexCount;

	public static bool ApproximatelyEqual(double x, double y)
	{
		double difference = Math.Abs(x - y);
		
		return difference < Epsilon;
	}
	
	/// <summary>
	/// Computes the modulus of a number, ensuring a non-negative result.
	/// </summary>
	/// <param name="dividend">The dividend, which can be a negative or positive integer.</param>
	/// <param name="divisor">The divisor, which can be a negative or positive integer, but not zero.</param>
	/// <returns>
	/// The modulus of <paramref name="dividend"/> divided by the absolute value of <paramref name="divisor"/>.
	/// Unlike the standard modulus operator, this function ensures that the
	/// result is always non-negative.
	/// </returns>
	/// <exception cref="System.ArgumentException">
	/// Thrown when <paramref name="divisor"/> is 0.
	/// </exception>
	public static int Mod(int dividend, int divisor) 
	{
		if (divisor == 0)
		{
			throw new ArgumentException("Divisor cannot be zero.", nameof(divisor));
		}

		int positiveDivisor = Math.Abs(divisor);
		int remainder = dividend % positiveDivisor;
		return remainder < 0 ? remainder + positiveDivisor : remainder;
	}

}

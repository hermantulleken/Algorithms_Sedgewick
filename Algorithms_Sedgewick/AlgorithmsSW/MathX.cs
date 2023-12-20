namespace AlgorithmsSW;

using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides math-related utility methods.
/// </summary>
public static class MathX
{
	private const double Epsilon = 0.00001;
	
	/// <summary>
	/// Computes the floor of the base-2 logarithm of a number.
	/// </summary>
	/// <param name="n">The number whose logarithm is to be computed.</param>
	/// <returns>0 if <paramref name="n"/> is 1 or smaller (including negative); otherwise, the floor of the base-2
	/// logarithm of <paramref name="n"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int IntegerCeilLog2(int n)
		=> n <= 1 ? 0 : (int)Math.Ceiling(Math.Log(n, 2));

	/// <summary>
	/// Computes the square of a number.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Sqr<T>(T n)
		where T : INumber<T>
		=> n * n;
	
	/// <summary>
	/// Computes the cube of a number.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Cube<T>(T n)
		where T : INumber<T>
		=> n * n * n;

	/// <summary>
	/// Checks whether two numbers are approximately equal.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ApproximatelyEqual(double a, double b) 
		=> Math.Abs(a - b) < Epsilon;

	/// <summary>
	/// Returns the given numbers as a tuple where the smallest one is first.
	/// </summary>
	/// <param name="a">The first number to compare.</param>
	/// <param name="b">The second number to compare.</param>
	/// <typeparam name="T">The type of the numbers.</typeparam>
	/// <returns>A tuple of the two given numbers where the smallest number is first.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (T min, T max) MinMax<T>(T a, T b)
		where T : IComparisonOperators<T, T, bool>
		=> a < b ? (a, b) : (b, a);
	
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

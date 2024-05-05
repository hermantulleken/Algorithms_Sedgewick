namespace AlgorithmsSW.String;

using System.Text;

public static class RandomStrings
{
	private static readonly Random Random = new();
	
	/// <summary>
	/// Generates a random string of a specified length from the given alphabet.
	/// </summary>
	/// <param name="alphabet">The alphabet to use for generating the string.</param>
	/// <param name="length">The desired length of the string.</param>
	/// <returns>A random string of characters from the specified alphabet.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the alphabet is null.</exception>
	/// <exception cref="ArgumentException">Thrown when the length is negative.</exception>
	[ExerciseReference(5, 1, 2)]
	public static string GenerateRandomString(this Alphabet alphabet, int length)
	{
		alphabet.ThrowIfNull();
		length.ThrowIfNegative();
		
		if (length == 0)
		{
			return string.Empty;
		}
		
		var builder = new StringBuilder(length);
		
		for (int i = 0; i < length; i++)
		{
			builder.Append(alphabet.ToChar(Random.Next(alphabet.Radix)));
		}

		return builder.ToString();
	}
	
	/// <summary>
	/// Generates a random string of a specified length from the given alphabet.
	/// </summary>
	/// <param name="alphabet">The alphabet to use for generating the string.</param>
	/// <param name="minLength">The minimum desired length of the string.</param>
	/// <param name="maxLength">The maximum desired length of the string.</param>
	/// <returns>A random string of characters from the specified alphabet.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the alphabet is null.</exception>
	/// <exception cref="ArgumentException">Thrown when the minimum length is greater than the maximum length.</exception>
	/// <exception cref="ArgumentException">Thrown when the minimum length is negative.</exception>
	/// <exception cref="ArgumentException">Thrown when the maximum length is negative.</exception>
	public static string GenerateRandomString(this Alphabet alphabet, int minLength, int maxLength)
	{
		alphabet.ThrowIfNull();
		minLength.ThrowIfNegative();
		maxLength.ThrowIfNegative();
		
		if (minLength > maxLength)
		{
			throw new ArgumentException("Minimum length cannot be greater than maximum length.");
		}
		
		return alphabet.GenerateRandomString(Random.Next(minLength, maxLength + 1));
	}
}

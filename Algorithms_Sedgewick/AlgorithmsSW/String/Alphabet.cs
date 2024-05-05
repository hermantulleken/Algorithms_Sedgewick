namespace AlgorithmsSW.String;

/// <summary>
/// Represents an alphabet, a subset of Unicode.
/// </summary>
public class Alphabet(string alphabet)
{
	public static readonly Alphabet Binary = new("01");
	public static readonly Alphabet Dna = new("ACTG");
	public static readonly Alphabet Octal = new("01234567");
	public static readonly Alphabet Decimal = new("0123456789");
	public static readonly Alphabet Hexadecimal = new("0123456789ABCDEF");
	public static readonly Alphabet Protein = new("ACDEFGHIKLMNPQRSTVWY");
	public static readonly Alphabet Lowercase = new("abcdefghijklmnopqrstuvwxyz");
	public static readonly Alphabet Uppercase = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
	public static readonly Alphabet Base64 = new("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");
	public static readonly Alphabet Mkhedruli = new("აბგდევზთიკლმნოპჟრსტუფქღყშჩცძწჭხჯჰ");
	
	public int Radix => alphabet.Length;
	
	public int BitCount => (int)Math.Floor(Math.Log(Radix, 2));

	public char ToChar(int index) => alphabet[index];

	public int ToIndex(char c) => alphabet.IndexOf(c);

	public bool Contains(char c) => alphabet.Contains(c);

	public int[] ToIndices(string s)
	{
		int[] indices = new int[s.Length];

		for (int i = 0; i != s.Length; ++i)
		{
			indices[i] = ToIndex(s[i]);
		}

		return indices;
	}

	public string ToChars(int[] indices)
	{
		char[] chars = new char[indices.Length];
		
		for (int i = 0; i != indices.Length; ++i)
		{
			chars[i] = ToChar(indices[i]);
		}

		return new(chars);
	}

	/// <inheritdoc/>
	public override string ToString() => alphabet;
}

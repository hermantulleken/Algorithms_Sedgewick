using System.IO;
using System.Text;

namespace AlgorithmsSW;

/// <summary>
/// Provides extension methods for the <see cref="TextReader"/> class to make it easier to read in collections from
/// a text file.
/// </summary>
public static class TextReaderExtensions
{
	public static string ReadWord(this TextReader reader)
	{
		reader.ThrowIfNull(); // Assuming this method correctly throws an ArgumentNullException if reader is null.

		// Skip initial whitespace
		int c;
		do
		{
			c = reader.Read();
			if (c == -1)
			{
				throw new EndOfStreamException();
			}
		} 
		while (char.IsWhiteSpace((char)c));

		var sb = new StringBuilder();
		do
		{
			sb.Append((char)c);
			c = reader.Read();
		}
		while (c != -1 && !char.IsWhiteSpace((char)c));
		return sb.ToString();
	}
	
	public static void ReadWhiteSpace(this TextReader reader)
	{
		while (!reader.IsEndOfStream())
		{
			if (char.IsWhiteSpace((char) reader.Peek()))
			{
				reader.Read();
			}
		}
	}
	
	public static bool IsEndOfStream(this TextReader reader)
	{
		reader.ThrowIfNull();

		return reader.Peek() == -1;
	}
	
	public static T ReadWordAndConvert<T>(this TextReader reader)
	{
		reader.ThrowIfNull();
		string word = reader.ReadWord();
		return (T)Convert.ChangeType(word, typeof(T));
	}
}

using System.IO;
using Support;

namespace Algorithms_Sedgewick;

public static class TextReaderExtensions
{
	public static int Int(this TextReader reader)
	{
		reader.ThrowIfNull();
		string? line = reader.ReadLine();

		if (line == null)
		{
			ThrowHelper.ThrowEndOfStream();
		}

		return int.Parse(line);
	}

	public static int[] Ints(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(int.Parse).ToArray();
	}

	public static string String(this TextReader reader)
	{
		return reader.ReadLine();
	}

	public static string[] Strings(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space);
	}

	public static double Double(this TextReader reader)
	{
		return double.Parse(reader.ReadLine());
	}

	public static double[] Doubles(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(double.Parse).ToArray();
	}

	public static bool Bool(this TextReader reader)
	{
		return bool.Parse(reader.ReadLine());
	}

	public static bool[] Bools(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(bool.Parse).ToArray();
	}

	public static long Long(this TextReader reader)
	{
		return long.Parse(reader.ReadLine());
	}

	public static long[] Longs(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(long.Parse).ToArray();
	}

	public static float Float(this TextReader reader)
	{
		return float.Parse(reader.ReadLine());
	}

	public static float[] Floats(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(float.Parse).ToArray();
	}

	public static decimal Decimal(this TextReader reader)
	{
		return decimal.Parse(reader.ReadLine());
	}

	public static decimal[] Decimals(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(decimal.Parse).ToArray();
	}

	public static char Char(this TextReader reader)
	{
		return char.Parse(reader.ReadLine());
	}

	public static char[] Chars(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(char.Parse).ToArray();
	}

	public static byte Byte(this TextReader reader)
	{
		return byte.Parse(reader.ReadLine());
	}

	public static byte[] Bytes(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(byte.Parse).ToArray();
	}

	public static short Short(this TextReader reader)
	{
		return short.Parse(reader.ReadLine());
	}

	public static short[] Shorts(this TextReader reader)
	{
		return reader.ReadLine().Split(Formatter.Space).Select(short.Parse).ToArray();
	}
	
	
}
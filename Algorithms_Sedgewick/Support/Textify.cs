namespace Algorithms_Sedgewick;

public static class Textify
{
	public const string Space = " ";
	public const string CommaSpace = ", ";
	public const string SemiColonSpace = "; ";
	public const string ColonSpace = ": ";
	
	public const string NewLine = "\n";
	public const string Tab = "\t";

	public const string NoValueString = "<no value>";
	public const string NullString = "<null>";
	private const string ToStringReturnedNull = "<ToString() returned null>";

	public static string AsText<T>(this T? obj) 
		=> (obj == null ? NullString : obj.ToString()) ?? ToStringReturnedNull;
	
	public static string AsText<T>(this IEnumerable<T>? enumerable, string separator = CommaSpace)
		=> enumerable == null ? NullString : string.Join(separator, enumerable.Select(AsText));
	
	public static string Describe<T, U>(this T label, U description, string separator = ColonSpace)
		=> label.AsText() + separator + description.AsText();
	
	public static string Bracket(this string @string, string leftBracket = "[", string? rightBracket = null)
	{
		rightBracket ??= leftBracket switch
		{
			"(" => ")",
			"[" => "]",
			"{" => "}",
			"<" => ">",
			_ => leftBracket,
		};
		
		return leftBracket + @string + rightBracket;
	}
}

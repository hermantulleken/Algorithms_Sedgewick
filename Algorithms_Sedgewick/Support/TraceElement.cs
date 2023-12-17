namespace Support;

public record TraceElement(int Level, string Name, bool HasValue, string? Value)
{
	private const string IndentString = "  ";
	
	public TraceElement(int Level, string Name)
		: this(Level, Name, false, default)
	{
	}
	
	public TraceElement(int Level, string Name, string value)
		: this(Level, Name, true, value)
	{
	}
	
	/// <inheritdoc/>
	public override string ToString() =>
		(HasValue
			? Name.Describe(Value.AsText())
			: Name)
		.Indent(Level, IndentString);
}

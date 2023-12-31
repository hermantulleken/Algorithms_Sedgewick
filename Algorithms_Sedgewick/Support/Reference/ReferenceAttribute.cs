namespace Support.Reference;

/// <summary>
/// Used to mark code to show where it comes from, or what problem it solves.
/// </summary>
/// <param name="reference">A string that denotes the source of code.</param>
/// <remarks> The source may include problems.</remarks>
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class ReferenceAttribute(string reference) : Attribute
{
	/// <inheritdoc/>
	public override string ToString() => reference;
}

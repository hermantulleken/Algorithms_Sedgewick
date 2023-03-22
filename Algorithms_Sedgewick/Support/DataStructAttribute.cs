namespace Support;


[AttributeUsage(AttributeTargets.Struct)]
public sealed class DataStructAttribute : Attribute
{
	public DataStructAttribute(string justification)
	{
	}
}

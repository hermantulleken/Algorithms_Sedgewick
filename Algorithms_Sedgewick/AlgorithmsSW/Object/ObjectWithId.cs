using AlgorithmsSW.Object;

namespace AlgorithmsSW;

/// <summary>
/// Wrapper that adds an ID to an object.
/// </summary>
public class ObjectWithId : IIdeable
{
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();
		
	public int Id { get; } = IdGenerator.GetNextId();

	public override string ToString() => $"{Id}";
}

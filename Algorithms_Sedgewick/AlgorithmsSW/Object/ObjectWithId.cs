namespace AlgorithmsSW.Object;

/// <summary>
/// An object with an ID. Useful for doing tests where objects are required to be distinguishable. 
/// </summary>
// TODO: This probably should move to Support. 
// TODO: We also need objects where hash functions can be given. 
public class ObjectWithId : IIdeable
{
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();
		
	public int Id { get; } = IdGenerator.GetNextId();

	public override string ToString() => $"{Id}";
}

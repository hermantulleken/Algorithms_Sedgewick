using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick;

/// <summary>
/// Wrapper that adds an ID to an object.
/// </summary>
public class ObjectWithId
{
	public static IComparer<ObjectWithId> Comparer = Comparer<ObjectWithId>.Create((x, y) => x.Id.CompareTo(y.Id));
	
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();
		
	public int Id { get; } = IdGenerator.GetNextId();
}

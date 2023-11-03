namespace Algorithms_Sedgewick.Object;

/// <summary>
/// Class that generates sequential ids.
/// </summary>
public class IdGenerator
{
	private int nextId = -1;

	public int GetNextId()
	{
		nextId++;
		return nextId;
	}
}

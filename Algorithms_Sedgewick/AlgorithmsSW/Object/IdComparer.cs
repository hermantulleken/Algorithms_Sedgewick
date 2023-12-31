namespace AlgorithmsSW.Object;

public class IdComparer : Comparer<IIdeable>
{
	public static readonly IdComparer Instance = new();

	private IdComparer()
	{
	}

	/// <inheritdoc/>
	public override int Compare(IIdeable? x, IIdeable? y) => x == y
		? 0
		: x == null
			? -1
			: y == null
				? 1
				: x.Id.CompareTo(y.Id);
	
	//public override int Compare(IIdeable? x, IIdeable? y) => x.Id - y.Id;
}

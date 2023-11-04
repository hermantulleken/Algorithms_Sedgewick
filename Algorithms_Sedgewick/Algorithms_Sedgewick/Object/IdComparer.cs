﻿namespace Algorithms_Sedgewick.Object;

public class IdComparer : Comparer<IIdeable>
{
	public override int Compare(IIdeable? x, IIdeable? y) => x == y
		? 0
		: x == null
			? -1
			: y == null
				? 1
				: x.Id.CompareTo(y.Id);
}
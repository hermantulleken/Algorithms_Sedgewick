namespace AlgorithmsSW.SymbolTable;

public sealed class PairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
{
	private readonly IComparer<TKey> comparer;

	public PairComparer(IComparer<TKey> comparer) => this.comparer = comparer;

	public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) 
		=> comparer.Compare(x.Key, y.Key);
}

using AlgorithmsSW;
using AlgorithmsSW.HashTable;
using AlgorithmsSW.SearchTrees;
using AlgorithmsSW.SymbolTable;
using Support;
using Timer = Support.Timer;

namespace PerformanceTests;

public class SymbolTablePerformanceTests
{
	public void RunTimeAddKeys() => Run(TimeAddKEysExperiment);
		
	public void RunLookupKeys() => Run(TimeLookupKeysExperiment);
		
	public void Run(Func<int, IList<long>> timeExperiment)
	{
		int start = 1;
		int end = 20;

		var times = new IList<long>[end - start];

		for (int i = start; i < end; i++)
		{
			times[i - start] = timeExperiment(i);

			PrintTable(FactoryTypeNames, times, i - start + 1);
		}
	}
		
	private static readonly string[] FactoryTypeNames = 
	{
		nameof(SymbolTableWithKeyArray<int, int>),
		nameof(SymbolTableWithSelfOrderingKeyArray<int, int>),
		nameof(SymbolTableWithParallelArrays<int, int>),
		nameof(OrderedSymbolTableWithUnorderedLinkedList<int, int>),
		nameof(OrderedSymbolTableWithOrderedArray<int, int>),
		nameof(SymbolTableWithOrderedParallelArray<int, int>),
				
		nameof(OrderedSymbolTableWithOrderedKeyArray<int, int>),
		nameof(OrderedSymbolTableWithOrderedLinkedList<int, int>),
				
		nameof(SymbolTableWithBinarySearchTree<int, int>) + ".Plain",
		nameof(SymbolTableWithBinarySearchTree<int, int>) + ".RedBlack",
				
		nameof(HashTableWithLinearProbing<int, int>),
		nameof(HashTableWithLinearProbing2<int, int>),
		nameof(HashTableWithSeparateChaining<int, int>),
		nameof(HashTableWithSeparateChaining2<int, int>),
		nameof(CuckooHashTable<int, int>),
		nameof(SystemDictionary<int, int>),
	};

	private static Func<ISymbolTable<int, int>>[] GetSymbolTableFactories(int initialCapacity, IComparer<int> comparer)
		=> new Func<ISymbolTable<int, int>>[]
		{
			() => new SymbolTableWithKeyArray<int, int>(initialCapacity, comparer),
			() => new SymbolTableWithSelfOrderingKeyArray<int, int>(initialCapacity, comparer),

			() => new SymbolTableWithParallelArrays<int, int>(initialCapacity, comparer),
			() => new OrderedSymbolTableWithUnorderedLinkedList<int, int>(comparer),

			() => new OrderedSymbolTableWithOrderedArray<int, int>(initialCapacity, comparer),
			() => new SymbolTableWithOrderedParallelArray<int, int>(initialCapacity, comparer),

			() => new OrderedSymbolTableWithOrderedKeyArray<int, int>(initialCapacity, comparer),
			() => new OrderedSymbolTableWithOrderedLinkedList<int, int>(comparer),

			() => new SymbolTableWithBinarySearchTree<int, int>(BinarySearchTree.Plain, comparer),
			() => new SymbolTableWithBinarySearchTree<int, int>(BinarySearchTree.RedBlack, comparer),

			() => new HashTableWithLinearProbing<int, int>(2 * initialCapacity, comparer),
			() => new HashTableWithLinearProbing2<int, int>(2 * initialCapacity, comparer),
			() => new HashTableWithSeparateChaining<int, int>(2 * initialCapacity, comparer),
			() => new HashTableWithSeparateChaining2<int, int>(2 * initialCapacity, comparer),
			() => new CuckooHashTable<int, int>(2 * initialCapacity, comparer),
			() => new SystemDictionary<int, int>(2 * initialCapacity, comparer),
		};
		
	private static void PrintTable(IList<string> names, IList<long>[] times, int end)
	{
		int rows = times[0].Count;

		string table = string.Empty;
			
		for (int j = 0; j < rows; j++)
		{
			table += names[j] + "\t";
				
			for (int i = 0; i < end; i++)
			{
				table += times[i][j] + "\t";
			}

			table += "\n";
		}
			
		Console.WriteLine(Formatter.DottedLine);
		Console.WriteLine(table);
		Console.WriteLine(Formatter.DottedLine);
	}

	private static IList<long> TimeAddKEysExperiment(int n)
	{
		int keysToFindCount = 1000;
		int keysToAddCount = 1 << n;

		(int[] keysToAdd, int[] keysToFind) = GetKeys(keysToAddCount, keysToFindCount);
		var factories = GetSymbolTableFactories(keysToAddCount, Comparer<int>.Default);

		var experiments 
			= factories.Select(factory 
				=> (Action<int[], int[]>)((keysToAdd1, _) 
					=> AddKeysExperiment(factory(), keysToAdd1)));

		var times = Timer.Time(experiments, () => keysToAdd, () => keysToFind);

		return times;
	}

	private static IList<long> TimeLookupKeysExperiment(int n)
	{
		int keysToFindCount = 1000;
		int keysToAddCount = 1 << n;

		(int[] keysToAdd, int[] keysToFind) = GetKeys(keysToAddCount, keysToFindCount);
		var factories = GetSymbolTableFactories(keysToAddCount, Comparer<int>.Default)
			.Select(factory => AddKeys(factory, keysToAdd));

		var experiments 
			= factories.Select(factory 
				=> (Action<int[], int[]>)((_, keysToFind1) 
					=> LookUpKeysExperiment(factory(), keysToFind1)));

		var times = Timer.Time(experiments, () => keysToAdd, () => keysToFind);

		return times;
	}

	private static (int[] keysToAdd, int[] keysToFind) GetKeys(int keysToAddCount, int keysToFindCount)
	{
		int[] keysToAdd = Generator
			.UniformRandomInt(keysToAddCount)
			.Take(keysToAddCount)
			.ToArray();

		int[] keysToFind = Generator
			.UniformRandomInt(keysToAddCount)
			.Take(keysToFindCount)
			.ToArray();
			
		return (keysToAdd, keysToFind);
	}

	private static void AddKeysExperiment(ISymbolTable<int, int> table, int[] keysToAdd)
	{
		foreach (int key in keysToAdd)
		{
			table.Add(key, key);
		}
	}
		
	private static void LookUpKeysExperiment(ISymbolTable<int, int> table, int[] keysToFind)
	{
		int unused = keysToFind.Count(table.ContainsKey);
	}

	private static Func<ISymbolTable<int, int>> AddKeys(Func<ISymbolTable<int, int>> factory, int[] keysToAdd)
	{
		var table = factory();

		foreach (int key in keysToAdd)
		{
			table.Add(key, key);
		}

		return () => table;
	}
}

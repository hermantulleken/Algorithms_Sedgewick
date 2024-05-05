namespace AlgorithmsSW.String;

using System.Numerics;
using Counter;
using List;
using Queue;
using Sort;
using SymbolTable;

/// <summary>
/// A class that provides string sorting algorithms.
/// </summary>
// TODO: Benchmark these algorithms, and also compare them with the standard sorting algorithms.
public static class StringSort
{
	// Assumes the number of different items is small. 
	[ExerciseReference(5, 1, 1)]
	public static void CountSort<T>(IRandomAccessList<T> list, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		comparer.ThrowIfNull();
		
		var counter = list.CountOccurrences(comparer);

		var sortedKeys = counter.Keys.ToRandomAccessList();
		Sort.InsertionSort(sortedKeys, comparer);

		var sortedCounts = sortedKeys
			.Select(key => counter[key])
			.ToRandomAccessList();

		var indexes = Accumulate(sortedCounts);
		var keysToIndex = sortedKeys.Invert(comparer);

		foreach (var str in sortedKeys)
		{
			list[indexes[keysToIndex[str]]++] = str;
		}
	}
	
	public static void RadixSort(IRandomAccessList<int> list, int maxElement)
	{
		list.ThrowIfNull();

		int[] counts = new int[maxElement];

		for (int i = 0; i < list.Count; i++)
		{
			int item = list[i];
			counts[item]++;
		}

		int[] indexes = Accumulate(counts);
		int[] copy = list.ToArray();

		for (int i = 0; i < copy.Length; i++)
		{
			int item = copy[i];
			list[indexes[item]++] = item;
		}
	}

	public static void RadixSort2(IRandomAccessList<int> list, int maxElement)
	{
		list.ThrowIfNull();

		int[] counts = new int[maxElement];

		foreach (int item in list)
		{
			counts[item]++;
		}

		int[] indexes = Accumulate(counts);
		int[] copy = list.ToArray();

		foreach (int item in copy)
		{
			list[indexes[item]++] = item;
		}
	}
	
	// Assumes the number of different items is small. 
	[ExerciseReference(5, 1, 7)]
	public static void CountSortWithQueues<T>(IRandomAccessList<T> list, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		comparer.ThrowIfNull();
		
		var groupItems = DataStructures.HashTable<T, IQueue<T>>(comparer);

		foreach (var item in list)
		{
			if (!groupItems.ContainsKey(item))
			{
				groupItems[item] = DataStructures.Queue<T>();
			}
			
			groupItems[item].Enqueue(item);
		}

		var sortedKeys = groupItems.Keys.ToRandomAccessList();
		
		Sort.InsertionSort(sortedKeys, comparer);

		int index = 0;
		foreach (var item in sortedKeys)
		{
			var queue = groupItems[item];
			
			while (!queue.IsEmpty)
			{
				list[index++] = queue.Dequeue();
			}
		}
	}

	/// <summary>
	/// Counts the occurrences of each unique string in a given collection.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	/// <param name="list">The collection of strings to count.</param>
	/// <param name="comparer">The comparer to use for comparing the strings.</param>
	/// <returns>A <see cref="Counter{T}"/> object where each unique item is associated with its count.</returns>
	public static Counter<T> CountOccurrences<T>(this IEnumerable<T> list, IComparer<T> comparer)
	{
		var counter = new Counter<T>(comparer);

		foreach (var str in list)
		{
			counter.Add(str);
		}

		return counter;
	}

	/// <summary>
	/// Accumulates the counts of the sorted items.
	/// </summary>
	/// <typeparam name="T">The type of the items being sorted.</typeparam>
	/// <param name="sortedCounts">The counts of the sorted items.</param>
	/// <returns>An array of the accumulated counts of the sorted items.</returns>
	public static IRandomAccessList<T> Accumulate<T>(IReadonlyRandomAccessList<T> sortedCounts)
		where T : INumber<T>
	{
		var accumulate = DataStructures.FixedSizeList<T>(sortedCounts.Count + 1);

		accumulate[0] = T.Zero;
		
		for (int i = 0; i < sortedCounts.Count; i++)
		{
			accumulate[i + 1] = accumulate[i] + sortedCounts[i];
		}

		return accumulate;
	}
	
	public static T[] Accumulate<T>(T[] sortedCounts)
		where T : INumber<T>
	{
		var accumulate = new T[sortedCounts.Length + 1];

		accumulate[0] = T.Zero;
		
		for (int i = 0; i < sortedCounts.Length; i++)
		{
			accumulate[i + 1] = accumulate[i] + sortedCounts[i];
		}

		return accumulate;
	}

	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="list">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="radix">The radix of the alphabet used to present the strings.</param>
	/// <remarks>This algorithm assumes that the character integer value is smaller than the radix. So for example,
	/// this implementation cannot be used to sort binary strings made from 0s and 1s. 
	/// </remarks>
	[AlgorithmReference(5, 1)]
	public static void LeastSignificantDigitSort(IRandomAccessList<string> list, int leadingCharsToSort, int radix)
	{
		list.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		radix.ThrowIfNotPositive();
		
		int stringCount = list.Count;

		for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
		{
			string str = list[stringIndex];
			
			if (str.Length < leadingCharsToSort)
			{
				throw new ArgumentException($"string {str} at {stringIndex} is too short.");
			}
		}
		
		string[] aux = new string[stringCount];
		
		// Sort by key-indexed counting on characterIndex-th char.
		for (int charIndex = leadingCharsToSort - 1; charIndex >= 0; charIndex--)
		{ 
			// Why + 1? So that the accumulation below is easier to compute.
			int[] count = new int[radix + 1];
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
			{
				// Why + 1?  So that the accumulation below is easier to compute.
				count[list[stringIndex][charIndex] + 1]++;
			}
			
			// Now count[charCode + 1] has the number of occurrences of charCode among the strings in position characterIndex.

			// Transform counts to indices. Make accumulative.
			for (int charCode = 0; charCode < radix; charCode++)
			{
				count[charCode + 1] += count[charCode];
			}
			
			// Now count[charCode] has the index in aux where the strings with charCode in position charIndex should start.
			
			// Distribute.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				// Why ++? Because we advanced the index as we copy, so the new index is where the next string should go.
				// Is count[strings[stringIndex][characterIndex]] used more than once?
				aux[count[list[stringIndex][charIndex]]++] = list[stringIndex];
			}

			// Copy back.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				list[stringIndex] = aux[stringIndex];
			}
		}
	}
	
	[ExerciseReference(5, 1, 15)]
	public static void Radix2Insertion(IRandomAccessList<int> list, int leadingCharsToSort = 16)
	{
		list.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		const int radix = 2;
		
		if (leadingCharsToSort > sizeof(int) * 8)
		{
			throw new ArgumentOutOfRangeException(
				nameof(leadingCharsToSort), 
				$"leadingCharsToSort must be at most {sizeof(int) * 8}.");
		}
		
		int stringCount = list.Count;
		int[] aux = new int[stringCount];
		
		for (int charIndex = 0; charIndex < leadingCharsToSort; charIndex++)
		{
			int count0 = 0;
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
			{
				if (((list[stringIndex] >> charIndex) & 1) == 0)
				{
					count0++;
				}
			}

			int count1 = count0;
			count0 = 0;
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				if (((list[stringIndex] >> charIndex) & 1) == 0)
				{
					aux[count0++] = list[stringIndex];
				}
				else
				{
					aux[count1++] = list[stringIndex];
				}
			}

			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				list[stringIndex] = aux[stringIndex];
			}
		}
		
		// Sort.InsertionSort(list, Comparer<int>.Default);
		// InsertionSort(list, 0, list.Count - 1, leadingCharsToSort);
	}
	
	[ExerciseReference(5, 1, 15)]
	public static void Radix4Insertion(IRandomAccessList<int> list, int leadingCharsToSort = 16)
	{
		list.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		leadingCharsToSort.ThrowIfOdd();
		const int radix = 4;
		
		if (leadingCharsToSort > sizeof(int) * 8)
		{
			throw new ArgumentOutOfRangeException(
				nameof(leadingCharsToSort), 
				$"leadingCharsToSort must be at most {sizeof(int) * 8}.");
		}
		
		int stringCount = list.Count;
		int[] aux = new int[stringCount];
		int[] count = new int[radix + 1];
		
		for (int charIndex = 0; charIndex < leadingCharsToSort; charIndex += 2)
		{
			count[0] = 0;
			count[1] = 0;
			count[2] = 0;
			count[3] = 0;
			count[4] = 0;
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
			{
				count[((list[stringIndex] >> charIndex) & 3) + 1]++;
			}
			
			count[2] += count[1];
			count[3] += count[2];
			count[4] += count[3];
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				aux[count[(list[stringIndex] >> charIndex) & 3]++] = list[stringIndex];
			}

			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				list[stringIndex] = aux[stringIndex];
			}
		}
		
		// Sort.InsertionSort(list, Comparer<int>.Default);
		// InsertionSort(list, 0, list.Count - 1, leadingCharsToSort);
	}

	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="list">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="radix">The radix of the alphabet used to present the strings.</param>
	/// <remarks>This algorithm assumes that the integer values are smaller than the radix.
	/// This can be used indirectly to sort strings of any <see cref="Alphabet"/> provided they are converted to and
	/// from integer arrays.
	/// </remarks>
	[AlgorithmReference(5, 1)]
	public static void LeastSignificantDigitSort(int[][] list, int leadingCharsToSort, int radix)
	{
		list.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		radix.ThrowIfNotPositive();
		
		int stringCount = list.Length;

		for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
		{
			int[] str = list[stringIndex];
			
			if (str.Length < leadingCharsToSort)
			{
				throw new ArgumentException($"string {str} at {stringIndex} is too short.");
			}
		}
		
		int[][] aux = new int[stringCount][];
		
		// Sort by key-indexed counting on characterIndex-th char.
		for (int charIndex = leadingCharsToSort - 1; charIndex >= 0; charIndex--)
		{ 
			// Why + 1? So that the accumulation below is easier to compute.
			int[] count = new int[radix + 1];
			
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
			{
				// Why + 1?  So that the accumulation below is easier to compute.
				count[list[stringIndex][charIndex] + 1]++;
			}
			
			// Now count[charCode + 1] has the number of occurrences of charCode among the strings in position characterIndex.

			// Transform counts to indices. Make accumulative.
			for (int charCode = 0; charCode < radix; charCode++)
			{
				count[charCode + 1] += count[charCode];
			}
			
			// Now count[charCode] has the index in aux where the strings with charCode in position charIndex should start.
			
			// Distribute.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				// Why ++? Because we advanced the index as we copy, so the new index is where the next string should go.
				// Is count[strings[stringIndex][characterIndex]] used more than once?
				aux[count[list[stringIndex][charIndex]]++] = list[stringIndex];
			}

			// Copy back.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				list[stringIndex] = aux[stringIndex];
			}
		}
	}

	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="list">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="alphabet">The alphabet used in the strings.</param>
	/// <remarks>This algorithm assumes that the integer values are smaller than the radix.
	/// This can be used indirectly to sort strings of any <see cref="Alphabet"/> provided they are converted to and
	/// from integer arrays.
	/// </remarks>
	[AlgorithmReference(5, 1)]
	[ExerciseReference(5, 1, 12)]
	public static void LeastSignificantDigitSort(IRandomAccessList<string> list, int leadingCharsToSort, Alphabet alphabet)
	{
		list.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		
		int[][] stringsAsIntegers = list
			.Select(alphabet.ToIndices)
			.ToArray();
		
		LeastSignificantDigitSort(stringsAsIntegers, leadingCharsToSort, alphabet.Radix);
		
		stringsAsIntegers
			.Select(alphabet.ToChars)
			.CopyTo(list);
	}
	
	/// <summary>
	/// Sorts an array of strings using the Most Significant Digit (MSD) radix sort algorithm.
	/// </summary>
	/// <param name="list">The array of strings to be sorted.</param>
	[AlgorithmReference(5, 2)]
	public static void MostSignificantDigitSort(IRandomAccessList<string> list)
	{
		list.ThrowIfNull();
		int length = list.Count;
		string[] aux = new string[length];
		
		MostSignificantDigitSort(list, 0, length - 1, 0, aux);
	}
	
	/// <summary>
	/// Sorts an array of strings using three-way quicksort.
	/// </summary>
	/// <param name="list">The list of strings to be sorted.</param>
	public static void Quicksort3Way(IRandomAccessList<string> list)
	{
		list.ThrowIfNull();
		
		Sort(0, list.Count - 1, 0);

		return;
		
		void Sort(int startIndex, int endIndex, int characterIndex)
		{
			if (endIndex <= startIndex)
			{
				return;
			}

			// TODO: Benchmark different values
			const int smallArrayCount = 15;

			// TODO: Benchmark whether this actually improves the algorithm
			if (endIndex - startIndex < smallArrayCount)
			{
				InsertionSort(list, startIndex, endIndex, characterIndex);
			}

			int leftEnd = startIndex;
			int rightEnd = endIndex;
			int pivot = CharAtOrSentinel(list[startIndex], characterIndex);
			int i = startIndex + 1;
			
			while (i <= rightEnd)
			{
				int current = CharAtOrSentinel(list[i], characterIndex);
				
				if (current < pivot)
				{
					list.SwapAt(leftEnd++, i++);
				}
				else if (current > pivot)
				{
					list.SwapAt(i, rightEnd--);
				}
				else
				{
					i++;
				}
			}
			
			// strings[startIndex..leftEnd-1] < pivot = strings[leftEnd..rightEnd] < strings[rightEnd+1..endIndex]
			Sort(startIndex, leftEnd - 1, characterIndex);
			
			if (pivot >= 0)
			{
				Sort(leftEnd, rightEnd, characterIndex + 1);
			}
			
			Sort(rightEnd + 1, endIndex, characterIndex);
		}
	}
	
	[ExerciseReference(5, 1, 14)]
	public static void Quicksort3Way(IRandomAccessList<int[]> list)
	{
		list.ThrowIfNull();
		
		Sort(0, list.Count - 1, 0);

		return;
		
		void Sort(int startIndex, int endIndex, int characterIndex)
		{
			if (endIndex <= startIndex)
			{
				return;
			}

			// TODO: Benchmark different values
			const int smallArrayCount = 15;

			// TODO: Benchmark whether this actually improves the algorithm
			if (endIndex - startIndex < smallArrayCount)
			{
				InsertionSort(list, startIndex, endIndex, characterIndex);
			}

			int leftEnd = startIndex;
			int rightEnd = endIndex;
			int pivot = CharAtOrSentinel(list[startIndex], characterIndex);
			int i = startIndex + 1;
			
			while (i <= rightEnd)
			{
				int current = CharAtOrSentinel(list[i], characterIndex);
				
				if (current < pivot)
				{
					list.SwapAt(leftEnd++, i++);
				}
				else if (current > pivot)
				{
					list.SwapAt(i, rightEnd--);
				}
				else
				{
					i++;
				}
			}
			
			// strings[startIndex..leftEnd-1] < pivot = strings[leftEnd..rightEnd] < strings[rightEnd+1..endIndex]
			Sort(startIndex, leftEnd - 1, characterIndex);
			
			if (pivot >= 0)
			{
				Sort(leftEnd, rightEnd, characterIndex + 1);
			}
			
			Sort(rightEnd + 1, endIndex, characterIndex);
		}
	}

	/// <summary>
	/// Sorts an array of strings using the Most Significant Digit (MSD) radix sort algorithm.
	/// </summary>
	/// <param name="list">The array of strings to be sorted.</param>
	/// <param name="startIndex">The starting index in the array for sorting.</param>
	/// <param name="endIndex">The ending index in the array for sorting.</param>
	/// <param name="characterIndex">The character index to consider for sorting.</param>
	/// <param name="aux">Auxiliary array for storing intermediate sorting results.</param>
	private static void MostSignificantDigitSort(
		IRandomAccessList<string> list,
		int startIndex,
		int endIndex,
		int characterIndex,
		string[] aux)
	{
		const int radix = 256;
		const int smallArrayCutoff = 15;
		
		// Sort from a[lo] to a[hi], starting at the dth character.
		if (endIndex <= startIndex + smallArrayCutoff)
		{
			InsertionSort(list, startIndex, endIndex, characterIndex); 
			return;
		}
		
		// Compute frequency counts.
		// Why + 2? Because we want to include the sentinel value -1.
		int[] count = new int[radix + 2];  

		for (int i = startIndex; i <= endIndex; i++)
		{
			count[CharAtOrSentinel(list[i], characterIndex) + 2]++;
		}

		// Transform counts to indices.
		for (int r = 0; r < radix + 1; r++) 
		{
			count[r + 1] += count[r];
		}

		// Distribute.
		// Why + 1 here and not +2 as before? Because we accumulated so that this will work.
		for (int i = startIndex; i <= endIndex; i++)
		{
			aux[count[CharAtOrSentinel(list[i], characterIndex) + 1]++] = list[i];
		}

		// Copy back.
		for (int i = startIndex; i <= endIndex; i++) 
		{
			list[i] = aux[i - startIndex];
		}
		
		// Recursively sort for each character value.
		for (int r = 0; r < radix; r++)
		{
			MostSignificantDigitSort(
				list, 
				startIndex + count[r], 
				startIndex + count[r + 1] - 1, 
				characterIndex + 1,
				aux);
		}
	}
	
	/// <summary>
	/// Sorts an array of strings using the Most Significant Digit (MSD) radix sort algorithm.
	/// </summary>
	/// <param name="list">The array of strings to be sorted.</param>
	/// <param name="startIndex">The starting index in the array for sorting.</param>
	/// <param name="endIndex">The ending index in the array for sorting.</param>
	/// <param name="characterIndex">The character index to consider for sorting.</param>
	/// <param name="aux">Auxiliary array for storing intermediate sorting results.</param>
	private static void MostSignificantDigitSort(
		IRandomAccessList<int[]> list,
		int startIndex,
		int endIndex,
		int characterIndex,
		int[][] aux)
	{
		const int radix = 256;
		const int smallArrayCutoff = 15;
		
		// Sort from a[lo] to a[hi], starting at the dth character.
		if (endIndex <= startIndex + smallArrayCutoff)
		{
			InsertionSort(list, startIndex, endIndex, characterIndex); 
			return;
		}
		
		// Compute frequency counts.
		// Why + 2? Because we want to include the sentinel value -1.
		int[] count = new int[radix + 2];  

		for (int i = startIndex; i <= endIndex; i++)
		{
			count[CharAtOrSentinel(list[i], characterIndex) + 2]++;
		}

		// Transform counts to indices.
		for (int r = 0; r < radix + 1; r++) 
		{
			count[r + 1] += count[r];
		}

		// Distribute.
		// Why + 1 here and not +2 as before? Because we accumulated so that this will work.
		for (int i = startIndex; i <= endIndex; i++)
		{
			aux[count[CharAtOrSentinel(list[i], characterIndex) + 1]++] = list[i];
		}

		// Copy back.
		for (int i = startIndex; i <= endIndex; i++) 
		{
			list[i] = aux[i - startIndex];
		}
		
		// Recursively sort for each character value.
		for (int r = 0; r < radix; r++)
		{
			MostSignificantDigitSort(
				list, 
				startIndex + count[r], 
				startIndex + count[r + 1] - 1, 
				characterIndex + 1,
				aux);
		}
	}
	
	[PageReference(712)]
	private static void InsertionSort(IRandomAccessList<string> list, int startIndex, int endIndex, int charStartIndex)
	{
		for (int i = startIndex; i <= endIndex; i++)
		{
			for (int j = i; j > startIndex && Less(list[j], list[j - 1], charStartIndex); j--)
			{
				list.SwapAt(j, j - 1);
			}
		}
	}
	
	private static void InsertionSort(IRandomAccessList<int> list, int startIndex, int endIndex, int charStartIndex)
	{
		for (int i = startIndex; i <= endIndex; i++)
		{
			for (int j = i; j > startIndex && Less(list[j], list[j - 1], charStartIndex); j--)
			{
				list.SwapAt(j, j - 1);
			}
		}
	}
	
	private static void InsertionSort(IRandomAccessList<int[]> list, int startIndex, int endIndex, int charStartIndex)
	{
		for (int i = startIndex; i <= endIndex; i++)
		{
			for (int j = i; j > startIndex && Less(list[j], list[j - 1], charStartIndex); j--)
			{
				list.SwapAt(j, j - 1);
			}
		}
	}

	// This version helps us deal with strings of different lengths by returning 
	// a sentinel for out of range characters
	private static int CharAtOrSentinel(string str, int index) => index < str.Length ? str[index] : -1;
	
	private static int CharAtOrSentinel(int[] str, int index) => index < str.Length ? str[index] : -1;
	
	// This version compares strings only starting at startCharacterIndex
	private static bool Less(string str1, string str2, int startCharacterIndex) 
		=> string.Compare(str1[startCharacterIndex..], str2[startCharacterIndex..], StringComparison.Ordinal) < 0;
	
	private static bool Less(int str1, int str2, int startCharacterIndex) 
		=> str1 >> startCharacterIndex < str2 >> startCharacterIndex;
	
	private static bool Less(int[] str1, int[] str2, int startCharacterIndex) 
		=> Less(str1[startCharacterIndex..], str2[startCharacterIndex..]);

	private static bool Less(int[] str1, int[] str2)
	{
		int end = Math.Max(str1.Length, str2.Length);

		for (int i = 0; i < end; i++)
		{
			if (CharAtOrSentinel(str1, i) < CharAtOrSentinel(str2, i))
			{
				return true;
			}
		}

		return false;
	}
}

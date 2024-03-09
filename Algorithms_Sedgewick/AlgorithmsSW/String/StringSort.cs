namespace AlgorithmsSW.String;

using List;

/// <summary>
/// A class that provides string sorting algorithms.
/// </summary>
public static class StringSort
{
	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="strings">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="radix">The radix of the alphabet used to present the strings.</param>
	/// <remarks>This algorithm assumes that the character integer value is smaller than the radix. So for example,
	/// this implementation cannot be used to sort binary strings made from 0s and 1s. 
	/// </remarks>
	[AlgorithmReference(5, 1)]
	public static void LeastSignificantDigitSort(string[] strings, int leadingCharsToSort, int radix)
	{
		strings.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		radix.ThrowIfNotPositive();
		
		int stringCount = strings.Length;

		for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
		{
			string str = strings[stringIndex];
			
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
				count[strings[stringIndex][charIndex] + 1]++;
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
				aux[count[strings[stringIndex][charIndex]]++] = strings[stringIndex];
			}

			// Copy back.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				strings[stringIndex] = aux[stringIndex];
			}
		}
	}
	
	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="strings">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="radix">The radix of the alphabet used to present the strings.</param>
	/// <remarks>This algorithm assumes that the integer values are smaller than the radix.
	/// This can be used indirectly to sort strings of any <see cref="Alphabet"/> provided they are converted to and
	/// from integer arrays.
	/// </remarks>
	[AlgorithmReference(5, 1)]
	public static void LeastSignificantDigitSort(int[][] strings, int leadingCharsToSort, int radix)
	{
		strings.ThrowIfNull();
		leadingCharsToSort.ThrowIfNotPositive();
		radix.ThrowIfNotPositive();
		
		int stringCount = strings.Length;

		for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
		{
			int[] str = strings[stringIndex];
			
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
				count[strings[stringIndex][charIndex] + 1]++;
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
				aux[count[strings[stringIndex][charIndex]]++] = strings[stringIndex];
			}

			// Copy back.
			for (int stringIndex = 0; stringIndex < stringCount; stringIndex++) 
			{
				strings[stringIndex] = aux[stringIndex];
			}
		}
	}

	/// <summary>
	/// Sorts an array of strings using the Least Significant Digit (LSD) radix sort algorithm,
	/// considering only a specified number of leading characters in each string.
	/// </summary>
	/// <param name="strings">The array of strings to be sorted. This algorithm assumes all the strings are of length
	/// at least <paramref name="leadingCharsToSort"/>.</param>
	/// <param name="leadingCharsToSort">The number of leading characters to consider during sorting.</param>
	/// <param name="alphabet">The alphabet used in the strings.</param>
	/// <remarks>This algorithm assumes that the integer values are smaller than the radix.
	/// This can be used indirectly to sort strings of any <see cref="Alphabet"/> provided they are converted to and
	/// from integer arrays.
	/// </remarks>
	[AlgorithmReference(5, 1)]
	public static void LeastSignificantDigitSort(string[] strings, int leadingCharsToSort, Alphabet alphabet)
	{
		int[][] stringsAsIntegers = strings
			.Select(alphabet.ToIndices)
			.ToArray();
		
		LeastSignificantDigitSort(stringsAsIntegers, leadingCharsToSort, alphabet.Radix);
		
		stringsAsIntegers
			.Select(alphabet.ToChars)
			.CopyTo(strings);
	}
	
	/// <summary>
	/// Sorts an array of strings using the Most Significant Digit (MSD) radix sort algorithm.
	/// </summary>
	/// <param name="strings">The array of strings to be sorted.</param>
	[AlgorithmReference(5,2)]
	public static void MostSignificantDigitSort(string[] strings)
	{
		int length = strings.Length;
		string[] aux = new string[length];
		
		MostSignificantDigitSort(strings, 0, length - 1, 0, aux);
	}
	
	

	/// <summary>
	/// Sorts an array of strings using the Most Significant Digit (MSD) radix sort algorithm.
	/// </summary>
	/// <param name="strings">The array of strings to be sorted.</param>
	/// <param name="startIndex">The starting index in the array for sorting.</param>
	/// <param name="endIndex">The ending index in the array for sorting.</param>
	/// <param name="characterIndex">The character index to consider for sorting.</param>
	/// <param name="aux">Auxiliary array for storing intermediate sorting results.</param>
	private static void MostSignificantDigitSort(
		string[] strings,
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
			InsertionSort(strings, startIndex, endIndex, characterIndex); 
			return;
		}
		
		// Compute frequency counts.
		int[] count = new int[radix + 2]; 

		for (int i = startIndex; i <= endIndex; i++)
		{
			count[CharAt(strings[i], characterIndex) + 2]++;
		}

		// Transform counts to indices.
		for (int r = 0; r < radix + 1; r++) 
		{
			count[r + 1] += count[r];
		}

		// Distribute.
		for (int i = startIndex; i <= endIndex; i++)
		{
			aux[count[CharAt(strings[i], characterIndex) + 1]++] = strings[i];
		}

		// Copy back.
		for (int i = startIndex; i <= endIndex; i++) 
		{
			strings[i] = aux[i - startIndex];
		}
		
		// Recursively sort for each character value.
		for (int r = 0; r < radix; r++)
		{
			MostSignificantDigitSort(
				strings, 
				startIndex + count[r], 
				startIndex + count[r + 1] - 1, 
				characterIndex + 1,
				aux);
		}
	}
	
	[PageReference(712)]
	private static void InsertionSort(string[] strings, int startIndex, int endIndex, int charStartIndex)
	{
		for (int i = startIndex; i <= endIndex; i++)
		{
			for (int j = i; j > startIndex && Less(strings[j], strings[j - 1], charStartIndex); j--)
			{
				strings.SwapAt(j, j - 1);
			}
		}
	}

	// This version helps us deal with strings of different lengths by returning 
	// a sentinel for out of range characters
	private static int CharAt(string str, int index) => index < str.Length ? str[index] : -1;
	
	// This version compares strings only starting at startCharacterIndex
	private static bool Less(string str1, string str2, int startCharacterIndex) 
		=> string.Compare(str1[startCharacterIndex..], str2.Substring(startCharacterIndex), StringComparison.Ordinal) < 0;
	
	public static void Quicksort3Way(string[] strings) 
		=> Quicksort3Way(strings, 0, strings.Length - 1, 0);

	private static void Quicksort3Way(string[] strings, int startIndex, int endIndex, int characterIndex)
	{
		if (endIndex <= startIndex)
		{
			return;
		}

		int leftEnd = startIndex;
		int rightEnd = endIndex;
		int pivot = CharAt(strings[startIndex], characterIndex);
		int i = startIndex + 1;
			
		while (i <= rightEnd)
		{
			int current = CharAt(strings[i], characterIndex);
				
			if (current < pivot)
			{
				strings.SwapAt(leftEnd++, i++);
			}
			else if (current > pivot)
			{
				strings.SwapAt(i, rightEnd--);
			}
			else
			{
				i++;
			}
		}
			
		// strings[startIndex..leftEnd-1] < pivot = strings[leftEnd..rightEnd] < strings[rightEnd+1..endIndex]
		Quicksort3Way(strings, startIndex, leftEnd - 1, characterIndex);
			
		if (pivot >= 0)
		{
			Quicksort3Way(strings, leftEnd, rightEnd, characterIndex + 1);
		}
			
		Quicksort3Way(strings, rightEnd + 1, endIndex, characterIndex);
	}
}

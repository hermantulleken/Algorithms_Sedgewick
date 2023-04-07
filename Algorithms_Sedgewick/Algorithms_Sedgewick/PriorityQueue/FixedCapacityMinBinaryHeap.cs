namespace Algorithms_Sedgewick.PriorityQueue;

using System.Diagnostics;
using List;
using Support;
using static System.Diagnostics.Debug;

/// <summary>
/// A container that allows efficient insertions and
/// retrieval of the minimum element. 
/// </summary>
// Note: This is a min binary heap, so comparisons in sink and swim are inverted compared to text book
// Note: The first element in the array is not used
public sealed class FixedCapacityMinBinaryHeap<T> : IPriorityQueue<T> 
	where T : IComparable<T>
{
	private const string EmptyHeapPresentation = "()";

#if WITH_INSTRUMENTATION
	private const string InvalidStateMarker = "~";
#endif
	
	private const int StartIndex = 1;
	
	private readonly T[] items;

#if WITH_INSTRUMENTATION
	private bool isStateValid = true;
#endif

	public int Count { get; private set; }
	
	public int Capacity { get; private set; }
	
	public bool IsEmpty => Count == 0;
	
	public bool IsSingleton => Count == 1;
	
	public bool IsFull => Count == Capacity;

	public T PeekMin
	{
		get
		{
			if (IsEmpty)
			{
				ThrowHelper.ThrowContainerEmpty();
			}
			
			return items[StartIndex];
		}
	}

#if WITH_INSTRUMENTATION
	private bool IsReferenceType { get; }
#endif
	
	public FixedCapacityMinBinaryHeap(int capacity)
	{
		Capacity = capacity;
		
		// We have one extra space that is not used to make the calculations simpler
		items = new T[StartIndex + capacity];
		Count = 0;
		
#if WITH_INSTRUMENTATION
		IsReferenceType = typeof(T).IsClass;
#endif
	}

	/// <summary>
	/// Pushes a new element onto the heap.
	/// </summary>
	/// <exception cref="InvalidOperationException">the heap is full.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
	// Question: should we allow null? Yes, no reason not to.
	public void Push(T item)
	{
		item.ThrowIfNull();
		
		if (IsFull)
		{
			ThrowHelper.ThrowContainerFull();
		}

		SetStateInvalid();
		
		Count++;
		items[Count] = item;
		
		if (Count > 1)
		{
			Swim(Count); // Assumes that swim does not depend on the value of count.
		}
		
		SetStateValid();
		AssertSatisfyHeapProperty();
		AssertUnusedEmptyIfReferenceType();
	}

	/// <summary>
	/// Retrieves and removes the minimum element in the heap.
	/// </summary>
	/// <exception cref="InvalidOperationException">the heap is empty.</exception>
	public T PopMin()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		SetStateInvalid();
		var min = items[StartIndex];
		Count--;
		
		if (IsEmpty)
		{
			items[StartIndex] = default;
		}
		else if (Count > 0)
		{
			MoveAt(Count + 1, StartIndex);

			if (Count > 1)
			{
				Sink(StartIndex);
			}
		}
		
		SetStateValid();
		AssertSatisfyHeapProperty();
		AssertUnusedEmptyIfReferenceType();

		return min;
	}

#if WITH_INSTRUMENTATION
	public bool SatisfyHeapProperty() => SatisfyHeapProperty(StartIndex);
	
	public string ToDebugString() => isStateValid ? ToPrettyString() : InvalidStateMarker + ToPrettyString();
	
	public override string ToString() => ToDebugString();
	
#else
	public override string ToString() => ToPrettyString();
#endif
	
	public string ToPrettyString()
		=> IsEmpty 
			? EmptyHeapPresentation
			: IsSingleton
				? AddBrackets(ToPrettyString(StartIndex))
				: ToPrettyString(StartIndex);

	private string AddBrackets(string str) => $"({str})";

	private bool LessAt(int i, int j) => ListExtensions.LessAt(items, i, j);

	private void SwapAt(int i, int j) => ListExtensions.SwapAt(items, i, j);

	private void MoveAt(int sourceIndex, int destinationIndex) => ListExtensions.MoveAt(items, sourceIndex, destinationIndex);

	private void Swim(int k)
	{
		Assert(k > StartIndex);
		
		while (k > StartIndex && LessAt(k, k / 2))
		{
			SwapAt(k / 2, k);
			k /= 2;
		}
	}

	private void Sink(int k)
	{
		Assert(Count != 1);
		
		while (2 * k <= Count)
		{
			int j = 2 * k;
			
			if (j < Count && LessAt(j + 1, j))
			{
				j++;
			}

			if (!LessAt(j, k))
			{
				break;
			}
			
			SwapAt(k, j);
			k = j;
		}
	}

	private string ToPrettyString(int k)
	{
		Assert(k != 0);
		Assert(!IsEmpty);
		
		int leftChild = 2 * k;
		int rightChild = leftChild + 1;

		return leftChild > Count
			? items[k].ToString()
			: rightChild > Count
				? $"({items[k]}, {ToPrettyString(leftChild)}, . )"
				: $"({items[k]}, {ToPrettyString(leftChild)}, {ToPrettyString(rightChild)})";
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public void AssertSatisfyHeapProperty()
	{
#if WITH_INSTRUMENTATION
		Assert(SatisfyHeapProperty());
#endif
	}	
	
#if WITH_INSTRUMENTATION
	private bool SatisfyHeapProperty(int k)
	{
		if (IsEmpty || IsSingleton)
		{
			return true;
		}
		
		Assert(isStateValid); // Otherwise the heap property is not supposed to hold
		
		int leftChild = 2 * k;
		int rightChild = leftChild + 1;
		
		if (leftChild > Count)
		{
			// Does not have a left child, and therefore also not a right child
			Assert(rightChild >= Count);
			
			return true;
		}

		if (LessAt(leftChild, k))
		{
			return false;
		}
		
		if (rightChild > Count)
		{
			// does not have a right child
			return true;
		}

		if (LessAt(rightChild, k))
		{
			return false;
		}

		return true;
	}
	
	// We check this to make sure there are no orphan objects. 
	private void AssertUnusedNull()
	{
		Assert(IsReferenceType);
		
		// ReSharper disable CompareNonConstrainedGenericWithNull
		Assert(items[0] == null);
		
		for (int i = Count + StartIndex; i < items.Length; i++)
		{
			Assert(items[i] == null);
		}
		// ReSharper restore CompareNonConstrainedGenericWithNull
	}
#endif
	
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	private void AssertUnusedEmptyIfReferenceType()
	{
#if WITH_INSTRUMENTATION
		if (IsReferenceType)
		{
			AssertUnusedNull();
		}
#endif
	}
	
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	private void SetStateValid()
	{
#if WITH_INSTRUMENTATION
		isStateValid = true;
#endif
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	private void SetStateInvalid()
	{
#if WITH_INSTRUMENTATION
		isStateValid = false;
#endif
	}

	// This op is O(n) 
	public T PopMax()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		if (IsSingleton)
		{
			return PopMin();
		}

		int firstLeaf = GetFirstLeaveIndex();
		int maxIndex = items.FindIndexOfMax(firstLeaf, Count + 1);

		var max = items[maxIndex];
		items[maxIndex] = items[Count];
		Swim(maxIndex);
		Count--;
		items[Count] = default;
		
		return max;
	}

	private int GetFirstLeaveIndex()
	{
		Assert(!IsEmpty);
		int lastIndex = Count;

		// 1 -> 1
		// 2 -> 2
		// 3 -> 2
		// 4 -> 3

		return IsSingleton ? StartIndex : lastIndex / 2 + 1;
	}
}

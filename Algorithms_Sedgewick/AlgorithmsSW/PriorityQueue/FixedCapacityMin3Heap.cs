using System.Diagnostics;
using AlgorithmsSW.List;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.PriorityQueue;

using System.Collections;

public class FixedCapacityMin3Heap<T> : IPriorityQueue<T>
{
	private const int Base = 3;
	private const string EmptyHeapPresentation = "()";

#if WITH_INSTRUMENTATION
	private const string InvalidStateMarker = "~";
#endif
	
	private readonly T[] items;
	private readonly IComparer<T> comparer;

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
			
			return items[0];
		}
	}

#if WITH_INSTRUMENTATION
	private bool IsReferenceType { get; }
#endif
	
	public FixedCapacityMin3Heap(int capacity, IComparer<T> comparer)
	{
		Capacity = capacity;
		this.comparer = comparer;	
		// We have one extra space that is not used to make the calculations simpler
		items = new T[capacity];
		Count = 0;
		
#if WITH_INSTRUMENTATION
		IsReferenceType = typeof(T).IsClass;
#endif
	}

	/// <summary>
	/// Pushes a new element onto the heap.
	/// </summary>
	/// <exception cref="InvalidOperationException">the heap is full.</exception>
	// Question: should we allow null? Yes, no reason not to.
	public void Push(T item)
	{
		item.ThrowIfNull();
		
		if (IsFull)
		{
			ThrowHelper.ThrowContainerFull();
		}

		SetStateInvalid();
		
		items[Count] = item;

		if (Count > 0)
		{
			Swim(Count); // Assumes that swim does not depend on the value of count.
		}
		
		Count++;
		
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
		var min = items[0];
		Count--;
		
		if (IsEmpty)
		{
			items[0] = default;
		}
		else if (Count > 0)
		{
			MoveAt(Count, 0);

			if (Count > 1)
			{
				Sink(0);
			}
		}
		
		SetStateValid();
		AssertSatisfyHeapProperty();
		AssertUnusedEmptyIfReferenceType();

		return min;
	}

#if WITH_INSTRUMENTATION
	public bool SatisfyHeapProperty() => SatisfyHeapProperty(0);
	
	public string ToDebugString() => isStateValid ? ToPrettyString() : InvalidStateMarker + ToPrettyString();
	
	public override string ToString() => ToDebugString();
	
#else
	/// <inheritdoc/>
	public override string ToString() => ToPrettyString();
#endif

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => items.Take(Count).GetEnumerator();
	
	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public string ToPrettyString()
		=> IsEmpty 
			? EmptyHeapPresentation
			: IsSingleton
				? AddBrackets(ToPrettyString(0))
				: ToPrettyString(0);

	private string AddBrackets(string str) => $"({str})";

	private bool LessAt(int i, int j) => ListExtensions.LessAt(items, i, j, comparer);

	private void SwapAt(int i, int j) => ListExtensions.SwapAt(items, i, j);

	private void MoveAt(int sourceIndex, int destinationIndex) => ListExtensions.MoveAt(items, sourceIndex, destinationIndex);

	private void Swim(int k)
	{
		Assert(k > 0);

		int parent = GetParentIndex(k);
		
		while (k > 0 && LessAt(k, parent))
		{
			SwapAt(parent, k);
			k = parent;
			parent = GetParentIndex(k);
		}
	}

	private int IndexOfMinAt(int index0, int index1, int index2)
	{
		Assert(index0 < Count);
		Assert(index0 < index1);
		Assert(index1 < index2);

		if (index1 >= Count)
		{
			return index0;
		}

		int indexOfMin = LessAt(index0, index1) ? index0 : index1;

		if (index2 >= Count)
		{
			return indexOfMin;
		}

		indexOfMin = LessAt(indexOfMin, index2) ? indexOfMin : index2;

		return indexOfMin;
	}
	
	private void Sink(int k)
	{
		Assert(Count != 1);
		int leftChild = GetChildIndex(k);
		
		while (leftChild < Count)
		{
			int minChild = IndexOfMinAt(leftChild, leftChild + 1, leftChild + 2);

			if (LessAt(minChild, k))
			{
				SwapAt(k, minChild);
			}
			else
			{
				break;
			}
			
			k = minChild;
			leftChild = GetChildIndex(k);
		}
	}

	private string ToPrettyString(int k)
	{
		Assert(!IsEmpty);
		
		int child0 = GetChildIndex(k);
		int child1 = child0 + 1;
		int child2 = child1 + 1;

		if (child0 >= Count)
		{
			return items[k].ToString();
		}

		if (child1 >= Count)
		{
			return $"({items[k]}, {ToPrettyString(child0)}, . .)";
		}

		if (child2 >= Count)
		{
			return $"({items[k]}, {ToPrettyString(child0)}, {ToPrettyString(child1)}, .)";
		}

		return $"({items[k]}, {ToPrettyString(child0)}, {ToPrettyString(child1)}, {ToPrettyString(child2)})";
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public void AssertSatisfyHeapProperty()
	{
#if WITH_INSTRUMENTATION
		Assert(SatisfyHeapProperty());
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
		int maxIndex = items.FindIndexOfMax(firstLeaf, Count + 1, comparer);

		var max = items[maxIndex];
		items[maxIndex] = items[Count];
		Swim(maxIndex);
		Count--;
		items[Count] = default;
		
		return max;
	}
	
#if WITH_INSTRUMENTATION
	private bool SatisfyHeapProperty(int k)
	{
		if (IsEmpty || IsSingleton)
		{
			return true;
		}
		
		Assert(isStateValid); // Otherwise the heap property is not supposed to hold
		
		int child0 = GetChildIndex(k);
		int child1 = child0 + 1;
		int child2 = child1 + 1;
		
		if (child0 >= Count)
		{
			return true;
		}

		if (LessAt(child0, k))
		{
			return false;
		}
		
		if (child1 >= Count)
		{
			return true;
		}

		if (LessAt(child1, k))
		{
			return false;
		}
		
		if (child2 >= Count)
		{
			return true;
		}

		if (LessAt(child2, k))
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
		for (int i = Count; i < items.Length; i++)
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

	private int GetFirstLeaveIndex()
	{
		Assert(!IsEmpty);
		int lastIndex = Count - 1;

		return IsSingleton ? 0 : GetParentIndex(lastIndex) + 1;
	}

	private static int GetChildIndex(int index) => Base * index + 1;

	// 1 -> 0, 2-> 0, 3 -> 0
	private static int GetParentIndex(int index) => (index - 1) / Base;
}

namespace AlgorithmsSW.PriorityQueue;

using System.Collections;
using System.Diagnostics;
using System.Text;
using List;
using Support;
using static System.Diagnostics.Debug;

public class FixedCapacityMinNHeap<T> : IPriorityQueue<T>
{
	private const string EmptyHeapPresentation = "()";

#if WITH_INSTRUMENTATION
	private const string InvalidStateMarker = "~";
#endif
	
	private readonly int Base = 3;
	
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
	
	public FixedCapacityMinNHeap(int @base, int capacity, IComparer<T> comparer)
	{
		Base = @base;
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

	private int IndexOfMinAt(int index0, int count)
	{
		int minIndex = index0;

		for (int i = 1; i < count; i++)
		{
			if (LessAt(index0 + i, minIndex))
			{
				minIndex = index0 + i;
			}
		}

		return minIndex;
	}
	
	private void Sink(int k)
	{
		Assert(Count != 1);
		int leftChild = GetChildIndex(k);
		
		while (leftChild < Count)
		{
			int childrenCount = Math.Min(Base, Count - leftChild);
			Assert(childrenCount != 0);
			int minChild = IndexOfMinAt(leftChild, childrenCount);

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

	public string ToPrettyString()
	{
		return IsEmpty ? EmptyHeapPresentation : ToPrettyString(0);
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

		Assert(isStateValid); // The heap property should be valid

		int firstChild = GetChildIndex(k);
		for (int i = 0; i < Base; i++)
		{
			int childIndex = firstChild + i;
			if (childIndex >= Count)
			{
				// No more children, so this subtree satisfies the heap property
				break;
			}

			if (LessAt(childIndex, k))
			{
				// Child is less than the node, which violates the min-heap property
				return false;
			}
		}

		// Check the subtrees of each child recursively
		for (int i = 0; i < Base; i++)
		{
			int childIndex = firstChild + i;
			if (childIndex < Count && !SatisfyHeapProperty(childIndex))
			{
				return false;
			}
		}

		return true; // This subtree satisfies the heap property
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

	private int GetChildIndex(int index) => Base * index + 1;

	// 1 -> 0, 2-> 0, 3 -> 0
	private int GetParentIndex(int index) => (index - 1) / Base;
	
	private string ToPrettyString(int k)
	{
		// Base case: if the current index is out of bounds
		if (k >= Count)
		{
			return string.Empty;
		}

		// Start with the current node's value
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(items[k].AsText());

		// Check if there are children
		int firstChild = GetChildIndex(k);
		
		if (firstChild >= Count)
		{
			return stringBuilder.ToString();
		}

		// Add each child's pretty string
		stringBuilder.Append(Formatter.CommaSpace);
		stringBuilder.Append("[");
		
		for (int i = 0; i < Base; i++)
		{
			int childIndex = firstChild + i;
			
			if (childIndex < Count)
			{
				if (i > 0)
				{
					stringBuilder.Append(Formatter.CommaSpace);
				}
				
				stringBuilder.Append(ToPrettyString(childIndex));
			}
		}
		
		stringBuilder.Append("]");

		return stringBuilder.ToString();
	}
}

using System.Collections.Generic;
using Algorithms_Sedgewick;

namespace UnitTests;

using NUnit.Framework;

[TestFixture]
public class HashSetTests
{
	// 1. Constructor Tests
	[Test]
	public void DefaultConstructor_InitialState()
	{
		// #1
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		Assert.That(hashSet.Count, Is.EqualTo(0));
	}

	[Test]
	public void Constructor_WithInitialCapacity()
	{
		// #2
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(10, Comparer<int>.Default);
		Assert.That(hashSet.Count, Is.EqualTo(0)); // Assuming the table size is accessible.
	}
	
	// 2. Add Method
	[Test]
	public void Add_SingleItem()
	{
		// #4
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		Assert.That(hashSet.Count, Is.EqualTo(1));
	}

	[Test]
	public void Add_SameItemTwice()
	{
		// #5
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(1);
		Assert.That(hashSet.Count, Is.EqualTo(1));
	}

	[Test]
	public void Add_MultipleItems()
	{
		// #6
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		Assert.That(hashSet.Count, Is.EqualTo(2));
	}

	[Test]
	public void Add_UntilResize()
	{
		// #7
		// This depends on the internal details of HashSet resizing mechanism.

		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(10, Comparer<int>.Default);
		for (int i = 0; i < 16; i++) // table size for 10 is 31, so we need to go more than half that
		{
			hashSet.Add(i);
		}
		Assert.That(hashSet.Count, Is.EqualTo(16));
	}

	// 3. Contains Method
	[Test]
	public void Contains_ItemNotInSet()
	{
		// #8
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		Assert.That(hashSet.Contains(1), Is.False);
	}

	[Test]
	public void Contains_AfterAdd()
	{
		// #9
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		Assert.That(hashSet.Contains(1), Is.True);
	}

	[Test]
	public void Contains_MultipleAddedItems()
	{
		// #10
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		Assert.That(hashSet.Contains(1) && hashSet.Contains(2), Is.True);
	}

	// 4. Remove Method
	[Test]
	public void Remove_NonExistingItem()
	{
		// #12
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		Assert.That(() => hashSet.Remove(1), Throws.TypeOf<KeyNotFoundException>());
	}

	[Test]
	public void Remove_SingleItem()
	{
		// #13
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Remove(1);
		Assert.That(hashSet.Count, Is.EqualTo(0));
	}

	[Test]
	public void Remove_OneOfMultipleItems()
	{
		// #14
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		hashSet.Remove(1);
		Assert.That(hashSet.Contains(2), Is.True);
	}
	
	[Test]
	public void Remove_OneOfMultipleItems_Object()
	{
		// #14
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<ObjectWithId>(ObjectWithId.Comparer);

		var obj1 = new ObjectWithId();
		var obj2 = new ObjectWithId();
		
		hashSet.Add(obj1);
		hashSet.Add(obj2);
		hashSet.Remove(obj1);
		Assert.That(hashSet.Contains(obj1), Is.False);
		Assert.That(hashSet.Contains(obj2), Is.True);
	}

	// This test assumes knowledge about the internal details of the hash set.
	[Test]
	public void Remove_UntilResize()
	{
		// #15
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(20, Comparer<int>.Default);
		for (int i = 0; i < 10; i++)
		{
			hashSet.Add(i);
		}
		for (int i = 0; i < 8; i++)
		{
			hashSet.Remove(i);
		}
		Assert.That(hashSet.Count, Is.LessThan(10)); 
	}

	// This test is a bit tricky without knowledge of the hash collisions, assuming a simple scenario.
	[Test]
	public void Remove_ReinsertInSameCluster()
	{
		// #16
		// Ideally, you'd find values which have the same hash, add them, remove one, and check the state.
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		hashSet.Remove(1);
		Assert.That(hashSet.Contains(2), Is.True);
	}

	// 5. GetEnumerator
	[Test]
	public void GetEnumerator_MultipleItems()
	{
		// #17
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		int sum = 0;
		foreach (var item in hashSet)
		{
			sum += item;
		}
		Assert.That(sum, Is.EqualTo(3));
	}

	[Test]
	public void GetEnumerator_AddAndRemove()
	{
		// #18
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(2);
		hashSet.Remove(1);
		int sum = 0;
		foreach (var item in hashSet)
		{
			sum += item;
		}
		Assert.That(sum, Is.EqualTo(2));
	}

	// 6. Hashing Mechanisms
	[Test]
	public void Hash_Collisions()
	{
		// #19
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(11);  // Assuming these produce a collision.
		Assert.That(hashSet.Count, Is.EqualTo(2));
	}

	[Test]
	public void Hash_RemoveCollidedItems()
	{
		// #20
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(11);  // Assuming these produce a collision.
		hashSet.Remove(1);
		Assert.That(hashSet.Contains(11), Is.True);
	}

	[Test]
	public void Hash_WrapAround()
	{
		// #21
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(5, Comparer<int>.Default);
		hashSet.Add(1);
		hashSet.Add(6);  // Assuming these wrap around.
		Assert.That(hashSet.Count, Is.EqualTo(2));
	}

	// 7. Resize
	[Test]
	public void Resize_OnAdd()
	{
		// #22
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(10, Comparer<int>.Default);
		for (int i = 0; i < 15; i++)
		{
			hashSet.Add(i);
		}
		Assert.That(hashSet.Count, Is.EqualTo(15));
	}

	[Test]
	public void Resize_OnRemove()
	{
		// #23
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<int>(20, Comparer<int>.Default);
		for (int i = 0; i < 15; i++)
		{
			hashSet.Add(i);
		}
		for (int i = 0; i < 13; i++)
		{
			hashSet.Remove(i);
		}
		Assert.That(hashSet.Count, Is.EqualTo(2));
	}

	// 8. Miscellaneous
	[Test]
	public void Ensure_ThrowIfNull()
	{
		// #24
		
		var hashSet = new Algorithms_Sedgewick.Set.HashSet<ObjectWithId>(ObjectWithId.Comparer);
		Assert.That(() => hashSet.Add(null), Throws.ArgumentNullException);
	}

	
}

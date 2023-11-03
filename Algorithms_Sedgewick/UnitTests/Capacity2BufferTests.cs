namespace UnitTests;

using Algorithms_Sedgewick.Buffer;
using NUnit.Framework;

[TestFixture]
public class Capacity2BufferTests
{
	[Test]
	public void InitializationTest()
	{
		var buffer = new Capacity2Buffer<int>();
		Assert.That(buffer.Count, Is.EqualTo(0));
		Assert.That(buffer.HasValue, Is.False);
		Assert.That(buffer.HasPreviousValue, Is.False);
	}

	[Test]
	public void InsertionTest()
	{
		var buffer = new Capacity2Buffer<int>();
		buffer.Insert(1);
		Assert.That(buffer.Count, Is.EqualTo(1));
		Assert.That(buffer.First, Is.EqualTo(1));
		Assert.That(buffer.Last, Is.EqualTo(1));

		buffer.Insert(2);
		Assert.That(buffer.Count, Is.EqualTo(2));
		Assert.That(buffer.First, Is.EqualTo(1));
		Assert.That(buffer.Last, Is.EqualTo(2));

		buffer.Insert(3);
		Assert.That(buffer.Count, Is.EqualTo(2));
		Assert.That(buffer.First, Is.EqualTo(2));
		Assert.That(buffer.Last, Is.EqualTo(3));
	}

	[Test]
	public void ClearTest()
	{
		var buffer = new Capacity2Buffer<int>();
		buffer.Insert(1);
		buffer.Insert(2);
		buffer.Clear();

		Assert.That(buffer.Count, Is.EqualTo(0));
		Assert.That(buffer.HasValue, Is.False);
		Assert.That(buffer.HasPreviousValue, Is.False);
	}

	[Test]
	public void AccessEmptyBufferTest()
	{
		var buffer = new Capacity2Buffer<int>();
		Assert.Throws<InvalidOperationException>(() => { _ = buffer.First; });
		Assert.Throws<InvalidOperationException>(() => { _ = buffer.Last; });
	}

	[Test]
	public void HasValueAndHasPreviousValueTest()
	{
		var buffer = new Capacity2Buffer<int>();
		buffer.Insert(1);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.HasPreviousValue, Is.False);

		buffer.Insert(2);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.HasPreviousValue, Is.True);
	}

	[Test]
	public void GetEnumeratorTest()
	{
		var buffer = new Capacity2Buffer<int>();
		buffer.Insert(1);
		
		using (var enumerator = buffer.GetEnumerator())
		{
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(1));
			Assert.That(enumerator.MoveNext(), Is.False);
		}

		buffer.Insert(2);
		
		using (var enumerator = buffer.GetEnumerator())
		{
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(1));
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(2));
			Assert.That(enumerator.MoveNext(), Is.False);
		}
	}

	[Test]
	public void InsertingNullValuesTest()
	{
		var buffer = new Capacity2Buffer<string>();
		buffer.Insert(null);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.First, Is.Null);
	}
}

[TestFixture]
public class OptimizedCapacity2BufferTests
{
	[Test]
	public void InitializationTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		Assert.That(buffer.Count, Is.EqualTo(0));
		Assert.That(buffer.HasValue, Is.False);
		Assert.That(buffer.HasPreviousValue, Is.False);
	}

	[Test]
	public void InsertionTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		buffer.Insert(1);
		Assert.That(buffer.Count, Is.EqualTo(1));
		Assert.That(buffer.First, Is.EqualTo(1));
		Assert.That(buffer.Last, Is.EqualTo(1));

		buffer.Insert(2);
		Assert.That(buffer.Count, Is.EqualTo(2));
		Assert.That(buffer.First, Is.EqualTo(1));
		Assert.That(buffer.Last, Is.EqualTo(2));

		buffer.Insert(3);
		Assert.That(buffer.Count, Is.EqualTo(2));
		Assert.That(buffer.First, Is.EqualTo(2));
		Assert.That(buffer.Last, Is.EqualTo(3));
	}

	[Test]
	public void ClearTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		buffer.Insert(1);
		buffer.Insert(2);
		buffer.Clear();

		Assert.That(buffer.Count, Is.EqualTo(0));
		Assert.That(buffer.HasValue, Is.False);
		Assert.That(buffer.HasPreviousValue, Is.False);
	}

	[Test]
	public void AccessEmptyBufferTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		Assert.Throws<InvalidOperationException>(() => { _ = buffer.First; });
		Assert.Throws<InvalidOperationException>(() => { _ = buffer.Last; });
	}

	[Test]
	public void HasValueAndHasPreviousValueTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		buffer.Insert(1);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.HasPreviousValue, Is.False);

		buffer.Insert(2);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.HasPreviousValue, Is.True);
	}

	[Test]
	public void GetEnumeratorTest()
	{
		var buffer = new OptimizedCapacity2Buffer<int>();
		buffer.Insert(1);
		
		using (var enumerator = buffer.GetEnumerator())
		{
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(1));
			Assert.That(enumerator.MoveNext(), Is.False);
		}

		buffer.Insert(2);
		
		using (var enumerator = buffer.GetEnumerator())
		{
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(1));
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.Current, Is.EqualTo(2));
			Assert.That(enumerator.MoveNext(), Is.False);
		}
	}

	[Test]
	public void InsertingNullValuesTest()
	{
		var buffer = new OptimizedCapacity2Buffer<string>();
		buffer.Insert(null);
		Assert.That(buffer.HasValue, Is.True);
		Assert.That(buffer.First, Is.Null);
	}
}

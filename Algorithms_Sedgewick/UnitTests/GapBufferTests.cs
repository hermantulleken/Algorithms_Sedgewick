using System.Collections.Generic;
using AlgorithmsSW.Stack;

namespace UnitTests;

using System;
using AlgorithmsSW.GapBuffer;
using NUnit.Framework;

[TestFixture]
public class GapBufferTests
{
	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_ToBeginning(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.MoveCursor(0);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(0));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_ToEnd(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.MoveCursor(gapBuffer.Count);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(gapBuffer.Count));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_ToMiddle(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.MoveCursor(1);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(1));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_InvalidIndex_ThrowsArgumentOutOfRangeException(IGapBuffer<int> gapBuffer)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => gapBuffer.MoveCursor(-1));
		Assert.Throws<ArgumentOutOfRangeException>(() => gapBuffer.MoveCursor(gapBuffer.Count + 1));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_MultipleMoves(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.MoveCursor(1);
		gapBuffer.MoveCursor(0);
		gapBuffer.MoveCursor(2);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(2));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_AfterAddRemoveOperations(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.AddAfter(4);
		gapBuffer.MoveCursor(1);
		gapBuffer.RemoveBefore();
		gapBuffer.MoveCursor(3);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(3));
	}

	[TestCaseSource(nameof(SingleElementGapBufferImplementations))]
	public void MoveCursor_SingleElementGapBuffer(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.AddAfter(1);
		gapBuffer.MoveCursor(0);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(0));
		gapBuffer.MoveCursor(1);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(1));
	}

	[TestCaseSource(nameof(GapBufferImplementations))]
	public void MoveCursor_MultipleElements(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.MoveCursor(0);
		gapBuffer.MoveCursor(1);
		gapBuffer.MoveCursor(2);
		gapBuffer.MoveCursor(3);
		Assert.That(gapBuffer.CursorIndex, Is.EqualTo(3));
	}
	
	[Test]
	public void AddBefore_GapBufferShouldGrowWhenFull()
	{
		// Arrange
		var gapBuffer = new GapBufferWithArray<int>(2); // Initial capacity of 2
		gapBuffer.AddBefore(1);
		gapBuffer.AddBefore(2);

		// Act
		gapBuffer.AddBefore(3); // This will trigger the Grow() method

		// Assert
		Assert.That(gapBuffer.Count, Is.EqualTo(3));
		Assert.That(gapBuffer[0], Is.EqualTo(1));
		Assert.That(gapBuffer[1], Is.EqualTo(2));
		Assert.That(gapBuffer[2], Is.EqualTo(3));
	}

	private static IEnumerable<TestCaseData> GapBufferImplementations()
	{
		yield return new TestCaseData(AddElementsToBuffer(new GapBufferWithArray<int>(5)))
			.SetName("GapBufferWithArray");
		
		yield return new TestCaseData(AddElementsToBuffer(new GapBufferWithStacks<int>(() => new StackWithResizeableArray<int>())))
			.SetName("GapBufferWithStacks");
	}
	
	private static IEnumerable<TestCaseData> SingleElementGapBufferImplementations()
	{
		yield return new TestCaseData(new GapBufferWithArray<int>(1))
			.SetName("SingleElementGapBufferWithArray");
		
		yield return new TestCaseData(new GapBufferWithStacks<int>(() => new StackWithResizeableArray<int>()))
			.SetName("SingleElementGapBufferWithStacks");
	}

	private static IGapBuffer<int> AddElementsToBuffer(IGapBuffer<int> gapBuffer)
	{
		gapBuffer.AddAfter(1);
		gapBuffer.AddAfter(2);
		gapBuffer.AddAfter(3);
		
		return gapBuffer;
	}
}

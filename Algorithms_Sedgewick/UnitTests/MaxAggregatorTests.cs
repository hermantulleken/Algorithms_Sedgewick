using System.Collections.Generic;
using AlgorithmsSW;

namespace UnitTests;

[TestFixture]
public class MaxAggregatorTests
{
	private readonly IComparer<int> intComparer = Comparer<int>.Default;
	
	[Test]
	public void MaxAggregator_InitiallyEmpty_ShouldNotHaveMaxValue()
	{
		var aggregator = new MaxAggregator<string, int>(intComparer);
		Assert.That(aggregator.HasMaxComparisonValue, Is.False);
	}
	
	[Test]
	public void AddIfBigger_NewMaxValue_ShouldUpdateMaxValue()
	{
		var aggregator = new MaxAggregator<string, int>(intComparer);
		aggregator.AddIfBigger("test", 10);
		
		Assert.That(aggregator.MaxComparisonComparisonValue, Is.EqualTo(10));
		Assert.That(aggregator.HasMaxComparisonValue, Is.True);
	}
	
	[Test]
	public void AddIfBigger_SmallerValue_ShouldNotChangeMaxValue()
	{
		var aggregator = new MaxAggregator<string, int>(intComparer);
		aggregator.AddIfBigger("initial", 10);
		aggregator.AddIfBigger("smaller", 5);
		
		Assert.That(aggregator.MaxComparisonComparisonValue, Is.EqualTo(10));
		Assert.That(aggregator.MaxValues, Has.Member("initial"));
		Assert.That(aggregator.MaxValues, Has.No.Member("smaller"));
	}
	
	[Test]
	public void AddIfBigger_SameValue_ShouldAddToMaxSet()
	{
		var aggregator = new MaxAggregator<string, int>(intComparer);
		aggregator.AddIfBigger("first", 10);
		aggregator.AddIfBigger("second", 10);
		
		Assert.That(aggregator.MaxValues, Has.Member("first"));
		Assert.That(aggregator.MaxValues, Has.Member("second"));
	}
	
	[Test]
	public void MaxValues_WhenEmpty_ShouldThrowInvalidOperationException()
	{
		var aggregator = new MaxAggregator<string, int>(intComparer);
		
		Assert.Throws<InvalidOperationException>(() => { _ = aggregator.MaxComparisonComparisonValue; });
	}
}

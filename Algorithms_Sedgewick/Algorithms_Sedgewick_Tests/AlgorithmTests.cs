using System.Collections.Generic;
using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using NUnit.Framework;

namespace Algorithms_Sedgewick_Tests;

[Parallelizable]
public class AlgorithmTests
{
	private static readonly ResizeableArray<int> Empty = new();
	private static readonly ResizeableArray<int> List135 = new(){ 1, 3, 5 };
	private static readonly ResizeableArray<int> List13335 = new(){ 1, 3, 3, 3, 5 };
	
	private static readonly IEnumerable<TestCaseData> FindTestCases = new List<TestCaseData>
	{
		new(Empty, 0) {ExpectedResult = 0, TestName = "Empty"},
		new(List135, 0) {ExpectedResult = 0, TestName = "Item before first"},
		new(List135, 6) {ExpectedResult = 3, TestName = "Item after last"},
		
		new(List135, 2) {ExpectedResult = 1, TestName = "Item after first"},
		new(List135, 4) {ExpectedResult = 2, TestName = "Item before last"},
		
		new(List135, 1) {ExpectedResult = 1, TestName = "Item at first"},
		new(List135, 3) {ExpectedResult = 2, TestName = "Item at second"},
		new(List135, 5) {ExpectedResult = 3, TestName = "Item at last"},
		new(List13335, 3) {ExpectedResult = 4, TestName = "Item at second to fourth"},
	};

	[TestCaseSource(nameof(FindTestCases))]
	public int TestEqualsSecond(IReadonlyRandomAccessList<int> list, int itemToPlace)
		=> list.FindInsertionIndex(itemToPlace);
}

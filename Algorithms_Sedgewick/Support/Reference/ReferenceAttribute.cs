namespace Support;

using System.Collections;

/// <summary>
/// Used to mark code to show where it comes from, or what problem it solves.
/// </summary>
/// <param name="reference">A string that denotes the source of code.</param>
/// <remarks> The source may include problems.</remarks>
public class ReferenceAttribute(string reference) : Attribute
{
	/// <inheritdoc/>
	public override string ToString() => reference;
}

public class PageReferenceAttribute(int pageNumber) : ReferenceAttribute($"Page {pageNumber}")
{
	public int PageNumber => pageNumber;
}

public class ExerciseReferenceAttribute(int chapterNumber, int sectionNumber, int exerciseNumber)
	: ReferenceAttribute($"Exercise {chapterNumber}.{sectionNumber}.{exerciseNumber}")
{
	public int ChapterNumber => chapterNumber;
	
	public int SectionNumber => sectionNumber;
	
	public int ExerciseNumber => exerciseNumber;
}

public class AlgorithmReferenceAttribute(int chapterNumber, int algorithmNumber)
	: ReferenceAttribute($"Algorithm {chapterNumber}.{algorithmNumber}")
{
	public int ChapterNumber => chapterNumber;
	
	public int AlgorithmNumber => algorithmNumber;
}

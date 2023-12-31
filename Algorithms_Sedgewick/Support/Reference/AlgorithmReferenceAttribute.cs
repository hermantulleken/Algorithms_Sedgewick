namespace Support.Reference;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class AlgorithmReferenceAttribute(int chapterNumber, int algorithmNumber)
	: ReferenceAttribute($"Algorithm {chapterNumber}.{algorithmNumber}")
{
	public int ChapterNumber => chapterNumber;
	
	public int AlgorithmNumber => algorithmNumber;
}

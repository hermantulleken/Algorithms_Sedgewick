namespace Support.Reference;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class ExerciseReferenceAttribute(int chapterNumber, int sectionNumber, int exerciseNumber)
	: ReferenceAttribute($"Exercise {chapterNumber}.{sectionNumber}.{exerciseNumber}")
{
	public int ChapterNumber => chapterNumber;
	
	public int SectionNumber => sectionNumber;
	
	public int ExerciseNumber => exerciseNumber;
}

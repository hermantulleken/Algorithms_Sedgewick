namespace Support.Reference;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class PageReferenceAttribute(int pageNumber) : ReferenceAttribute($"Page {pageNumber}")
{
	public int PageNumber => pageNumber;
}

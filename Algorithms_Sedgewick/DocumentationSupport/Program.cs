using System.Reflection;
using AlgorithmsSW;
using Support.Reference;

int[] chapterPage =
[
	3,
	243,
	361,
	515,
	695,
	853, 
	1000
];

var assembly = 
	Assembly.GetAssembly(typeof(Algorithms)) 
	?? throw new ($"Assembly {nameof(Algorithms)}not found.");

var references = GetReferences();
ProcessReferences();

Console.WriteLine("Markdown file generated.");

return;

void ProcessReferences()
{
	using var writer = new StreamWriter("output.md");
	
	for(int i = 0; i < 6; i++)
	{
		int chapter = i + 1;
		writer.WriteLine($"### Chapter {chapter}");
		
		var pages = GetPages(i);
		if (pages.Any())
		{
			writer.WriteLine($"#### Page References");
			WriteReferences(pages, writer);
		}
		
		
		var algorithms = GetAlgorithms(chapter);
		if (algorithms.Any())
		{
			writer.WriteLine($"#### Algorithms");
			WriteReferences(algorithms, writer);
		}
		
		for(int j = 0; j < 5; j++)
		{
			int section = j + 1;
			var examples = GetExamples(chapter, section);
			
			if(!examples.Any()) continue;
			
			writer.WriteLine($"#### Section {section}");
			WriteReferences(examples, writer);
		}
	}
}

List<(PageReferenceAttribute, object)> GetPages(int chapter)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber)
		.ToList();
	
	int GetNumber((PageReferenceAttribute pageReference, object _) reference)
		=> reference.pageReference.PageNumber;
	
	(PageReferenceAttribute, object) CastReference((ReferenceAttribute, object) reference)
		=> ((PageReferenceAttribute) reference.Item1, reference.Item2);
	
	bool Match((ReferenceAttribute, object) reference)
		=> reference.Item1 is PageReferenceAttribute pageReference
			&& IsInChapter(pageReference.PageNumber);
	
	bool IsInChapter(int page)
	{
		return page >= chapterPage[chapter] && page < chapterPage[chapter + 1];
	}
}

List<(ExerciseReferenceAttribute, object)> GetExamples(int chapter, int section)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber)
		.ToList();
	
	int GetNumber((ExerciseReferenceAttribute exerciseReference, object _) reference)
		=> reference.exerciseReference.ExerciseNumber;
	
	(ExerciseReferenceAttribute, object) CastReference((ReferenceAttribute, object) reference)
		=> ((ExerciseReferenceAttribute) reference.Item1, reference.Item2);
	
	bool Match((ReferenceAttribute, object) reference)
		=> reference.Item1 is ExerciseReferenceAttribute exerciseReference
			&& exerciseReference.ChapterNumber == chapter
			&& exerciseReference.SectionNumber == section;
}

List<(AlgorithmReferenceAttribute, object)> GetAlgorithms(int chapter)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber)
		.ToList();
	
	int GetNumber((AlgorithmReferenceAttribute algorithmReference, object _) reference)
		=> reference.algorithmReference.AlgorithmNumber;
	
	(AlgorithmReferenceAttribute, object) CastReference((ReferenceAttribute, object) reference)
		=> ((AlgorithmReferenceAttribute) reference.Item1, reference.Item2);
	
	bool Match((ReferenceAttribute, object) reference)
		=> reference.Item1 is AlgorithmReferenceAttribute algorithmReference
			&& algorithmReference.ChapterNumber == chapter;
}

List<(ReferenceAttribute, object)> GetReferences()
{
	List<(ReferenceAttribute, object)> newReferences = [];

	foreach (var type in assembly.GetTypes())
	{
		var attribute = type.GetCustomAttribute<ReferenceAttribute>();
		if (attribute != null)
		{
			newReferences.Add((attribute, type));
		}

		foreach (var method in type.GetMethods())
		{
			attribute = method.GetCustomAttribute<ReferenceAttribute>();
			if (attribute != null)
			{
				newReferences.Add((attribute, (type, method)));
			}
		}

		foreach (var property in type.GetProperties())
		{
			attribute = property.GetCustomAttribute<ReferenceAttribute>();
			if (attribute != null)
			{
				newReferences.Add((attribute, (type, property)));
			}
		}
	}

	return newReferences;
}

static string GetClassLink(ReferenceAttribute reference, Type type) 
	=> $"- {reference}: Class @{type.FullName}";

static string GetMethodLink(ReferenceAttribute reference, Type type, MethodInfo methodInfo) 
	=> $"- {reference}: Method {GetMethodSignature(type, methodInfo)}";

static string GetPropertyLink(ReferenceAttribute reference, Type type, MemberInfo property) 
	=> $"- {reference}: Property @{type.FullName}.{property.Name}";

static string GetMethodSignature(Type type, MethodInfo method) 
	=> $"@{type.FullName}.{method.Name}*";

void WriteReferences<TReference>(IEnumerable<(TReference, object)> valueTuples, StreamWriter streamWriter)
	where TReference : ReferenceAttribute
{
	foreach (var (reference, obj) in valueTuples)
	{
		switch (obj)
		{
			case Type type:
				streamWriter.WriteLine(GetClassLink(reference, type));
				break;
			case (Type type, MethodInfo methodInfo):
				streamWriter.WriteLine(GetMethodLink(reference, type, methodInfo));
				break;
			case (Type type, PropertyInfo propertyInfo):
				streamWriter.WriteLine(GetPropertyLink(reference, type, propertyInfo));
				break;
			default:
				throw new NotSupportedException();
		}
	}
}

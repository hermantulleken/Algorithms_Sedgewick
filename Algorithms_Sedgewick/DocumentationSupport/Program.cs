using System.Reflection;
using AlgorithmsSW;
using Support;

int[] chapterPage = new int[]
{
	3,
	243,
	361,
	515,
	695,
	853, 
	1000
};

var assembly = Assembly.GetAssembly(typeof(Algorithms));
var references = GetReferences();
ProcessReferences(references);

Console.WriteLine("Markdown file generated.");

return;

void ProcessReferences(List<(ReferenceAttribute, object)> references)
{
	using var writer = new StreamWriter("output.md");
	
	for(int i = 0; i < 6; i++)
	{
		int chapter = i + 1;
		writer.WriteLine($"### Chapter {chapter}");
		writer.WriteLine();
		
		var pages = GetPages(references, i);
		if (pages.Any())
		{
			writer.WriteLine($"#### Page References");
			WriteReferences(pages, writer);
		}
		
		
		var algorithms = GetAlgorithms(references, chapter);
		if (algorithms.Any())
		{
			writer.WriteLine($"#### Algorithms");
			WriteReferences(algorithms, writer);
		}
		
		for(int j = 0; j < 5; j++)
		{
			int section = j + 1;
			var examples = GetExamples(references, chapter, section);
			
			if(!examples.Any()) continue;
			
			writer.WriteLine($"#### Section {section}");
			writer.WriteLine();
			
			WriteReferences(examples, writer);
		}
	}
}

IEnumerable<(PageReferenceAttribute, object)> GetPages(List<(ReferenceAttribute, object)> references, int chapter)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber);
	
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

IEnumerable<(ExerciseReferenceAttribute, object)> GetExamples(List<(ReferenceAttribute, object)> references, int chapter, int section)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber);
	
	int GetNumber((ExerciseReferenceAttribute exerciseReference, object _) reference)
		=> reference.exerciseReference.ExerciseNumber;
	
	(ExerciseReferenceAttribute, object) CastReference((ReferenceAttribute, object) reference)
		=> ((ExerciseReferenceAttribute) reference.Item1, reference.Item2);
	
	bool Match((ReferenceAttribute, object) reference)
		=> reference.Item1 is ExerciseReferenceAttribute exerciseReference
			&& exerciseReference.ChapterNumber == chapter
			&& exerciseReference.SectionNumber == section;
}

IEnumerable<(AlgorithmReferenceAttribute, object)> GetAlgorithms(List<(ReferenceAttribute, object)> references, int chapter)
{
	return references
		.Where(Match)
		.Select(CastReference)
		.OrderBy(GetNumber);
	
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
	List<(ReferenceAttribute, object)> references = [];

	foreach (var type in assembly.GetTypes())
	{
		var attribute = type.GetCustomAttribute<ReferenceAttribute>();
		if (attribute != null)
		{
			references.Add((attribute, type));
		}

		foreach (var method in type.GetMethods())
		{
			attribute = method.GetCustomAttribute<ReferenceAttribute>();
			if (method.GetCustomAttribute<ReferenceAttribute>() != null)
			{
				references.Add((attribute, (type, method)));
			}
		}
	}

	return references;
}

static string GetClassLink(ReferenceAttribute reference, Type type) 
	=> $"- {reference}: @{type.FullName}";

static string GetMethodLink(ReferenceAttribute reference, Type type, MethodInfo methodInfo) 
	=> $"- {reference}: {GetMethodSignature(type, methodInfo)}";

static string GetMethodSignature(Type type, MethodInfo method)
{
	return $"@{type.FullName}.{method.Name}*";
}


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
			default:
				throw new NotSupportedException();
		}
	}
}

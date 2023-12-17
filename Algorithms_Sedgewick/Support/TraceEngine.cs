using System.Diagnostics;

namespace Support;

public class TraceEngine
{
	public int currentLevel = 0;
	
	private readonly List<TraceElement> trace = [];
	
	public bool WriteToConsole = true;
	
	public IEnumerable<TraceElement> TraceList => trace;
	
	public event Action<TraceElement> EventTraced;
	
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public void IncLevel()
	{
		currentLevel++;
	}

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public void DecLevel()
	{
		if (currentLevel == 0)
		{
			throw new InvalidOperationException("Cannot decrement level below zero.");
		}
		
		currentLevel--;
	}

	public void Trace<T>(string name, T value) => AddTrace(new TraceElement(currentLevel, name, value.AsText()));

	public void Trace(string name) => AddTrace( new TraceElement(currentLevel, name));

	public void TraceIteration<TIndex, T>(TIndex index, T value) => Trace(index.AsText(), value);

	public void TraceIteration<TIndex>(TIndex index) => Trace(index.AsText());

	public override string ToString() => trace.AsText(Formatter.NewLine);

	public void Clear() => trace.Clear();

	public void ResetLevel() => currentLevel = 0;

	public void Reset()
	{
		Clear();
		ResetLevel();
	}
	
	private void AddTrace(TraceElement traceElement)
	{
		trace.Add(traceElement);
		EventTraced?.Invoke(traceElement);
		
		if (WriteToConsole)
		{
			Console.WriteLine(traceElement.ToString());
		}
	}
}

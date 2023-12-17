using System.Diagnostics;

namespace Support;

/// <summary>
/// Provides a simple way to trace the execution of algorithms.
/// </summary>
/// <remarks>
/// To use this class, you must define the WITH_INSTRUMENTATION conditional compilation symbol. You need to call
/// <see cref="Init"/> at the beginning of your program. Then, you can use the <see cref="Trace"/> methods to trace
/// execution of algorithms. 
/// </remarks>
/// <example>
/// Here is how to the tracer in a recursive implementation of the Fibonacci algorithm:
/// [!code-csharp[](../../AlgorithmsSW/TestAlgorithms.cs#TraceExample)]
/// </example>
public static class Tracer
{
	private static TraceEngine? traceEngine;
	
	/// <summary>
	/// Gets or sets a value indicating whether the trace events should be written to the console.
	/// </summary>
	/// <seealso cref="TraceList"/>
	public static bool WriteToConsole
	{
		get => TraceEngine.WriteToConsole;
		set => TraceEngine.WriteToConsole = value;
	}

	/// <summary>
	/// An event that is raised when a trace event is recorded.
	/// </summary>
	public static event Action<TraceElement> EventTraced
	{
		add => TraceEngine.EventTraced += value;
		remove => TraceEngine.EventTraced -= value;
	}
	
	public static IEnumerable<TraceElement> TraceList => TraceEngine.TraceList;
	
	/// <summary>
	/// Gets all the trace events recorded so far.
	/// </summary>
	private static TraceEngine TraceEngine => traceEngine ??= new TraceEngine();
	
	/// <summary>
	/// Initializes the tracing process. Should be called at the beginning of the program.
	/// </summary>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void Init() => TraceEngine.Reset();

	/// <summary>
	/// Records a trace event with a specific name and an associated value.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with the trace event.</typeparam>
	/// <param name="name">The name of the trace event.</param>
	/// <param name="value">The value associated with the trace event.</param>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void Trace<T>(string name, T value) => TraceEngine.Trace(name, value);

	/// <summary>
	/// Records a trace event with a specific name.
	/// </summary>
	/// <param name="name">The name of the trace event.</param>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void Trace(string name) => TraceEngine.Trace(name);

	/// <summary>
	/// Records a trace event for an iteration, capturing both the index and the value of the current iteration.
	/// </summary>
	/// <typeparam name="TIndex">The type of the index used in the iteration.</typeparam>
	/// <typeparam name="T">The type of the value in the current iteration.</typeparam>
	/// <param name="index">The index of the current iteration.</param>
	/// <param name="value">The value of the current iteration.</param>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void TraceIteration<TIndex, T>(TIndex index, T value) => TraceEngine.TraceIteration(index, value);

	/// <summary>
	/// Records a trace event for an iteration, capturing the index of the current iteration.
	/// </summary>
	/// <typeparam name="TIndex">The type of the index used in the iteration.</typeparam>
	/// <param name="index">The index of the current iteration.</param>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void TraceIteration<TIndex>(TIndex index) => TraceEngine.TraceIteration(index);

	/// <summary>
	/// Records a trace event for an iteration with a specific name, capturing the index of the current iteration.
	/// </summary>
	/// <typeparam name="TIndex">The type of the index used in the iteration.</typeparam>
	/// <param name="name">The name of the trace event.</param>
	/// <param name="index">The index of the current iteration.</param>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void TraceIteration<TIndex>(string name, TIndex index) => TraceEngine.Trace(name, index);

	/// <summary>
	/// Increases the indentation level for tracing. Should be paired with a corresponding call to <see cref="DecLevel"/>.
	/// </summary>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void IncLevel() => TraceEngine.IncLevel();

	/// <summary>
	/// Decreases the indentation level for tracing. Should be paired with a corresponding call to <see cref="IncLevel"/>.
	/// </summary>
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	public static void DecLevel() => TraceEngine.DecLevel();
}

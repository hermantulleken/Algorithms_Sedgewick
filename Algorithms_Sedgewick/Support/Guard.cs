namespace Support;

/// <summary>
/// A class that can be used to count things (typically executions) and throw an exception if the count exceeds a limit. 
/// </summary>
/// <param name="defaultLimit">The default limit that is used if no limit is in <see cref="Reset()"/>.</param>
/// <param name="limitExceededMessage">The error message to display when the limit is exceeded.</param>
/// <param name="incDecMismatchMessage">The error message to display when <see cref="Dec"/> has been called more than
/// <see cref="Inc"/>.</param>
/// <remarks>
/// This class is used to implement <see cref="RecursionDepthGuard"/> and <see cref="IterationGuard"/>.
///
/// This class can be used as a straight forward counter (for example, to count the number of iterations in a loop),
/// or as a counter that can be incremented and decremented (for example, to count the recursion depth).
/// </remarks>
public class Guard(int defaultLimit, string limitExceededMessage, string incDecMismatchMessage)
{
	private int counter;
	private int limit;

	/// <summary>
	/// Resets the counter to zero and sets the limit to the default value.
	/// </summary>
	public void Reset()
	{
		counter = 0;
		limit = defaultLimit;
	}

	/// <summary>
	/// Resets the counter to zero and sets the limit to the specified value.
	/// </summary>
	/// <param name="limit">The limit.</param>
	public void Reset(int limit)
	{
		counter = 0;
		this.limit = limit;
	}
	
	/// <summary>
	/// Increments the counter.
	/// </summary>
	/// <exception cref="InvalidOperationException">The limit is exceeded.</exception>
	public void Inc()
	{
		counter++;

		if (counter > limit)
		{
			throw new InvalidOperationException(limitExceededMessage);
		}
	}

	/// <summary>
	/// Decrements the counter.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="Dec"/>has been called more than <see cref="Inc"/>.
	/// </exception>
	/// <remarks>
	/// Dec should never be called more times than <see cref="Inc"/>; the idea is that this keeps track of
	/// scope exists (while Inc keeps track of scope enters).
	/// </remarks>
	/// <seealso cref="RecursionDepthGuard"/>
	public void Dec()
	{
		counter--;

		if (counter < 0)
		{
			throw new InvalidOperationException(incDecMismatchMessage);
		}
	}
}
namespace Algorithms_Sedgewick.ValueSnapshot;

/// <summary>
/// Represents a tracker that detects when a value crosses a specified threshold.
/// </summary>
/// <typeparam name="T">The type of the value being tracked.</typeparam>
public class ThresholdCrossTracker<T> : ValueSnapshot<T>
{
	private readonly IComparer<T> comparer;
	
	/// <summary>
	/// Gets or sets a value indicating whether tracking is enabled for this <see cref="ThresholdCrossTracker{T}"/>.
	/// </summary>
	/// <remarks>When <see langword="false"/>, none of this type's events will be raised.</remarks>
	public bool TrackingEnabled { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ThresholdCrossTracker{T}"/> class.
	/// </summary>
	/// <param name="threshold">The threshold value against which crossings are detected.</param>
	/// <param name="comparer">The comparer used to determine the order of values. If not provided, the default comparer for type <typeparamref name="T"/> is used.</param>
	public ThresholdCrossTracker(T threshold, IComparer<T>? comparer = null)
	{
		this.comparer = comparer ?? Comparer<T>.Default;
		Threshold = threshold;
		TrackingEnabled = true;
	}

	/// <summary>
	/// Occurs when the current value crosses the specified threshold in any direction.
	/// </summary>
	public event Action<ThresholdCrossTracker<T>>? ThresholdCrossed;

	/// <summary>
	/// Occurs when the current value crosses the specified threshold in an upwards direction.
	/// </summary>
	public event Action<ThresholdCrossTracker<T>>? ThresholdCrossedUpwards;

	/// <summary>
	/// Occurs when the current value crosses the specified threshold in a downwards direction.
	/// </summary>
	public event Action<ThresholdCrossTracker<T>>? ThresholdCrossedDownwards;

	/// <summary>
	/// Gets or sets the current value of this tracker.
	/// </summary>
	/// <remarks>
	/// When setting the value, the tracker checks if the value has crossed the specified threshold.
	/// If a threshold crossing is detected, the appropriate events (<see cref="ThresholdCrossed"/>, <see cref="ThresholdCrossedUpwards"/>, or <see cref="ThresholdCrossedDownwards"/>) are raised.
	/// </remarks>
	public override T? Value
	{
		get => base.Value;
		
		set
		{
			base.Value = value;

			if (!HasPreviousValue)
			{
				return;
			}

			if (!TrackingEnabled)
			{
				return;
			}
			
			int previousComparison = comparer.Compare(PreviousValue, Threshold);
			int currentComparison = comparer.Compare(Value, Threshold);

			if (previousComparison < 0 && currentComparison >= 0)
			{
				ThresholdCrossedUpwards?.Invoke(this);
				ThresholdCrossed?.Invoke(this);
			}
			else if (previousComparison > 0 && currentComparison <= 0)
			{
				ThresholdCrossedDownwards?.Invoke(this);
				ThresholdCrossed?.Invoke(this);
			}
		}
	}

	/// <summary>
	/// Gets the threshold value against which crossings are detected.
	/// </summary>
	public T Threshold { get; private set; }
}

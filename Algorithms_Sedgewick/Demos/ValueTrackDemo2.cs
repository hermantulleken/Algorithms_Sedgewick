using AlgorithmsSW;
using AlgorithmsSW.ValueSnapshot;

namespace Demos;

/// <summary>
/// Shows how to track multiple values and fire events in a manipulated order.
/// </summary>
public sealed class ValueTrackDemo2 : IDisposable
{
	public event Action<int, bool> ValueChanged;
	
	private readonly List<bool> hasChanged = new();
	private readonly List<ValueChangeTracker<bool>> trackers = new();
	private readonly List<Action> unsubscribeActions = new();

	private bool disposed = false;
	public ValueTrackDemo2()
	{
		for (int i = 0; i < 10; i++)
		{
			void TrackerOnValueChanged<T>(ValueChangeTracker<T> obj)
			{
				hasChanged[i] = true;
			}
			
			void Unsubscribe()
			{
				trackers[i].ValueChanged -= TrackerOnValueChanged;
			}
			
			unsubscribeActions.Add(Unsubscribe);

			var tracker = new ValueChangeTracker<bool>();
			tracker.ValueChanged += TrackerOnValueChanged;
			
			trackers.Add(tracker);
			hasChanged.Add(false);
		}
	}

	/// <summary>
	/// Call this to check if any values have changed and raise the <see cref="ValueChanged"/> event if they have.
	/// </summary>
	/// <remarks>
	/// <see cref="ValueChanged"/> is raised first for changed values that are true, then for changed values that are false.
	/// </remarks>
	/// <exception cref="ObjectDisposedException">this object has been disposed.</exception>
	public void Update()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(nameof(ValueTrackDemo2));
		}
		
		UpdateValues();
		
		for (int i = 0; i < trackers.Count; i++)
		{
			if (hasChanged[i] && trackers[i].Value)
			{
				ValueChanged?.Invoke(i, trackers[i].Value);
			}
		}
		
		for (int i = 0; i < trackers.Count; i++)
		{
			if (hasChanged[i] && !trackers[i].Value)
			{
				ValueChanged?.Invoke(i, trackers[i].Value);
			}
		}

		for (int i = 0; i < trackers.Count; i++)
		{
			hasChanged[i] = false;
		}
	}

	private void UpdateValues()
	{
		// Update values here.
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	private void Dispose(bool disposing)
	{
		if (disposed || !disposing)
		{
			return;
		}
		
		foreach (var action in unsubscribeActions)
		{
			action();
		}

		disposed = true;
	}
	
#pragma warning disable SA1600
	~ValueTrackDemo2() => Dispose(false);
#pragma warning restore SA1600
}

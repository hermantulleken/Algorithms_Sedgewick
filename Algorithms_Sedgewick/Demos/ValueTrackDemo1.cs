using AlgorithmsSW;
using AlgorithmsSW.ValueSnapshot;

namespace Demos;

/// <summary>
/// This shows how to use <see cref="ValueChangeTracker{T}"/> to track changes to multiple values, and fire them
/// in a prioritized fashion.
/// </summary>
public sealed class ValueTrackDemo1 : IDisposable
{
	private readonly Dictionary<object, bool> hasChanged;

	private readonly ValueChangeTracker<bool> tracker1;
	private readonly ValueChangeTracker<int> tracker2;
	private readonly ValueChangeTracker<string?> tracker3;

	private bool disposed = false;

	public event Action<bool> Value1Changed;

	public event Action<int> Value2Changed;

	public event Action<string?> Value3Changed;

	public ValueTrackDemo1()
	{
		tracker1 = new ValueChangeTracker<bool>();
		tracker2 = new ValueChangeTracker<int>();
		tracker3 = new ValueChangeTracker<string?>();
		hasChanged = new Dictionary<object, bool>
		{
			[tracker1] = false,
			[tracker2] = false,
			[tracker3] = false,
		};

		tracker1.ValueChanged += TrackerOnValueChanged;
		tracker2.ValueChanged += TrackerOnValueChanged;
		tracker3.ValueChanged += TrackerOnValueChanged;
	}

	private void TrackerOnValueChanged<T>(ValueChangeTracker<T> obj)
	{
		hasChanged[obj] = true;
	}

	public void Update()
	{
		if (disposed)
		{
			throw new ObjectDisposedException(nameof(ValueTrackDemo1));
		}
		
		UpdateValues();

		if (hasChanged[tracker1])
		{
			Value1Changed?.Invoke(tracker1.Value);
		}
		else if (hasChanged[tracker2])
		{
			Value2Changed?.Invoke(tracker2.Value);
		}
		else if (hasChanged[tracker3])
		{
			Value3Changed?.Invoke(tracker3.Value);
		}

		foreach (object key in hasChanged.Keys)
		{
			hasChanged[key] = false;
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
		if (disposed)
		{
			return;
		}

		if (disposing)
		{
			tracker1.ValueChanged -= TrackerOnValueChanged;
			tracker2.ValueChanged -= TrackerOnValueChanged;
			tracker3.ValueChanged -= TrackerOnValueChanged;
		}

		disposed = true;
	}

	~ValueTrackDemo1()
	{
		Dispose(false);
	}
}

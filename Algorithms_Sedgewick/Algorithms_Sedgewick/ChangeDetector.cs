using Algorithms_Sedgewick.Buffer;

namespace Algorithms_Sedgewick;

public class ChangeDetector<T>
{
	private readonly IBuffer<T> buffer;
	private readonly IEqualityComparer<T> comparer;

	public event Action<T, T> OnValueChanged;
    
	public T Value
	{
		get => buffer.Last;

		set
		{
			buffer.Insert(value);
            
			if (buffer.Count == 2 && !comparer.Equals(Value, PreviousValue))
			{
				OnValueChanged?.Invoke(Value, PreviousValue);
			}
		}
	}
    
	public T PreviousValue => buffer.First;

	public ChangeDetector(IEqualityComparer<T> comparer = null)
	{
		this.comparer = comparer ?? EqualityComparer<T>.Default;
		buffer = new FullCapacity2Buffer<T>();
	}

	public void Clear() => buffer.Clear();
}

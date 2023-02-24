namespace Algorithms_Sedgewick.Buffer;

internal static class Buffer
{
	internal static Exception EmptyBufferInvalid() 
		=> new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
}